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
        public override IEnumerator Perform(InworldCharacter inworldCharacter, Dictionary<string, string> parameters)
        {
            if (parameters.TryGetValue("where", out string itemID) && EntityManager.Instance.FindItem(itemID, out EntityItem entityItem))
            {
                EntityItem characterEntity = inworldCharacter.GetComponent<EntityItem>();
                if (!characterEntity)
                {
                    EntityManager.Instance.FailTask(this, inworldCharacter, $"Failed to find Entity Item component for {inworldCharacter.Name}", parameters);
                    yield break;
                }
                
                NavMeshAgent navMeshAgent = inworldCharacter.GetComponent<NavMeshAgent>();
                Vector3 dirToCharacter = inworldCharacter.transform.position - entityItem.transform.position;
                Vector3 samplePosition = entityItem.transform.position + new Vector3(dirToCharacter.x, 0, dirToCharacter.z).normalized;
                if (navMeshAgent && NavMesh.SamplePosition(samplePosition, out NavMeshHit navMeshHit, m_MaxHitDistance,NavMesh.AllAreas) &&
                    navMeshAgent.SetDestination(navMeshHit.position))
                {
                    while (navMeshAgent.pathPending || (navMeshAgent.hasPath && navMeshAgent.remainingDistance > m_DestinationReachedThreshold))
                        yield return null;

                    if (navMeshAgent.remainingDistance <= m_DestinationReachedThreshold)
                    {
                        
                        characterEntity.UpdateProperty("location", $"Character is near {entityItem.DisplayName}.");
                        EntityManager.Instance.CompleteTask(this, inworldCharacter, parameters);
                    }
                    else
                    {
                        characterEntity.UpdateProperty("location", "Unknown.");
                        EntityManager.Instance.FailTask(this, inworldCharacter, $"Character failed to reach the destination.", parameters);
                    }
                } else 
                    EntityManager.Instance.FailTask(this, inworldCharacter, $"Character can not go to that location.", parameters);
            } else 
                EntityManager.Instance.FailTask(this, inworldCharacter, $"Failed to find item: {itemID}.", parameters);
        }
    }
}
