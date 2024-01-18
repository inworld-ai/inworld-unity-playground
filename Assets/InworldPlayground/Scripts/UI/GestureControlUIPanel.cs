/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections;
using UnityEngine;

namespace Inworld.Playground
{
    /// <summary>
    ///     Control UI Panel for setting an Inworld character's Gesture animations.
    /// </summary>
    public class GestureControlUIPanel : ControlUIPanel
    {
        [SerializeField] private float m_TransitionTime = 0.5f;
        [SerializeField] private Animator m_Animator;

        static readonly int s_Gesture = Animator.StringToHash("Gesture");

        private void Awake()
        {
            m_SelectionIndex = 1;
        }

        protected override void OnDropdownValueChanged(int value)
        {
            m_SelectionIndex = value + 1;
        }
        
        protected override void OnPlayButtonClicked()
        {
            StartCoroutine(IPlayGesture());
        }

        private IEnumerator IPlayGesture()
        {
            m_Animator.SetInteger(s_Gesture, m_SelectionIndex);
            m_PlayButton.interactable = false;
            // Wait for transition
            yield return new WaitForSeconds(m_TransitionTime);
            // Wait for animation clip to finish
            yield return new WaitForSeconds(m_Animator.GetCurrentAnimatorClipInfo(1)[0].clip.length);
            
            m_Animator.SetInteger(s_Gesture, 0);
            m_PlayButton.interactable = true;
        }
    }
}
