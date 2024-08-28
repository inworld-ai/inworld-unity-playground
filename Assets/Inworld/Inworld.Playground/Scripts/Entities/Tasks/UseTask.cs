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
using UnityEngine.AI;

namespace Inworld.Map
{
    [CreateAssetMenu(fileName = "UseTask", menuName = "Inworld/Tasks/UseTask")]
    public class UseTask : Task
    {
        public override IEnumerator Perform(InworldCharacter inworldCharacter, Dictionary<string, string> parameters)
        {
            if (parameters.TryGetValue("what", out string itemID) && EntityManager.Instance.FindItem(itemID, out EntityItem entityItem))
            {
                Inventory inventory = inworldCharacter.GetComponent<Inventory>();
                if (!inventory || !inventory.Contains(itemID))
                {
                    EntityManager.Instance.FailTask(this, inworldCharacter, $"{inworldCharacter.Name} does not have the item: {itemID}.", parameters);
                    yield break;
                }

                EntityItemKey entityItemKey = entityItem.GetComponent<EntityItemKey>();
                if (entityItemKey)
                    entityItemKey.Use(inworldCharacter);
                
                InworldAI.Log($"{inworldCharacter.Name} uses {entityItem.DisplayName}.");
                EntityManager.Instance.CompleteTask(this, inworldCharacter, parameters);
            } else 
                EntityManager.Instance.FailTask(this, inworldCharacter, $"Failed to find item: {itemID}.", parameters);
        }
    }
}
