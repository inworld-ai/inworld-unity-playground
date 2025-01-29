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
        [SerializeField] Toggle m_MicModeInteractionToggle;
        [SerializeField] Toggle m_MicModePTTToggle;
        [SerializeField] Toggle m_MicModeTurnBasedToggle;
        [SerializeField] Toggle m_AECToggle;
        [SerializeField] Button m_PlayButton;
        [SerializeField] Button m_ConnectButton;
        [SerializeField] TMP_Text m_PlayerNameText;
        [SerializeField] TMP_Text m_WorkspaceText;
        [SerializeField] TMP_Text m_ConnectionStatusText;

        #region Event Callback Functions

        void OnStatusChanged(InworldConnectionStatus status)
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

            //TODO(Yan): This is called If current scene is setup.
            UpdatePlayButton(status == InworldConnectionStatus.Connected);
        }

        #endregion

        //TODO(Yan): Mic Device
        void UpdatePlayButton(bool interactable)
        {
            string micDevice = "";//PlaygroundManagerBak.MicDevice;
#if !UNITY_WEBGL
            if (Microphone.devices.Length == 0 ||
                (!string.IsNullOrEmpty(micDevice) && !Microphone.devices.Contains(micDevice)))
            {
                m_PlayButton.interactable = false;
                return;
            }

            m_PlayButton.interactable = interactable;
#endif
        }

        #region UI Callback Functions

        public void Play()
        {
            UpdatePlayButton(false);
            // PlaygroundManagerBak.Instance.Play();
        }

        public void SetMicrophoneModeInteractive(bool value)
        {
            if (!value) 
                return;
            // PlaygroundManagerBak.InteractionMode = MicSampleMode.INTERACTIVE;
        }

        public void SetMicrophoneModePushToTalk(bool value)
        {
            if (!value) 
                return;
            // PlaygroundManagerBak.InteractionMode = MicSampleMode.PUSH_TO_TALK;
        }

        public void SetMicrophoneModeTurnByTurn(bool value) 
        {
            if (!value) 
                return;
            // PlaygroundManagerBak.InteractionMode = MicSampleMode.TURN_BASED;
        }

        public void SetAECEnabled(bool value)
        {
            // PlaygroundManagerBak.EnableAEC = value;
        }


        public void Connect()
        {
            switch (InworldController.Status)
            {
                case InworldConnectionStatus.Error:
                case InworldConnectionStatus.Idle:
                    // PlaygroundManagerBak.Instance.LoadData();
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

        void OnEnable()
        {
            InworldController.Client.OnStatusChanged += OnStatusChanged;
            // m_PlayerNameText.text = $"Player Name: {PlaygroundManagerBak.PlayerName}";
            // m_WorkspaceText.text = $"Workspace: {PlaygroundManagerBak.WorkspaceName}";
            //
            // switch (PlaygroundManagerBak.InteractionMode)
            // {
            //     case MicSampleMode.INTERACTIVE:
            //         m_MicModeInteractionToggle.isOn = true;
            //         m_MicModePTTToggle.isOn = false;
            //         m_MicModeTurnBasedToggle.isOn = false;
            //         break;
            //     case MicSampleMode.PUSH_TO_TALK:
            //         m_MicModePTTToggle.isOn = true;
            //         m_MicModeInteractionToggle.isOn = false;
            //         m_MicModeTurnBasedToggle.isOn = false;
            //         break;
            //     case MicSampleMode.TURN_BASED:
            //         m_MicModeTurnBasedToggle.isOn = true;
            //         m_MicModeInteractionToggle.isOn = false;
            //         m_MicModePTTToggle.isOn = false;
            //         break;
            // }
            // m_AECToggle.isOn = PlaygroundManagerBak.EnableAEC;
            UpdatePlayButton(true);
            OnStatusChanged(InworldController.Status);
        }

        void OnDisable()
        {
            UpdatePlayButton(false);
            if (InworldController.Instance)
                InworldController.Client.OnStatusChanged -= OnStatusChanged;
        }

        #endregion
    }
}