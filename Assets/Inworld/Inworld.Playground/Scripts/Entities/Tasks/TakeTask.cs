/*************************************************************************************************
 * Copyright 2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using Inworld.Packet;
using UnityEngine;

namespace Inworld.Map
{
    [CreateAssetMenu(fileName = "TakeTask", menuName = "Inworld/Tasks/TakeTask")]
    public class TakeTask : ItemTask
    {
        public override IEnumerator Perform(InworldCharacter inworldCharacter, List<TriggerParameter> parameters)
        {
            Dictionary<string, string> parameterDictionary = ParseParameters(parameters);
            if (!parameterDictionary.TryGetValue("what", out string whatItemID) || !EntityManager.Instance.FindItem(whatItemID, out EntityItem whatEntityItem))
            {
                EntityManager.Instance.FailTask(this, inworldCharacter, $"Failed to find item: {whatItemID}.");
                yield break;
            }

            if (!IsItemNearby(inworldCharacter, whatEntityItem))
            {
                EntityManager.Instance.FailTask(this, inworldCharacter, $"Item is too far away: {whatItemID}.");
                yield break;
            }
            
            if (parameterDictionary.TryGetValue("from", out string fromItemID) && EntityManager.Instance.FindItem(fromItemID, out EntityItem fromEntityItem))
            {
                if (!IsItemNearby(inworldCharacter, fromEntityItem))
                {
                    EntityManager.Instance.FailTask(this, inworldCharacter, $"Item is too far away: {fromItemID}.");
                    yield break;
                }
            }

            Inventory inventory = inworldCharacter.GetComponent<Inventory>();
            if (!inventory || !inventory.AddItem(whatEntityItem))
            {
                EntityManager.Instance.FailTask(this, inworldCharacter, $"{inworldCharacter.Name} already has item: {whatItemID}.");
                yield break;
            }
            whatEntityItem.gameObject.SetActive(false);
            EntityManager.Instance.CompleteTask(this, inworldCharacter);
        }


    }
}
