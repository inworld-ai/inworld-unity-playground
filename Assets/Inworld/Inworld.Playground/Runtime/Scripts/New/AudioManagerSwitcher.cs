/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/


using Inworld.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace Inworld.Playground
{
    public class AudioManagerSwitcher : MonoBehaviour
    {
        [Header("Audio Manager Templates:")]
        [SerializeField] InworldAudioManager m_AECAudioManager;
        [SerializeField] InworldAudioManager m_TurnBasedAudioManager;
        [SerializeField] InworldAudioManager m_PTTAudioManager;
        [SerializeField] InworldAudioManager m_NoFilter;
        [SerializeField] InworldAudioManager m_NoVAD;
        [Header("Toggles:")]
        [SerializeField] Toggle m_TglInteractive;
        [SerializeField] Toggle m_TglPtt;
        [SerializeField] Toggle m_TglTurnBased;
        [SerializeField] Toggle m_TglAEC;
        [SerializeField] Toggle m_TglVAD;

        InworldAudioManager m_PrevAudioManager;
        public bool IsAvailable => Application.platform == RuntimePlatform.WindowsPlayer 
                                   || Application.platform == RuntimePlatform.WindowsEditor
                                   || Application.platform == RuntimePlatform.OSXEditor
                                   || Application.platform == RuntimePlatform.OSXPlayer;

        void Awake()
        {
            m_AECAudioManager = Instantiate(m_AECAudioManager);
            m_AECAudioManager.gameObject.SetActive(false);
            m_TurnBasedAudioManager = Instantiate(m_TurnBasedAudioManager);
            m_TurnBasedAudioManager.gameObject.SetActive(false);
            m_PTTAudioManager = Instantiate(m_PTTAudioManager);
            m_PTTAudioManager.gameObject.SetActive(false);
            m_NoFilter = Instantiate(m_NoFilter);
            m_NoFilter.gameObject.SetActive(false);
            m_NoVAD = Instantiate(m_NoVAD);
            m_NoVAD.gameObject.SetActive(false);
        }

        void OnEnable()
        {
            if (IsAvailable) 
                return;
            m_TglInteractive.isOn = false;
            m_TglAEC.isOn = false;
            m_TglVAD.isOn = false;
            m_TglInteractive.interactable = false;
            m_TglAEC.interactable = false;
            m_TglVAD.interactable = false;
        }

        public void OnSampleModeChanged()
        {
            InworldAudioManager result = GetSampleMode();
            if (result == m_PrevAudioManager)
                return;
            ReplaceAudioManager(result);
            m_PrevAudioManager = result;
        }

        void ReplaceAudioManager(InworldAudioManager aecAudioManager)
        {
            if (!InworldController.Instance)
                return;
            if (InworldController.Audio)
            {
                InworldController.Audio.transform.SetParent(transform);
                InworldController.Audio.gameObject.SetActive(false);
            }
            aecAudioManager.transform.SetParent(InworldController.Instance.transform);
            aecAudioManager.transform.localPosition = Vector3.zero;
            aecAudioManager.gameObject.SetActive(true);
        }

        InworldAudioManager GetSampleMode()
        {
            if (m_TglInteractive.isOn)
            {
                if (m_TglAEC.isOn)
                {
                    return m_TglVAD.isOn ? m_AECAudioManager : m_NoVAD;
                }
                return m_NoFilter;
            }
            if (m_TglPtt.isOn)
                return m_PTTAudioManager;
            return m_TurnBasedAudioManager;
        }
    }
}