/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using Inworld.Sample;
using UnityEngine;

namespace Inworld.Playground
{
    // TODO(Yan): We don't need this.
    /// <summary>
    ///     Handles user input controls UI for the UI overlay.
    /// </summary>
    [Obsolete]
    public class ControlsUI : MonoBehaviour
    {
        [SerializeField] GameObject m_PushToTalkControl;

        void Awake()
        {
            m_PushToTalkControl.SetActive(false);
        }

        void OnEnable()
        {
            PlayerController.Instance.onCanvasClosed.AddListener(OnPlay);
        }

        void OnDisable()
        {
            if (PlayerController.Instance)
                PlayerController.Instance.onCanvasClosed.RemoveListener(OnPlay);
        }

        void OnPlay()
        {
            // m_PushToTalkControl.SetActive(PlaygroundManagerBak.InteractionMode == MicSampleMode.PUSH_TO_TALK);
        }
    }
}