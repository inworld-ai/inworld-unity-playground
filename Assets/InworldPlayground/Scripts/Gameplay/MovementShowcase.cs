/*************************************************************************************************
 * Copyright 2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using UnityEngine;
using UnityEngine.AI;

namespace Inworld.Playground
{
    /// <summary>
    ///     Generic handler for a showcase involving character movement.
    /// </summary>
    public class MovementShowcase : MonoBehaviour
    {
        [SerializeField]
        protected NavMeshAgent m_NavMeshAgent;
        [SerializeField]
        protected InworldCharacter m_InworldCharacter;
        
        Vector2 m_CurrentDestination;
        
        protected virtual void OnEnable()
        {
            m_InworldCharacter.onGoalCompleted.AddListener(OnInworldCharacterGoalCompleted);
        }

        protected virtual void OnDisable()
        {
            m_InworldCharacter.onGoalCompleted.RemoveListener(OnInworldCharacterGoalCompleted);
        }

        protected virtual void OnInworldCharacterGoalCompleted(string triggerName)
        {

        }

        protected void MoveTo(Vector3 position)
        {
            if (NavMesh.SamplePosition(position, out NavMeshHit hit, 10, NavMesh.AllAreas))
            {
                m_CurrentDestination = new Vector2(hit.position.x, hit.position.z);
                
                if (!IsAgentWithinStoppingDistance(m_CurrentDestination)) 
                    m_NavMeshAgent.SetDestination(hit.position);
            }
        }

        void Update()
        {
            if (!m_NavMeshAgent.hasPath) return;
            
            if (IsAgentWithinStoppingDistance(m_CurrentDestination)) 
                m_NavMeshAgent.ResetPath();
        }

        bool IsAgentWithinStoppingDistance(Vector3 position)
        {
            Vector2 agentPosition = new Vector2(m_NavMeshAgent.transform.position.x, m_NavMeshAgent.transform.position.z);
            return Vector2.Distance(agentPosition, position) < m_NavMeshAgent.stoppingDistance;
        }
    }

}
