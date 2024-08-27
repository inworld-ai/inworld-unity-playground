/*************************************************************************************************
 * Copyright 2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using Inworld.Packet;
using UnityEngine;
using UnityEngine.AI;

namespace Inworld.Map
{
    [CreateAssetMenu(fileName = "GoTask", menuName = "Inworld/Tasks/GoTask")]
    public class GoTask : Task
    {
        [SerializeField] protected float m_MaxHitDistance = 5f;
        [SerializeField] protected float m_DestinationReachedThreshold = 0.1f;
        public override IEnumerator Perform(InworldCharacter inworldCharacter, List<TriggerParameter> parameters)
        {
            Dictionary<string, string> parameterDictionary = ParseParameters(parameters);
            if (parameterDictionary.TryGetValue("where", out string itemID) && EntityManager.Instance.FindItem(itemID, out EntityItem entityItem))
            {
                NavMeshAgent navMeshAgent = inworldCharacter.GetComponent<NavMeshAgent>();
                if (navMeshAgent && NavMesh.SamplePosition(entityItem.transform.position, out NavMeshHit navMeshHit, m_MaxHitDistance,NavMesh.AllAreas) &&
                    navMeshAgent.SetDestination(navMeshHit.position))
                {
                    while (navMeshAgent.pathPending || (navMeshAgent.hasPath && navMeshAgent.remainingDistance > m_DestinationReachedThreshold))
                        yield return null;
                    
                    if(navMeshAgent.remainingDistance <= m_DestinationReachedThreshold)
                        EntityManager.Instance.CompleteTask(this, inworldCharacter);
                    else 
                        EntityManager.Instance.FailTask(this, inworldCharacter, $"Character failed to reach the destination.");
                } else 
                    EntityManager.Instance.FailTask(this, inworldCharacter, $"Character can not go to that location.");
            } else 
                EntityManager.Instance.FailTask(this, inworldCharacter, $"Failed to find item: {itemID}.");
        }
    }
}
