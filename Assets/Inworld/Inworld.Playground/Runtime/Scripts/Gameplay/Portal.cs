/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Inworld.Playground
{
    /// <summary>
    ///     Handles portals for changing Playground scenes.
    /// </summary>
    public class Portal : MonoBehaviour
    {
        [SerializeField] TMP_Text m_PortalName;

        /// <summary>
        ///     Change scene to the one specified by m_SceneName on the component.
        /// </summary>
        void OnTriggerEnter(Collider other)
        {
            if (InworldController.Instance)
                InworldController.Client.StopAudioTo(true);
            SceneManager.LoadSceneAsync(m_PortalName.text);
        }
    }
}
