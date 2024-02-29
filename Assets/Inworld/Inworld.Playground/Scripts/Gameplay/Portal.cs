/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using UnityEngine;

namespace Inworld.Playground
{
    /// <summary>
    ///     Handles portals for changing Playground scenes.
    /// </summary>
    public class Portal : MonoBehaviour
    {
        [SerializeField] 
        private string m_SceneName;
        
        /// <summary>
        ///     Change scene to the one specified by m_SceneName on the component.
        /// </summary>
        public void Activate()
        {
            PlaygroundManager.Instance.ChangeScene(m_SceneName);
        }
    }
}
