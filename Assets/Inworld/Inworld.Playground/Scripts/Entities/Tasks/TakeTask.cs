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
        public override IEnumerator Perform(InworldCharacter inworldCharacter, Dictionary<string, string> parameters)
        {
            if (!parameters.TryGetValue("what", out string whatItemID) || !EntityManager.Instance.FindItem(whatItemID, out EntityItem whatEntityItem))
            {
                EntityManager.Instance.FailTask(this, inworldCharacter, $"Failed to find item: {whatItemID}.", parameters);
                yield break;
            }
            
            Inventory inventory = inworldCharacter.GetComponent<Inventory>();
            if (!inventory)
            {
                EntityManager.Instance.FailTask(this, inworldCharacter, $"{inworldCharacter.Name} can not carry items.", parameters);
                yield break;
            }
            
            if (inventory.Contains(whatItemID))
            {
                EntityManager.Instance.FailTask(this, inworldCharacter, $"{inworldCharacter.Name} already has item: {whatItemID}.", parameters);
                yield break;
            }

            if (!IsItemNearby(inworldCharacter, whatEntityItem))
            {
                EntityManager.Instance.FailTask(this, inworldCharacter, $"Item is too far away: {whatItemID}.", parameters);
                yield break;
            }
            
            if (parameters.TryGetValue("from", out string fromItemID) && EntityManager.Instance.FindItem(fromItemID, out EntityItem fromEntityItem))
            {
                if (!IsItemNearby(inworldCharacter, fromEntityItem))
                {
                    EntityManager.Instance.FailTask(this, inworldCharacter, $"Item is too far away: {fromItemID}.", parameters);
                    yield break;
                }
            }
            
            if (!inventory.AddItem(whatEntityItem))
            {
                EntityManager.Instance.FailTask(this, inworldCharacter, $"{inworldCharacter.Name} could not pick up item: {whatItemID}.", parameters);
                yield break;
            }
            whatEntityItem.gameObject.SetActive(false);
            InworldAI.Log($"{inworldCharacter.Name} took item: {whatEntityItem.DisplayName}.");
            whatEntityItem.UpdateProperty("location", $"In {inworldCharacter.Name}'s inventory.");
            EntityManager.Instance.CompleteTask(this, inworldCharacter, parameters);
        }


    }
}
