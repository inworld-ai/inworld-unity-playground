/*************************************************************************************************
 * Copyright 2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/


using Inworld;
using Inworld.BehaviorEngine;
using UnityEngine;

namespace Playground
{
    public abstract class ItemTaskHandler : TaskHandler
    {
        [SerializeField] protected float m_ItemNearbyThreshold = 2;
        
        protected virtual bool IsItemNearby(InworldCharacter inworldCharacter, EntityItem entityItem)
        {
            return Vector2.Distance(new Vector2(inworldCharacter.transform.position.x, inworldCharacter.transform.position.z),
                       new Vector2(entityItem.transform.position.x, entityItem.transform.position.z)) <= m_ItemNearbyThreshold;
        }
    }
}

