/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using UnityEngine;

namespace Inworld.Playground
{
    /// <summary>
    ///     Handles user input controls UI for the UI overlay.
    /// </summary>
    public class ControlsUI : MonoBehaviour
    {
        [SerializeField] private GameObject m_PushToTalkControl;

        private void Awake()
        {
            m_PushToTalkControl.SetActive(false);
        }

        private void OnEnable()
        {
            PlaygroundManager.Instance.OnPlay += OnPlay;
        }

        private void OnDisable()
        {
            if(PlaygroundManager.Instance)
                PlaygroundManager.Instance.OnPlay -= OnPlay;
        }

        private void OnPlay()
        {
            m_PushToTalkControl.SetActive(PlaygroundManager.Instance.GetInteractionMode() == MicrophoneMode.PushToTalk);
        }
    }
}
