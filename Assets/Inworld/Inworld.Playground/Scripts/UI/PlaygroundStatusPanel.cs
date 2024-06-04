/*************************************************************************************************
 * Copyright 2022-2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *
 * Derivative of Inworld.Sample.StatusPanel
 *************************************************************************************************/

using System;
using Inworld.Packet;
using TMPro;
using UnityEngine;

namespace Inworld.Playground
{
    /// <summary>
    ///     Handles the status panel for connection and scene changes.
    ///     TODO: Inherit from Inworld.Sample.StatusPanel
    /// </summary>
    public class PlaygroundStatusPanel : MonoBehaviour
    {
        [SerializeField] GameObject m_Board;
        [SerializeField] TMP_Text m_Indicator;
        [SerializeField] TMP_Text m_Error;
        [SerializeField] GameObject m_NoMic;

        private void Awake()
        {
            if (m_Board)
                m_Board.SetActive(true);
            if (m_Indicator)
                m_Indicator.text = "Loading";
        }

        protected virtual void OnEnable()
        {
            InworldController.Audio.OnStartCalibrating.AddListener(() => SwitchMic(true));
            InworldController.Audio.OnStopCalibrating.AddListener(() => SwitchMic(false));
            InworldController.Client.OnErrorReceived += OnErrorReceived;
            InworldController.Client.OnStatusChanged += OnStatusChanged;
            PlaygroundManager.Instance.OnPlay.AddListener(OnEndSceneChange);
            PlaygroundManager.Instance.OnStartInworldSceneChange.AddListener(OnStartInworldSceneChange);
            PlaygroundManager.Instance.OnEndInworldSceneChange.AddListener(OnEndInworldSceneChange);
        }

        protected virtual void OnDisable()
        {
            if (!InworldController.Instance || !PlaygroundManager.Instance)
                return;
            InworldController.Client.OnErrorReceived -= OnErrorReceived;
            InworldController.Client.OnStatusChanged -= OnStatusChanged;
            PlaygroundManager.Instance.OnPlay.RemoveListener(OnEndSceneChange);
            PlaygroundManager.Instance.OnStartInworldSceneChange.RemoveListener(OnStartInworldSceneChange);
            PlaygroundManager.Instance.OnEndInworldSceneChange.RemoveListener(OnEndInworldSceneChange);
        }
        
        void OnErrorReceived(InworldError error)
        {
            m_Board.SetActive(true);
            m_Error.gameObject.SetActive(true);
            m_Error.text = error.message;
        }
        void SwitchMic(bool isOn)
        {
            if (m_NoMic)
                m_NoMic.SetActive(isOn);
        }
        void OnStatusChanged(InworldConnectionStatus incomingStatus)
        {
            bool hidePanel = incomingStatus == InworldConnectionStatus.Idle && !InworldController.HasError || incomingStatus == InworldConnectionStatus.Connected;
            if (m_Board)
                m_Board.SetActive(!hidePanel);
            if (m_Indicator)
                m_Indicator.text = incomingStatus.ToString();
            if (m_Error && incomingStatus == InworldConnectionStatus.Error)
                m_Error.text = InworldController.Client.ErrorMessage;
        }
        
        void OnEndSceneChange()
        {
            if (m_Board)
                m_Board.SetActive(false);
        }
        
        void OnStartInworldSceneChange()
        {
            if (m_Board)
                m_Board.SetActive(true);
            if (m_Indicator)
                m_Indicator.text = "Changing Inworld Scene";
        }
        
        void OnEndInworldSceneChange()
        {
            if (m_Board)
                m_Board.SetActive(false);
        }
    }
}
