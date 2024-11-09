/*************************************************************************************************
 * Copyright 2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/
using System;
using System.Collections.Generic;
using Inworld.BehaviorEngine;
using UnityEngine;

namespace Inworld.Map
{
    public class Lightbulb : MonoBehaviour
    {
        [SerializeField] private Light m_LightSource;
        [SerializeField] private Material m_LightOnMaterial;
        [SerializeField] private Material m_LightOffMaterial;
        
        [SerializeField] private Task m_TurnOnTask;
        [SerializeField] private Task m_TurnOffTask;
        [SerializeField] private EntityItem m_ItemTaskPerformedOn;
        [SerializeField] private InworldCharacterTaskHandler m_CharacterTaskHandler;

        private MeshRenderer m_MeshRenderer;
        private EntityItem m_EntityItem;

        private void Awake()
        {
            m_MeshRenderer = GetComponent<MeshRenderer>();
            m_EntityItem = GetComponent<EntityItem>();
        }

        private void OnEnable()
        {
            m_CharacterTaskHandler.onTaskComplete.AddListener(OnTaskComplete);
        }
        
        private void OnDisable()
        {
            m_CharacterTaskHandler.onTaskComplete.RemoveListener(OnTaskComplete);
        }

        private void OnTaskComplete(string brainName, string taskName, Dictionary<string, string> parameters)
        {
            if (parameters.TryGetValue("what", out string itemID) && itemID == m_ItemTaskPerformedOn.ID)
            {
                if (taskName == m_TurnOnTask.TaskName)
                {
                    Material[] materials = m_MeshRenderer.materials;
                    materials[0] = m_LightOnMaterial;
                    m_MeshRenderer.materials = materials;
                    m_LightSource.enabled = true;
                    m_EntityItem.UpdateProperty("state", "On and producing light.");
                    InworldAI.Log("Lightbulb has been turned on");
                } else if (taskName == m_TurnOffTask.TaskName)
                {
                    Material[] materials = m_MeshRenderer.materials;
                    materials[0] = m_LightOffMaterial;
                    m_MeshRenderer.materials = materials;
                    m_LightSource.enabled = false;
                    m_EntityItem.UpdateProperty("state", "Off and not producing any light.");
                    InworldAI.Log("Lightbulb has been turned off");
                }
            }
        }
    }
}
