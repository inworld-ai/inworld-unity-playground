/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections.Generic;
using Inworld.Playground.Data;
using UnityEngine;

namespace Inworld.Playground
{
    /// <summary>
    ///     
    /// </summary>
    public class ChangeSceneShowcase : MonoBehaviour
    {
        [SerializeField] private bool m_StartAsDay = true;
        [SerializeField] private string m_DaySceneName = "day";
        [SerializeField] private string m_NightSceneName = "night";
        [Header("Objects")]
        [SerializeField] private Light m_LightBulbSource;
        [SerializeField] private MeshRenderer m_LightBulbMeshRenderer;
        [SerializeField] private Light m_DayLightSource;
        [SerializeField] private Light m_NightLightSource;
        [SerializeField] private Material m_DayMaterial;
        [SerializeField] private Material m_NightMaterial;

        private bool m_IsDay;

        private void Awake()
        {
            SetTimeOfDay(m_StartAsDay);
        }

        void SetTimeOfDay(bool isDay)
        {
            m_IsDay = isDay;
            m_LightBulbSource.enabled = m_IsDay;
            m_DayLightSource.enabled = m_IsDay;
            m_NightLightSource.enabled = !m_IsDay;
            RenderSettings.skybox = m_IsDay ? m_DayMaterial : m_NightMaterial;
            if(m_IsDay)
                m_LightBulbMeshRenderer.materials[0].EnableKeyword("_EMISSION");
            else
                m_LightBulbMeshRenderer.materials[0].DisableKeyword("_EMISSION");
        }

        public void SwitchScene()
        {
            SetTimeOfDay(!m_IsDay);
            PlaygroundManager.Instance.ChangeInworldScene(m_IsDay ? m_DaySceneName : m_NightSceneName);
        }
    }
}
