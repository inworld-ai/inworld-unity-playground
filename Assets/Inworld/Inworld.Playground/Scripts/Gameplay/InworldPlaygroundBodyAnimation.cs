/*************************************************************************************************
 * Copyright 2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using Inworld.Assets;
using Inworld.Packet;
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

        protected override void OnCharacterStartSpeaking(string brainName)
        {
            if (m_NavMeshAgent && m_NavMeshAgent.velocity.magnitude == 0)
                base.OnCharacterStartSpeaking(brainName);
        }

        protected override void OnCharacterEndSpeaking(string brainName)
        {
            if (m_NavMeshAgent && m_NavMeshAgent.velocity.magnitude == 0)
                base.OnCharacterEndSpeaking(brainName);
        }

        void Update()
        {
            if (!m_NavMeshAgent) return;
            
            if (m_NavMeshAgent.velocity.magnitude > 0)
                m_BodyAnimator.SetInteger(s_Motion, MovementAnimMainStatus);
            else if (m_BodyAnimator.GetInteger(s_Motion) == MovementAnimMainStatus)
                HandleMainStatus(AnimMainStatus.Neutral);
        }
        
        protected override void HandleEmotion(EmotionPacket packet)
        {
            m_BodyAnimator.SetFloat(s_Random, Random.Range(0, 1) > 0.5f ? 1 : 0);
            m_BodyAnimator.SetFloat(s_RemainSec, m_Interaction.AnimFactor);
            _ProcessEmotion(packet.emotion.behavior.ToUpper());
        }

        void _ProcessEmotion(string emotionBehavior)
        {
            EmotionMapData emoMapData = m_EmotionMap[emotionBehavior];
            if (emoMapData == null)
            {
                InworldAI.LogError($"Unhandled emotion {emotionBehavior}");
                return;
            }
            m_BodyAnimator.SetInteger(s_Emotion, (int)emoMapData.bodyEmotion);
            m_BodyAnimator.SetTrigger("Gesture_" + (int)emoMapData.bodyGesture);
        }
    }
}

