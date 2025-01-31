/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using UnityEngine;
using Inworld.Audio;
using TMPro;
using UnityEngine.UI;

namespace Inworld.Playground
{
    public class PlaygroundAudioSetting : MonoBehaviour
    {
        [Header("Microphone Settings:")]
        [SerializeField] TMP_Dropdown m_Dropdown;
        [SerializeField] Button m_MicButton;
        [SerializeField] Button m_CalibrationButton;
        [SerializeField] Sprite m_MicOn;
        [SerializeField] Sprite m_MicOff;

        bool m_IsMicOn;
        protected void OnEnable()
        {
            if (!InworldController.Audio)
                return;
            IMicrophoneHandler micHandler = InworldController.Audio.GetModule<IMicrophoneHandler>();
            if (micHandler == null)
                return;
            List<string> devices = micHandler.ListMicDevices();
            if (!m_Dropdown) 
                return;
            if (m_Dropdown.options == null)
                m_Dropdown.options = new List<TMP_Dropdown.OptionData>();
            m_Dropdown.captionText.text = "Select Input Device";
            m_Dropdown.options.Clear();
            foreach (string device in devices)
            {
                m_Dropdown.options.Add(new TMP_Dropdown.OptionData(device));
            }
        }
        public void UpdateAudioInput(int nIndex)
        {
            if (nIndex < 0)
                return;

            if (!InworldController.Audio)
                return;
            IMicrophoneHandler micHandler = InworldController.Audio.GetModule<IMicrophoneHandler>();
            if (micHandler != null)
                micHandler.ChangeInputDevice(Microphone.devices[nIndex]);
            if (m_MicButton)
            {
                m_MicButton.interactable = true;
                m_MicButton.image.sprite = m_IsMicOn ? m_MicOff : m_MicOn;
            }
            m_CalibrationButton.interactable = true;
        }
    }
}