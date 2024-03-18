/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inworld.Playground
{
    /// <summary>
    ///     Control UI Panel for setting an Inworld character's Gesture animations.
    /// </summary>
    public class GestureControlUIPanel : ControlUIPanel
    {
        [SerializeField] private float m_TransitionTime = 0.5f;
        [SerializeField] private List<Animator> m_Animators;

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
            foreach (var animator in m_Animators)
                animator.SetInteger(s_Gesture, m_SelectionIndex);
            m_PlayButton.interactable = false;
            // Wait for transition
            while (m_Animators[0].GetCurrentAnimatorClipInfo(1) == null ||
                   m_Animators[0].GetCurrentAnimatorClipInfo(1).Length == 0)
                yield return new WaitForEndOfFrame();
            
            // Wait for animation clip to finish
            yield return new WaitForSeconds(m_Animators[0].GetCurrentAnimatorClipInfo(1)[0].clip.length);

            foreach (var animator in m_Animators)
                animator.SetInteger(s_Gesture, 0);

            m_PlayButton.interactable = true;
        }
    }
}
