/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inworld.Playground
{
    public class PlaygroundSetting : MonoBehaviour
    {
        [Header("User Info:")]
        [SerializeField] TMP_Text m_UserName;
        [SerializeField] TMP_Text m_Workspace;
        [Header("Connection:")]
        [SerializeField] TMP_Text m_ConnectStatus;
        [SerializeField] TMP_Text m_TxtConnect;
        [SerializeField] Button m_BtnPlay;
        void OnEnable()
        {
            InworldController.Client.OnStatusChanged += OnStatusChanged;
            _UpdateUserInfo();
            if (!InworldController.Instance)
                return;
            OnStatusChanged(InworldController.Client.Status);
        }

        void OnDisable()
        {
            if (!InworldController.Instance)
                return;
            InworldController.Client.OnStatusChanged -= OnStatusChanged;
        }

        void _UpdateUserInfo()
        {
            if (m_UserName)
                m_UserName.text = $"Player Name: {InworldAI.User.Name}";
            if (m_Workspace)
                m_Workspace.text = $"Workspace: {InworldController.Instance.GameData.workspaceName}";
        }

        void OnStatusChanged(InworldConnectionStatus status)
        {
            if (m_ConnectStatus)
                m_ConnectStatus.text = status.ToString();
            if (!m_TxtConnect) 
                return;
            switch (status)
            {
                case InworldConnectionStatus.Connected:
                    m_TxtConnect.text = "Disconnect";
                    if (m_BtnPlay)
                        m_BtnPlay.interactable = true;
                    break;
                case InworldConnectionStatus.Error:
                case InworldConnectionStatus.Idle:
                    m_TxtConnect.text = "Connect";
                    if (m_BtnPlay)
                        m_BtnPlay.interactable = false;
                    break;
            }

        }
    }
}