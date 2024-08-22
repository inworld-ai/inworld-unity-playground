/*************************************************************************************************
 * Copyright 2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using UnityEngine;

namespace Inworld.Map
{
    public abstract class Task : ScriptableObject
    {
        public string ID => id;
        [SerializeField] private string id;
        public abstract bool PerformTask(InworldCharacter inworldCharacter);
    }
    [CreateAssetMenu(fileName = "GoTask", menuName = "Inworld/Tasks/GoTask")]
    public class GoTask : Task
    {
        [SerializeField] private Transform destinationTransform;

        public override bool PerformTask(InworldCharacter inworldCharacter)
        {
            // NavMeshAgent navMeshAgent = inworldCharacter.GetComponent<NavMeshAgent>();
            // if (navMeshAgent && NavMesh.FindClosestEdge(destinationTransform.position, out NavMeshHit navMeshHit, NavMesh.AllAreas))
            // {
            //     
            // }
            return false;
        }
    }
}
