/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace Inworld.Playground
{
    /// <summary>
    ///     Handles the Change Scene showcase which shows how you can update the current Inworld scene dynamically
    ///     and how different scenes can effect the characters.
    /// </summary>
    public class ChangeSceneShowcase : MonoBehaviour
    {
        [SerializeField] bool m_StartAsDay = true;
        [SerializeField] string m_DaySceneName = "day";
        [SerializeField] string m_NightSceneName = "night";

        [Header("Objects")] [SerializeField] List<Light> m_LightBulbSources;

        [SerializeField] List<MeshRenderer> m_LightBulbMeshRenderers;
        [SerializeField] Light m_DayLightSource;
        [SerializeField] Material m_DayMaterial;
        [SerializeField] Material m_NightMaterial;
        [SerializeField] Material m_BulbOnMaterial;
        [SerializeField] Material m_BulbOffMaterial;

        bool m_IsDay;

        void Awake()
        {
            SetTimeOfDay(m_StartAsDay);
        }

        void SetTimeOfDay(bool isDay)
        {
            m_IsDay = isDay;
            foreach (Light lightBulbSource in m_LightBulbSources)
                lightBulbSource.enabled = m_IsDay;

            m_DayLightSource.enabled = m_IsDay;
            RenderSettings.skybox = m_IsDay ? m_DayMaterial : m_NightMaterial;

            foreach (MeshRenderer meshRenderer in m_LightBulbMeshRenderers)
            {
                Material[] materials = meshRenderer.materials;
                if (m_IsDay)
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
            string sceneName = m_IsDay ? m_DaySceneName : m_NightSceneName;
            string sceneFullName = InworldAI.GetSceneFullName(InworldController.Instance.GameData.workspaceName, sceneName);
            InworldController.Instance.LoadScene(sceneFullName);
        }
    }
}