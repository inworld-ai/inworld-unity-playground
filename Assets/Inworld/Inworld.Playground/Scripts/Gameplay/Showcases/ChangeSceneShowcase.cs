/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using Inworld.Playground.Data;
using UnityEngine;

namespace Inworld.Playground
{
    /// <summary>
    ///     Handles the Change Scene showcase which shows how you can update the current Inworld scene dynamically
    ///         and how different scenes can effect the characters.
    /// </summary>
    public class ChangeSceneShowcase : MonoBehaviour
    {
        [SerializeField] private bool m_StartAsDay = true;
        [SerializeField] private string m_DaySceneName = "day";
        [SerializeField] private string m_NightSceneName = "night";
        [Header("Objects")]
        [SerializeField] private List<Interactable> m_Interactables;
        [SerializeField] private List<Light> m_LightBulbSources;
        [SerializeField] private List<MeshRenderer> m_LightBulbMeshRenderers;
        [SerializeField] private Light m_DayLightSource;
        [SerializeField] private Material m_DayMaterial;
        [SerializeField] private Material m_NightMaterial;
        [SerializeField] private Material m_BulbOnMaterial;
        [SerializeField] private Material m_BulbOffMaterial;

        private bool m_IsDay;

        private void Awake()
        {
            SetTimeOfDay(m_StartAsDay);
            foreach (Interactable interactable in m_Interactables)
                interactable.IsActive = false;
        }
        
        private void OnEnable()
        {
            InworldController.Client.OnStatusChanged += OnStatusChanged;
        }

        private void OnDisable()
        {
            if(InworldController.Client)
                InworldController.Client.OnStatusChanged -= OnStatusChanged;
        }
        
        private void OnStatusChanged(InworldConnectionStatus status)
        {
            foreach (Interactable interactable in m_Interactables)
                interactable.IsActive = status == InworldConnectionStatus.Connected;
        }

        private void SetTimeOfDay(bool isDay)
        {
            m_IsDay = isDay;
            foreach (Light lightBulbSource in m_LightBulbSources)
                lightBulbSource.enabled = m_IsDay;
            
            m_DayLightSource.enabled = m_IsDay;
            RenderSettings.skybox = m_IsDay ? m_DayMaterial : m_NightMaterial;

            foreach (MeshRenderer meshRenderer in m_LightBulbMeshRenderers)
            {
                var materials = meshRenderer.materials;
                if(m_IsDay)
                    materials[0] = m_BulbOnMaterial;
                else
                    materials[0] = m_BulbOffMaterial;
                meshRenderer.materials = materials;
            }
        }
        
        /// <summary>
        ///     Switch the Inworld scene from Day to Night or vice versa.
        /// </summary>
        public void SwitchScene()
        {
            SetTimeOfDay(!m_IsDay);
            PlaygroundManager.Instance.ChangeInworldScene(m_IsDay ? m_DaySceneName : m_NightSceneName);
        }
    }
}
