/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inworld.Playground
{
    /// <summary>
    ///     Handles the Settings UI.
    /// </summary>
    public class SettingsUI : MonoBehaviour
    {
        [SerializeField] 
        private Toggle m_MicModeInteractionToggle;
        [SerializeField] 
        private Toggle m_MicModePTTToggle;
        [SerializeField] 
        private Toggle m_MicModeTurnBasedToggle;
        [SerializeField] 
        private Toggle m_AECToggle;
        [SerializeField] 
        private Button m_PlayButton;
        [SerializeField]
        private Button m_ConnectButton;
        [SerializeField]
        private TMP_Text m_PlayerNameText;
        [SerializeField]
        private TMP_Text m_WorkspaceText;
        [SerializeField]
        private TMP_Text m_ConnectionStatusText;
        
        private PlaygroundManager m_PlaygroundManager;

        #region UI Callback Functions
        public void Play()
        {
            UpdatePlayButton(false);
            m_PlaygroundManager.Play();
        }
        
        public void SetMicrophoneModeInteractive(bool value)
        {
            if (!value) return;
            m_PlaygroundManager.UpdateMicrophoneMode(MicrophoneMode.Interactive);
        }
        
        public void SetMicrophoneModePushToTalk(bool value)
        {
            if (!value) return;
            m_PlaygroundManager.UpdateMicrophoneMode(MicrophoneMode.PushToTalk);
        }
        
        public void SetMicrophoneModeTurnByTurn(bool value)
        {
            if (!value) return;
            m_PlaygroundManager.UpdateMicrophoneMode(MicrophoneMode.TurnByTurn);
        }
        
        public void SetAECEnabled(bool value)
        {
            m_PlaygroundManager.UpdateEnableAEC(value);
        }

        public void Connect()
        {
            switch (InworldController.Status)
            {
                case InworldConnectionStatus.Error:
                case InworldConnectionStatus.Idle:
                    PlaygroundManager.Instance.LoadData();
                    InworldController.Instance.Init();
                    m_ConnectButton.interactable = false;
                    break;
                case InworldConnectionStatus.Connected:
                    InworldController.Instance.Disconnect();
                    break;
            }
        }
        #endregion

        #region Unity Event Functions
        private void Awake()
        {
            m_PlaygroundManager = PlaygroundManager.Instance;
        }

        private void OnEnable()
        {
            InworldController.Client.OnStatusChanged += OnStatusChanged;
            m_PlayerNameText.text = $"Player Name: {m_PlaygroundManager.GetPlayerName()}";
            m_WorkspaceText.text = $"Workspace: {m_PlaygroundManager.GetWorkspaceId()}";

            var interactionMode = m_PlaygroundManager.GetInteractionMode();
            switch (interactionMode)
            {
                case MicrophoneMode.Interactive:
                    m_MicModeInteractionToggle.isOn = true;
                    m_MicModePTTToggle.isOn = false;
                    m_MicModeTurnBasedToggle.isOn = false;
                    break;
                case MicrophoneMode.PushToTalk:
                    m_MicModePTTToggle.isOn = true;
                    m_MicModeInteractionToggle.isOn = false;
                    m_MicModeTurnBasedToggle.isOn = false;
                    break;
                case MicrophoneMode.TurnByTurn:
                    m_MicModeTurnBasedToggle.isOn = true;
                    m_MicModeInteractionToggle.isOn = false;
                    m_MicModePTTToggle.isOn = false;
                    break;
            }
            
            var aecEnabled = m_PlaygroundManager.GetEnableAEC();
            m_AECToggle.isOn = aecEnabled;
            
            if(PlaygroundManager.Instance.CurrentScene.name != PlaygroundManager.SetupSceneName)
                UpdatePlayButton(true);
            
            OnStatusChanged(InworldController.Status);
        }

        private void OnDisable()
        {
            UpdatePlayButton(false);
            if(InworldController.Instance)
                InworldController.Client.OnStatusChanged -= OnStatusChanged;
        }
        #endregion

        #region Event Callback Functions
        private void OnStatusChanged(InworldConnectionStatus status)
        {
            m_ConnectionStatusText.text = status.ToString();
            switch (status)
            {
                case InworldConnectionStatus.Error:
                case InworldConnectionStatus.Idle:
                    m_ConnectButton.GetComponentInChildren<TMP_Text>().text = "Connect";
                    m_ConnectButton.interactable = true;
                    break;
                case InworldConnectionStatus.Connected:
                    m_ConnectButton.GetComponentInChildren<TMP_Text>().text = "Disconnect";
                    m_ConnectButton.interactable = true;
                    break;
                default:
                    m_ConnectButton.interactable = false;
                    break;
            }
            if(PlaygroundManager.Instance.CurrentScene.name == PlaygroundManager.SetupSceneName)
                UpdatePlayButton(status == InworldConnectionStatus.Connected);
        }
        #endregion
        
        private void UpdatePlayButton(bool interactable)
        {
            string micDevice = m_PlaygroundManager.GetMicrophoneDevice();
            if (Microphone.devices.Length == 0 || 
                (!string.IsNullOrEmpty(micDevice) && !Microphone.devices.Contains(micDevice)))
            {
                m_PlayButton.interactable = false;
                return;
            }
            m_PlayButton.interactable = interactable;
        }
    }
}
