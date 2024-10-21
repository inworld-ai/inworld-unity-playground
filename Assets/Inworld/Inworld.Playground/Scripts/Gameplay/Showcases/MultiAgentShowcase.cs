/*************************************************************************************************
 * Copyright 2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using Inworld.Entities;
using Inworld.Sample;
using UnityEngine;

namespace Inworld.Playground
{
    /// <summary>
    ///     Handler for the Multi-Agent showcase.
    /// </summary>
    public class MultiAgentShowcase : SingletonBehavior<MultiAgentShowcase>
    {
        public Transform CurrentSpeakingTransform
        {
            get
            {
                if (m_IsPlayerSpeaking)
                    return Camera.main.transform;
                else if (m_CurrentSpeakingCharacter)
                {
                    SightAngle sightAngle = m_CurrentSpeakingCharacter.GetComponent<SightAngle>();
                    if (sightAngle)
                        return sightAngle.HeadTransform;
                    InworldAI.LogException("Missing SightAngle component on Inworld Character.");
                }
                return null;
            }
        }

        [SerializeField] private float m_DefaultTimeBeforeNextTurn = 1;
        [SerializeField] private float m_TimeBeforeNextTurnAfterPlayer = 5;
        [SerializeField] private float m_NextTurnDelay = 10;
        [SerializeField] private List<InworldCharacter> m_CharactersInConversation;

        private InworldCharacter m_CurrentSpeakingCharacter;
        private bool m_IsPlayerSpeaking;

        private float m_TimeBeforeNextTurn;
        private float m_ConversationTimer;
        
        private void Awake()
        {
            PlaygroundManager.Instance.GameData.capabilities.multiAgent = true;
            InworldController.CharacterHandler.SelectingMethod = CharSelectingMethod.AutoChat;
        }

        private void OnDestroy()
        {
            if(InworldController.CharacterHandler)
                InworldController.CharacterHandler.SelectingMethod = CharSelectingMethod.SightAngle;
        }

        private void OnEnable()
        {
            InworldController.Audio.Event.onRecordingStart.AddListener(OnPlayerStartSpeaking);
            InworldController.Audio.Event.onRecordingEnd.AddListener(OnPlayerStopSpeaking);
            foreach (InworldCharacter inworldCharacter in m_CharactersInConversation)
            {
                inworldCharacter.Event.onBeginSpeaking.AddListener(OnCharacterBeginSpeaking);
                inworldCharacter.Event.onEndSpeaking.AddListener(OnCharacterEndSpeaking);
            }
        }

        private void OnDisable()
        {
            if (InworldController.Audio)
            {
                InworldController.Audio.Event.onRecordingStart.RemoveListener(OnPlayerStartSpeaking);
                InworldController.Audio.Event.onRecordingEnd.RemoveListener(OnPlayerStopSpeaking);
            }
            
            foreach (InworldCharacter inworldCharacter in m_CharactersInConversation)
            {
                if (inworldCharacter)
                {
                    inworldCharacter.Event.onBeginSpeaking.RemoveListener(OnCharacterBeginSpeaking);
                    inworldCharacter.Event.onEndSpeaking.RemoveListener(OnCharacterEndSpeaking);
                }
            }
        }

        private void Start()
        {
            StartCoroutine(ConversationHandler());
        }

        private IEnumerator ConversationHandler()
        {
            m_TimeBeforeNextTurn = m_DefaultTimeBeforeNextTurn;
            m_ConversationTimer = m_TimeBeforeNextTurn;
            
            while (true)
            {
                if (InworldController.Status != InworldConnectionStatus.Connected)
                    yield return PlaygroundManager.Instance.Connect();

                if (!InworldAI.Capabilities.multiAgent)
                {
                    InworldAI.Capabilities.multiAgent = true;
                    InworldController.Client.SendSessionConfig(false);
                }

                if (!m_CurrentSpeakingCharacter && !m_IsPlayerSpeaking)
                {
                    m_ConversationTimer += Time.deltaTime;
                    if (m_ConversationTimer >= m_TimeBeforeNextTurn)
                    {
                        m_ConversationTimer = 0;
                        InworldController.Instance.SendTrigger(InworldMessenger.NextTurn);
                        yield return new WaitForSeconds(m_NextTurnDelay);
                        Debug.Log("Send Next Turn");
                    }
                }
                else
                    m_ConversationTimer = 0;
                yield return null;
            }
        }

        private void OnPlayerStartSpeaking()
        {
            m_IsPlayerSpeaking = true;
            Debug.Log("Player Start Speaking");
        }
        
        private void OnPlayerStopSpeaking()
        {
            m_IsPlayerSpeaking = false;
            m_TimeBeforeNextTurn = m_TimeBeforeNextTurnAfterPlayer;
            Debug.Log("Player Stop Speaking");
        }
        
        private void OnCharacterBeginSpeaking(string brainName)
        {
            m_CurrentSpeakingCharacter = InworldController.CharacterHandler.GetCharacterByBrainName(brainName);
            Debug.Log($"Character {m_CurrentSpeakingCharacter.Name} Start Speaking");
        }

        private void OnCharacterEndSpeaking(string brainName)
        {
            if (Math.Abs(m_TimeBeforeNextTurn - m_TimeBeforeNextTurnAfterPlayer) < 0.1f)
                m_TimeBeforeNextTurn = m_TimeBeforeNextTurnAfterPlayer / 2f;
            else
                m_TimeBeforeNextTurn = m_DefaultTimeBeforeNextTurn;
            Debug.Log($"Character {m_CurrentSpeakingCharacter.Name} Stop Speaking");
            m_CurrentSpeakingCharacter = null;
        }

    }
}
