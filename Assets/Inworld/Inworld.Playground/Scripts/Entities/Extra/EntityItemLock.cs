/*************************************************************************************************
 * Copyright 2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/
using UnityEngine;
using UnityEngine.Events;

namespace Inworld.Map
{ 
    [RequireComponent(typeof(EntityItem))]
    [RequireComponent(typeof(Collider))]
    public class EntityItemLock : MonoBehaviour
    {
        public UnityEvent onUnlocked;
        
        [SerializeField] private EntityItemKey m_Key;
        private EntityItem m_EntityItem;

        protected virtual void Awake()
        {
            m_EntityItem = GetComponent<EntityItem>();
        }

        public virtual bool Unlock(EntityItemKey entityItemKey)
        {
            if (entityItemKey == m_Key)
            {
                Destroy(this);
                InworldAI.Log($"Unlocked {m_EntityItem.DisplayName}");
                m_EntityItem.UpdateProperty("lock", "Unlocked.");
                onUnlocked?.Invoke();
                return true;
            }
            return false;
        }
    }
}
