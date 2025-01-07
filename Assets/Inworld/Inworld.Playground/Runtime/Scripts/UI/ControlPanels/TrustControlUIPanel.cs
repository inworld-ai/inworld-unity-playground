/*************************************************************************************************
 * Copyright 2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        void _OnTrustLevelChanged(int newTrustLevel)
        {
            int nDiff = Mathf.Abs(newTrustLevel - m_CurrentTrustLevel);
            switch (nDiff)
            {
                case 0:
                    return;
                case 1:
                    m_CurrentTrustLevel = newTrustLevel;
                    m_InworldCharacter.SendTrigger($"trust_level{m_CurrentTrustLevel}");
                    break;
                case 2:
                    m_CurrentTrustLevel = newTrustLevel;
                    StartCoroutine(_ResetSession());
                    break;
            }
        }

        IEnumerator _ResetSession()
        {
            yield return InworldController.Client.DisconnectAsync();
            InworldController.Instance.GetAccessToken();
        }

        public void OnSuspiciousToggleValueChanged(bool value)
        {
            if (value)
                _OnTrustLevelChanged(1);
        }

        public void OnUncertainToggleValueChanged(bool value)
        {
            if (value)
                _OnTrustLevelChanged(2);
        }
        
        public void OnTrustingToggleValueChanged(bool value)
        {
            if (value)
                _OnTrustLevelChanged(3);
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
