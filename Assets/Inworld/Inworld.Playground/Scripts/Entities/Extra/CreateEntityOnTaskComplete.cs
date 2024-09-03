/*************************************************************************************************
 * Copyright 2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections.Generic;
using Inworld.Packet;
using UnityEngine;
using UnityEngine.Serialization;

namespace Inworld.Map
{
    [RequireComponent(typeof(EntityItem))]
    public class CreateEntityOnTaskComplete : MonoBehaviour
    {
        [SerializeField] private Task m_Task;
        [SerializeField] private EntityItem m_ItemTaskPerformedOn;
        
        private EntityItem m_EntityItemToCreate;

        private void Awake()
        {
            m_EntityItemToCreate = GetComponent<EntityItem>();
        }

        private void OnEnable()
        {
            EntityManager.Instance.onTaskComplete.AddListener(OnTaskComplete);
        }

        private void OnDisable()
        {
            EntityManager.Instance.onTaskComplete.RemoveListener(OnTaskComplete);
        }

        private void OnTaskComplete(string brainName, string taskID, Dictionary<string, string> parameters)
        {
            if (taskID == m_Task.TaskName && parameters.TryGetValue("what", out string itemID) && itemID == m_ItemTaskPerformedOn.ID)
                EntityManager.Instance.AddItem(m_EntityItemToCreate);
        }
    }
}
