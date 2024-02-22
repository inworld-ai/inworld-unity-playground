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
    ///     An interactable item that will play an animation on interaction.
    /// </summary>
    public class AnimationInteractable : Interactable
    {
        [Header("Animation")]
        [SerializeField] private Animator m_Animator;
        [SerializeField] private string m_TriggerName;

        /// <summary>
        ///     Set the 'm_TriggerName' trigger of the referenced Animator.
        /// </summary>
        protected override void _Interact()
        {
            m_Animator.SetTrigger(m_TriggerName);
        }
    }
}
