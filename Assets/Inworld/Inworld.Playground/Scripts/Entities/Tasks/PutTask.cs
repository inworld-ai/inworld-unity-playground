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
        public override IEnumerator Perform(InworldCharacter inworldCharacter, List<TriggerParameter> parameters)
        {
            Dictionary<string, string> parameterDictionary = ParseParameters(parameters);
            if (!parameterDictionary.TryGetValue("what", out string whatItemID) || !EntityManager.Instance.FindItem(whatItemID, out EntityItem whatEntityItem))
            {
                EntityManager.Instance.FailTask(this, inworldCharacter, $"Failed to find item: {whatItemID}.");
                yield break;
            }

            Inventory inventory = inworldCharacter.GetComponent<Inventory>();
            if (!inventory || !inventory.Contains(whatItemID))
            {
                EntityManager.Instance.FailTask(this, inworldCharacter, $"{inworldCharacter.Name} does not have the item: {whatEntityItem}.");
                yield break;
            }
            
            if (!parameterDictionary.TryGetValue("where", out string whereItemID) || !EntityManager.Instance.FindItem(whereItemID, out EntityItem whereEntityItem))
            {
                EntityManager.Instance.FailTask(this, inworldCharacter, $"Failed to find item: {whereItemID}.");
                yield break;
            }
            
            if (!IsItemNearby(inworldCharacter, whereEntityItem))
            {
                EntityManager.Instance.FailTask(this, inworldCharacter, $"Item is too far away: {whereItemID}.");
                yield break;
            }
            
            Collider itemCollider = whereEntityItem.GetComponentInChildren<Collider>();
            if (!itemCollider)
            {
                EntityManager.Instance.FailTask(this, inworldCharacter, $"Item can not have objects placed on top of it: {whereItemID}.");
                yield break;
            }

            whatEntityItem.transform.position = itemCollider.bounds.center + new Vector3(0, itemCollider.bounds.extents.y + m_ItemPlacementYOffset, 0);
            
            inventory.RemoveItem(whatEntityItem);
            whatEntityItem.gameObject.SetActive(true);
            EntityManager.Instance.CompleteTask(this, inworldCharacter);
        }
    }
}
