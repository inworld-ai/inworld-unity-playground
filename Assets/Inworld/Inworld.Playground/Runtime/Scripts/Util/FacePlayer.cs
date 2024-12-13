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
    ///     Rotates the object to face the PlayerController (if one exists in the scene).
    /// </summary>
    public class FacePlayer : MonoBehaviour
    {
        [SerializeField] private bool m_Reverse;
        private void Update()
        {
            if(PlayerControllerPlayground.Instance)
                transform.forward = (m_Reverse ? -1 : 1) * 
                                    (PlayerControllerPlayground.Instance.transform.position - transform.position);
        }
    }
}
