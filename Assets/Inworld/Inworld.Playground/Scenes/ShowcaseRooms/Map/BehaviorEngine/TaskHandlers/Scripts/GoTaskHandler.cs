using Inworld;
using Inworld.BehaviorEngine;
using System.Collections;
using System.Collections.Generic;
using Inworld.Map;
using UnityEngine;
using UnityEngine.AI;

public class GoTaskHandler : TaskHandler
{
    [SerializeField] protected float m_MaxHitDistance = 5f;
    [SerializeField] protected float m_DestinationReachedThreshold = 0.1f;

    protected EntityItem m_CharacterEntityItem;
    protected NavMeshAgent m_NavMeshAgent;
    
    public override bool Validate(Task task, InworldCharacter inworldCharacter, Dictionary<string, string> parameters, out string message)
    {
        if(!base.Validate(task, inworldCharacter, parameters, out message))
            return false;

        m_CharacterEntityItem = inworldCharacter.GetComponent<EntityItem>();
        if (!m_CharacterEntityItem)
        {
            message = $"Failed to find Entity Item component for {inworldCharacter.Name}";
            return false;
        }
        
        m_NavMeshAgent = inworldCharacter.GetComponent<NavMeshAgent>();
        if(!m_NavMeshAgent)
        {
            message = $"{inworldCharacter.Name} is unable to move.";
            return false;
        }
        
        return true;
    }

    public override IEnumerator Execute(InworldCharacter inworldCharacter)
    {
        EntityItem whereItem = m_EntityItems["where"];
        
        Vector3 dirToCharacter = inworldCharacter.transform.position - whereItem.transform.position;
        Vector3 samplePosition = whereItem.transform.position + new Vector3(dirToCharacter.x, 0, dirToCharacter.z).normalized;
        if (m_NavMeshAgent && NavMesh.SamplePosition(samplePosition, out NavMeshHit navMeshHit, m_MaxHitDistance,NavMesh.AllAreas) &&
            m_NavMeshAgent.SetDestination(navMeshHit.position))
        {
            while (m_NavMeshAgent.pathPending || (m_NavMeshAgent.hasPath && m_NavMeshAgent.remainingDistance > m_DestinationReachedThreshold))
                yield return null;

            if (m_NavMeshAgent.remainingDistance <= m_DestinationReachedThreshold)
            {
                m_CharacterEntityItem.UpdateProperty("location", $"Character is near {whereItem.DisplayName}.");
                Complete();
            }
            else
            {
                m_CharacterEntityItem.UpdateProperty("location", "Unknown.");
                Fail($"Character failed to reach the destination.");
            }
        } else 
            Fail($"Character can not go to that location.");
    }
}
