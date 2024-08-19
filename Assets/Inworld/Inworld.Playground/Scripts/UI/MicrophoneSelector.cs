/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inworld.Playground
{
    /// <summary>
    ///     Handles Microphone selection UI.
    ///     Derivative of AudioCaptureTest from Inworld.Sample.
    /// </summary>
    public class MicrophoneSelector : AudioCapture
    {
        [SerializeField] TMP_Dropdown m_Dropdown;
        [SerializeField] TMP_Text m_Text;
        [SerializeField] Image m_Volume;
        [SerializeField] Button m_MicButton;
        [SerializeField] Button m_CalibrationButton;
        [SerializeField] Sprite m_MicOn;
        [SerializeField] Sprite m_MicOff;

        private int m_MicIndex;
  
        /// <summary>
        ///     Change the current input device from the selection of drop down field.
        /// </summary>
        /// <param name="nIndex">The index of the audio input devices.</param>
        public void UpdateAudioInput(int nIndex)
        {
    #if !UNITY_WEBGL
            if (nIndex < 0)
                return;

            m_MicIndex = nIndex;
            ChangeInputDevice(Microphone.devices[m_MicIndex]);
            PlaygroundManager.Instance.SetMicrophoneDevice(Microphone.devices[m_MicIndex]);
            m_MicButton.interactable = true;
            m_CalibrationButton.interactable = true;
            m_MicButton.image.sprite = m_MicOn;
            IsRecording = m_MicButton.image.sprite == m_MicOn;
#endif
        }
        
        /// <summary>
        ///     Mute/Unmute the microphone.
        /// </summary>
        public void UpdateMicrophoneMute()
        {
            if (!m_MicButton.interactable)
                return;
            if (m_MicButton.image.sprite == m_MicOn)
            {
                IsRecording = false;
                m_MicButton.image.sprite = m_MicOff;
                m_Volume.fillAmount = 0;
            }
            else
            {
                IsRecording = true;
                m_MicButton.image.sprite = m_MicOn;
            }
        }
        
        protected override void Awake()
        {
            base.Awake();
#if !UNITY_WEBGL
            string[] devices = Microphone.devices;
            if (m_Dropdown.options == null)
                m_Dropdown.options = new List<TMP_Dropdown.OptionData>();
            m_Dropdown.captionText.text = "Select Input Device";
            m_Dropdown.options.Clear();
            foreach (string device in devices)
            {
                m_Dropdown.options.Add(new TMP_Dropdown.OptionData(device));
            }
#endif
        }

        protected override void OnEnable()
        {
            string currentMicDevice = PlaygroundManager.Instance.GetMicrophoneDevice();
            for(var i = 0; i < m_Dropdown.options.Count; i++)
            {
                var option = m_Dropdown.options[i];
                if (option.text == currentMicDevice)
                    m_Dropdown.value = i;
            }
            
            if (Microphone.devices.Length > m_MicIndex)
                UpdateAudioInput(m_MicIndex);
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            IsRecording = false;
            base.OnDisable();
        }
        
        protected override void Collect()
        {
            if (!IsRecording) return;
            
            GetAudioData();
            m_Volume.fillAmount = IsRecording ? CalculateSNR() * 0.05f : 0f;
        }
        
    }
}
