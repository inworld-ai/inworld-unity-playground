/*************************************************************************************************
 * Copyright 2022-2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;


namespace Inworld.Playground
{
    /// <summary>
    ///     Handles UI for a the Trust Control Panel.
    /// </summary>
    public class TrustControlUIPanel : MonoBehaviour
    {
        [SerializeField] InworldCharacter m_InworldCharacter;

        int m_CurrentTrustLevel = 1;

        public void OnSuspiciousToggleValueChanged(bool value)
        {
            if (!value || m_CurrentTrustLevel == 1) return;

            m_CurrentTrustLevel = 1;
            m_InworldCharacter.SendTrigger("trust_level1");
        }

        public void OnUncertainToggleValueChanged(bool value)
        {
            if (!value || m_CurrentTrustLevel == 2) return;

            m_CurrentTrustLevel = 2;
            m_InworldCharacter.SendTrigger("trust_level2");
        }
        
        public void OnTrustingToggleValueChanged(bool value)
        {
            if (!value || m_CurrentTrustLevel == 3) return;

            m_CurrentTrustLevel = 3;
            m_InworldCharacter.SendTrigger("trust_level3");
        }

        void OnEnable()
        {
            InworldController.Client.OnStatusChanged += ClientOnOnStatusChanged;
        }
        
        void OnDisable()
        {
            if(InworldController.Instance)
                InworldController.Client.OnStatusChanged -= ClientOnOnStatusChanged;
        }
        
        void ClientOnOnStatusChanged(InworldConnectionStatus status)
        {
            if (status == InworldConnectionStatus.Connected)
            {
                m_InworldCharacter.SendTrigger($"trust_level{m_CurrentTrustLevel}");
            }
        }
    }
}
