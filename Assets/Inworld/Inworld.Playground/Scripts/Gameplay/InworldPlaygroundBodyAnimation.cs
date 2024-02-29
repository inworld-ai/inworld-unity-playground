/*************************************************************************************************
 * Copyright 2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using Inworld.Assets;
using UnityEngine;
using UnityEngine.AI;

namespace Inworld.Playground
{
    /// <summary>
    ///     Body Animation handling for the Playground Inworld characters.
    ///     Adds movement animation to the original InworldBodyAnimation.
    /// </summary>
    public class InworldPlaygroundBodyAnimation : InworldBodyAnimation
    {
        protected const int MovementAnimMainStatus = 4;
        
        [SerializeField] NavMeshAgent m_NavMeshAgent;
        
        protected override void OnStartStopInteraction(bool isStarting)
        {
            if (m_NavMeshAgent.velocity.magnitude == 0)
                base.OnStartStopInteraction(isStarting);
        }

        void Update()
        {
            if (m_NavMeshAgent.velocity.magnitude > 0)
                m_BodyAnimator.SetInteger(s_Motion, MovementAnimMainStatus);
            else if (m_BodyAnimator.GetInteger(s_Motion) == MovementAnimMainStatus)
                HandleMainStatus(AnimMainStatus.Neutral);
        }
    }
}

