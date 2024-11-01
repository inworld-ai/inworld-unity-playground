/*************************************************************************************************
 * Copyright 2022-2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using Inworld.AEC;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inworld.Playground
{
    /// <summary>
    ///     AEC enabled audio capture with support for updating the default sample mode.
    /// </summary>
    public class PlaygroundAECAudioCapture : InworldAECAudioCapture
    {
        [Space(10)][Header("Microphone Settings:")]
        [SerializeField] TMP_Dropdown m_Dropdown;
        [SerializeField] TMP_Text m_Text;
        [SerializeField] Image m_Volume;
        [SerializeField] Button m_MicButton;
        [SerializeField] Button m_CalibrationButton;
        [SerializeField] Sprite m_MicOn;
        [SerializeField] Sprite m_MicOff;
        
        private int m_MicIndex;
        MicSampleMode m_PrevSampleModePlayground;
  
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
            m_MicButton.image.sprite = SampleMode == MicSampleMode.NO_MIC ? m_MicOff : m_MicOn;
#endif
        }
        
        /// <summary>
        ///     Mute/Unmute the microphone.
        /// </summary>
        public void UpdateMicrophoneMute()
        {
            if (!m_MicButton || !m_MicButton.interactable)
                return;
            if (m_MicButton.image.sprite == m_MicOn)
            {
                m_PrevSampleModePlayground = SampleMode;
                SampleMode = MicSampleMode.NO_MIC;
                m_MicButton.image.sprite = m_MicOff;
                m_Volume.fillAmount = 0;
            }
            else
            {
                SampleMode = m_PrevSampleModePlayground;
                m_MicButton.image.sprite = m_MicOn;
            }
        }
        /// <summary>
        /// A flag for this component is using AEC
        /// </summary>
        public override bool EnableAEC => PlaygroundManager.Instance.GetEnableAEC();

        protected override void Init()
        {
#if !UNITY_WEBGL
            string[] devices = Microphone.devices;
            if (m_Dropdown)
            {
                if (m_Dropdown.options == null)
                    m_Dropdown.options = new List<TMP_Dropdown.OptionData>();
                m_Dropdown.captionText.text = "Select Input Device";
                m_Dropdown.options.Clear();
                foreach (string device in devices)
                {
                    m_Dropdown.options.Add(new TMP_Dropdown.OptionData(device));
                }
            }
#endif
            base.Init();
        }
        protected override void OnPlayerCanvasOpen()
        {
            if (SampleMode == MicSampleMode.NO_MIC)
                return;
            base.OnPlayerCanvasOpen();
        }
        protected override void OnPlayerCanvasClosed()
        {
            if (SampleMode == MicSampleMode.NO_MIC)
                return;
            base.OnPlayerCanvasClosed();
        }
        public override void Recalibrate()
        {
            if (EnableVAD)
                m_BackgroundNoise = 0.001f; // Skip calibrating process.
        }

        protected override bool Collect()
        {
            if (m_Volume)
                m_Volume.fillAmount = SampleMode == MicSampleMode.NO_MIC ? 0f : CalculateSNR() * 0.05f;
            return base.Collect();
        }

        public void UpdateMicrophoneDevices()
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
        }
    }
}
