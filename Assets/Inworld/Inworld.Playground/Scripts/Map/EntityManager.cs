/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inworld.Playground
{
    public class EntityManager : MonoBehaviour
    {
        private List<EntityItem> items;
        private void Awake()
        {
            items = new List<EntityItem>(FindObjectsOfType<EntityItem>());
        }

        private void Start()
        {
            foreach (EntityItem item in items)
                InworldController.Client.CreateItems(new List<Packet.EntityItem>() { item.Get() }, item.GetEntities());
        }
    }
}
