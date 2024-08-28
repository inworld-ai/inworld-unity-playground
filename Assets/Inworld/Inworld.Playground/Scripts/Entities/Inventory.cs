/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace Inworld.Map
{
    public class Inventory : MonoBehaviour
    {
        protected readonly Dictionary<string, EntityItem> m_Items = new Dictionary<string, EntityItem>();
        
        public bool AddItem(EntityItem item)
        {
            if (!m_Items.TryAdd(item.ID, item))
                return false;
            return true;
        }

        public bool RemoveItem(EntityItem item)
        {
            if (m_Items.ContainsKey(item.ID))
            {
                m_Items.Remove(item.ID);
                return true;
            }
            return false;
        }
        
        public bool Contains(string itemID)
        {
            return m_Items.ContainsKey(itemID);
        }
    }
}
