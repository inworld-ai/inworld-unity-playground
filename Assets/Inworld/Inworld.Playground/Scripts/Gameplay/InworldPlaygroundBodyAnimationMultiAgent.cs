/*************************************************************************************************
 * Copyright 2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using UnityEngine;

namespace Inworld.Playground
{
    /// <summary>
    ///     Body Animation handling for the Playground Inworld characters of the Multi-Agent showcase.
    ///     Adds additional handling for character head movement to support looking at other characters.
    /// </summary>
    public class InworldPlaygroundBodyAnimationMultiAgent : InworldPlaygroundBodyAnimation
    {
        [SerializeField] private Transform m_StartLookTransform;

        private Transform m_CurrentLookTransform;
        private Vector3 m_CurrentLookPosition;

        protected void Start()
        {
            m_CurrentLookTransform = m_StartLookTransform;
            m_CurrentLookPosition = m_CurrentLookTransform.position;
        }

        protected new void OnAnimatorIK(int layerIndex)
        {
            if (!m_BodyAnimator)
                return;
            if (!MultiAgentShowcase.Instance.CurrentSpeakingTransform || MultiAgentShowcase.Instance.CurrentSpeakingTransform == m_HeadTransform)
            {
                if(m_CurrentLookTransform)
                    StartLookAt(m_CurrentLookTransform.position);
                else
                    StartLookAt(m_CurrentLookPosition);
                return;
            }

            m_CurrentLookTransform = MultiAgentShowcase.Instance.CurrentSpeakingTransform;
            StartLookAt(m_CurrentLookTransform.position);
        }

        protected override void StartLookAt(Vector3 lookPos)
        {
            m_CurrentLookPosition = Vector3.Lerp(m_CurrentLookPosition, lookPos, Time.deltaTime);
            
            base.StartLookAt(m_CurrentLookPosition);
        }
    }
}

