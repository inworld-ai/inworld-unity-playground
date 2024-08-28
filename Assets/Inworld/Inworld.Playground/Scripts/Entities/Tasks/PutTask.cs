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
    [CreateAssetMenu(fileName = "PutTask", menuName = "Inworld/Tasks/PutTask")]
    public class PutTask : ItemTask
    {
        [SerializeField] private float m_ItemPlacementYOffset = 1;
        public override IEnumerator Perform(InworldCharacter inworldCharacter, Dictionary<string, string> parameters)
        {
            if (!parameters.TryGetValue("what", out string whatItemID) || !EntityManager.Instance.FindItem(whatItemID, out EntityItem whatEntityItem))
            {
                EntityManager.Instance.FailTask(this, inworldCharacter, $"Failed to find item: {whatItemID}.", parameters);
                yield break;
            }

            Inventory inventory = inworldCharacter.GetComponent<Inventory>();
            if (!inventory || !inventory.Contains(whatItemID))
            {
                EntityManager.Instance.FailTask(this, inworldCharacter, $"{inworldCharacter.Name} does not have the item: {whatEntityItem}.", parameters);
                yield break;
            }
            
            if (!parameters.TryGetValue("where", out string whereItemID) || !EntityManager.Instance.FindItem(whereItemID, out EntityItem whereEntityItem))
            {
                EntityManager.Instance.FailTask(this, inworldCharacter, $"Failed to find item: {whereItemID}.", parameters);
                yield break;
            }
            
            if (!IsItemNearby(inworldCharacter, whereEntityItem))
            {
                EntityManager.Instance.FailTask(this, inworldCharacter, $"Item is too far away: {whereItemID}.", parameters);
                yield break;
            }
            
            Collider itemCollider = whereEntityItem.GetComponentInChildren<Collider>();
            if (!itemCollider)
            {
                EntityManager.Instance.FailTask(this, inworldCharacter, $"Item can not have objects placed on top of it: {whereItemID}.", parameters);
                yield break;
            }

            whatEntityItem.transform.position = itemCollider.bounds.center + new Vector3(0, itemCollider.bounds.extents.y + m_ItemPlacementYOffset, 0);
            
            inventory.RemoveItem(whatEntityItem);
            whatEntityItem.gameObject.SetActive(true);
            InworldAI.Log($"{inworldCharacter.Name} placed item: {whatEntityItem.DisplayName} on {whereEntityItem.DisplayName}");
            whatEntityItem.UpdateProperty("location", $"On {whereEntityItem.DisplayName}.");
            EntityManager.Instance.CompleteTask(this, inworldCharacter, parameters);
        }
    }
}
