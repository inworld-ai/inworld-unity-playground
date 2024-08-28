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
    public abstract class Task : ScriptableObject
    {
        public string ID => m_Id;
        [SerializeField] private string m_Id;
        public abstract IEnumerator Perform(InworldCharacter inworldCharacter, Dictionary<string, string> parameters);
    }

    public abstract class ItemTask : Task
    {
        [SerializeField] protected float m_ItemNearbyThreshold = 2;
        
        protected virtual bool IsItemNearby(InworldCharacter inworldCharacter, EntityItem entityItem)
        {
            return Vector2.Distance(new Vector2(inworldCharacter.transform.position.x, inworldCharacter.transform.position.z),
                       new Vector2(entityItem.transform.position.x, entityItem.transform.position.z)) <= m_ItemNearbyThreshold;
        }
    }
}
