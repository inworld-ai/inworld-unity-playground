/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using UnityEngine;
using UnityEngine.UI;

namespace Inworld.Playground
{
    /// <summary>
    ///     Shows/Hides a recording icon to display when audio is being captured.
    /// </summary>
    public class RecordingUI : MonoBehaviour
    {
        [SerializeField] private Image m_RecordingIcon;

        private void Update()
        {
            m_RecordingIcon.enabled = InworldController.Audio.SampleMode == MicSampleMode.PUSH_TO_TALK ? 
                                          Input.GetKey(KeyCode.V) :
                                          InworldController.Audio && InworldController.Audio.IsCapturing;
        }
    }
}
