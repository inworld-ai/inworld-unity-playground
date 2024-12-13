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
    public class MicrophoneSelector : MonoBehaviour
    {
        protected void OnEnable()
        {
            if (InworldController.Audio is PlaygroundAECAudioCapture playgroundAudioCapture)
                playgroundAudioCapture.UpdateMicrophoneDevices();

        }
    }
}
