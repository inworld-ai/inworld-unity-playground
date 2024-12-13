/*************************************************************************************************
 * Copyright 2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections;
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
        [SerializeField] private Button m_Button;
        [SerializeField] private Toggle m_AudioToggle;
        [SerializeField] private Toggle m_EmotionToggle;
        [SerializeField] private Toggle m_RelationToggle;
        [SerializeField] private Toggle m_LipsyncToggle;

        private Capabilities m_Capabilities;
        private Coroutine m_CapabilityRequestCoroutine;

        private void Awake()
        {
            m_Capabilities = new Capabilities(InworldAI.Capabilities);
        }
        
        public void SendCapabilitiesRequest()
        {
            if (m_CapabilityRequestCoroutine != null) return;

            m_CapabilityRequestCoroutine = StartCoroutine(SendCapabilityRequestEnumerator());
        }

        IEnumerator SendCapabilityRequestEnumerator()
        {
            if(InworldController.Status != InworldConnectionStatus.Connected)
                yield return PlaygroundManager.Instance.Connect();
            
            m_Capabilities.audio = m_AudioToggle.isOn;
            m_Capabilities.emotions = m_EmotionToggle.isOn;
            m_Capabilities.relations = m_RelationToggle.isOn;
            m_Capabilities.phonemeInfo = m_LipsyncToggle.isOn;
            InworldAI.Capabilities = m_Capabilities;
            InworldController.Client.SendSessionConfig(false);

            m_CapabilityRequestCoroutine = null;
        }
    }
}
