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
        [SerializeField] Button m_Button;
        [SerializeField] Toggle m_AudioToggle;
        [SerializeField] Toggle m_EmotionToggle;
        [SerializeField] Toggle m_RelationToggle;
        [SerializeField] Toggle m_LipsyncToggle;

        Capabilities m_Capabilities;

        void Awake()
        {
            m_Capabilities = new Capabilities(InworldAI.Capabilities);
        }

        public void SendCapabilitiesRequest()
        {
            if (InworldController.Status != InworldConnectionStatus.Connected)
                return;

            m_Capabilities.audio = m_AudioToggle.isOn;
            m_Capabilities.emotions = m_EmotionToggle.isOn;
            m_Capabilities.relations = m_RelationToggle.isOn;
            m_Capabilities.phonemeInfo = m_LipsyncToggle.isOn;
            InworldAI.Capabilities = m_Capabilities;
            InworldController.Client.SendSessionConfig(false);
        }
    }
}