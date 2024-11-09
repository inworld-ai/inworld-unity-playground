/*************************************************************************************************
 * Copyright 2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/
using Inworld.BehaviorEngine;
using UnityEngine;

namespace Inworld.Map
{
    [RequireComponent(typeof(EntityItem))]
    public class EntityItemKey : MonoBehaviour
    {
        [SerializeField] private float m_UseRadius = 3;
        
        public virtual void Use(InworldCharacter inworldCharacter)
        {
            Collider[] colliders = Physics.OverlapSphere(inworldCharacter.transform.position, m_UseRadius);
            foreach (Collider collider in colliders)
            {
                EntityItemLock entityItemLock = collider.GetComponent<EntityItemLock>();
                if (entityItemLock)
                    entityItemLock.Unlock(this);
            }
        }
    }
}
