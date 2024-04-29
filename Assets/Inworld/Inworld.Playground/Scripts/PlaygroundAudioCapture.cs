/*************************************************************************************************
 * Copyright 2022-2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using Inworld.AEC;
using UnityEngine;

namespace Inworld.Playground
{
    /// <summary>
    ///     Base audio capture with support for updating the default sample mode.
    /// </summary>
    public class PlaygroundAudioCapture : AudioCapture
    {
        /// <summary>
        ///     Updates the default sample mode. 
        /// </summary>
        /// <param name="micSampleMode">The sample mode to set as default.</param>
        public void UpdateDefaultSampleMode(MicSampleMode micSampleMode)
        {
            if (micSampleMode == MicSampleMode.PUSH_TO_TALK) 
                m_PushToTalkKey = KeyCode.V;
            else
                m_PushToTalkKey = KeyCode.None;
            m_InitSampleMode = micSampleMode;
            m_SamplingMode = micSampleMode;
        }
    }
}
