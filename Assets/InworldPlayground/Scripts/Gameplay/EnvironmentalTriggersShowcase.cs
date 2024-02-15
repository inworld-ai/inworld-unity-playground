/*************************************************************************************************
 * Copyright 2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Inworld.Playground
{
    /// <summary>
    ///     Handler for the Environmental Triggers showcase.
    /// </summary>
    public class EnvironmentalTriggersShowcase : MonoBehaviour
    {
        [SerializeField]
        NavMeshAgent m_NavMeshAgent;
        [SerializeField]
        InworldCharacter m_InworldCharacter;
        [SerializeField]
        Transform m_AreaATransform;
        [SerializeField]
        Transform m_AreaBTransform;

        string m_LastDestination;
        Vector2 m_CurrentDestination;
        
        void OnEnable()
        {
            m_InworldCharacter.onGoalCompleted.AddListener(OnInworldCharacterGoalCompleted);
        }

        void OnDisable()
        {
            m_InworldCharacter.onGoalCompleted.RemoveListener(OnInworldCharacterGoalCompleted);
        }

        void OnInworldCharacterGoalCompleted(string triggerName)
        {
            switch (triggerName)
            {
                case "motion_area_a":
                case "request_area_a":
                    if (m_LastDestination == "A") break;
                    MoveTo(m_AreaATransform.position);
                    m_LastDestination = "A";
                    break;
                case "motion_area_b":
                case "request_area_b":
                    if (m_LastDestination == "B") break;
                    MoveTo(m_AreaBTransform.position);
                    m_LastDestination = "B";
                    break;
            }
        }

        void MoveTo(Vector3 position)
        {
            if (NavMesh.SamplePosition(position, out NavMeshHit hit, 10, NavMesh.AllAreas))
            {
                m_NavMeshAgent.SetDestination(hit.position);
                m_CurrentDestination = new Vector2(hit.position.x, hit.position.z);
            }
        }

        void Update()
        {
            if (!m_NavMeshAgent.hasPath) return;
            
            Vector2 agentPosition = new Vector2(m_NavMeshAgent.transform.position.x, m_NavMeshAgent.transform.position.z);
            if (Vector2.Distance(agentPosition, m_CurrentDestination) < m_NavMeshAgent.stoppingDistance)
                m_NavMeshAgent.ResetPath();
        }
    }

}
