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
    ///     AEC enabled audio capture with support for updating the default sample mode.
    /// </summary>
    public class PlaygroundAECAudioCapture : InworldAECAudioCapture
    {
        /// <summary>
        /// A flag for this component is using AEC
        /// </summary>
        public override bool EnableAEC => PlaygroundManager.Instance.GetEnableAEC();
        
        /// <summary>
        ///     Updates the default sample mode. 
        /// </summary>
        /// <param name="micSampleMode">The sample mode to set as default.</param>
        public void UpdateDefaultSampleMode(MicSampleMode micSampleMode)
        {
            m_SamplingMode = micSampleMode;
        }
    }
}
