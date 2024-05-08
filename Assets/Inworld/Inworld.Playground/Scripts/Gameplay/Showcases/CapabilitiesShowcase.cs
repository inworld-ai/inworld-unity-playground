/*************************************************************************************************
 * Copyright 2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using Inworld.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Inworld.Playground
{
    /// <summary>
    ///     Handler for the Capabilities showcase.
    /// </summary>
    public class CapabilitiesShowcase : MonoBehaviour
    {
        [SerializeField] private Toggle m_AudioToggle;
        [SerializeField] private Toggle m_EmotionToggle;
        [SerializeField] private Toggle m_RelationToggle;
        [SerializeField] private Toggle m_NarratedActionToggle;
        [SerializeField] private Toggle m_LipsyncToggle;

        private Capabilities m_Capabilities;

        private void Awake()
        {
            m_Capabilities = new Capabilities(InworldAI.Capabilities);
        }

        public void SendCapabilitiesRequest()
        {
            m_Capabilities.audio = m_AudioToggle.isOn;
            m_Capabilities.emotions = m_EmotionToggle.isOn;
            m_Capabilities.relations = m_RelationToggle.isOn;
            m_Capabilities.narratedActions = m_NarratedActionToggle.isOn;
            m_Capabilities.phonemeInfo = m_LipsyncToggle.isOn;
            InworldAI.Capabilities = m_Capabilities;
            InworldController.Client.SendCapabilities();
        }
    }
}
