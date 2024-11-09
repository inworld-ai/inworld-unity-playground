/*************************************************************************************************
 * Copyright 2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Inworld.Packet;
using UnityEngine;
using UnityEngine.AI;

namespace Inworld.Map
{
    [CreateAssetMenu(fileName = "ExamineTask", menuName = "Inworld/Tasks/ExamineTask")]
    public class ExamineTask : Task
    {
        public override IEnumerator Perform(InworldCharacter inworldCharacter, Dictionary<string, string> parameters)
        {
            if (parameters.TryGetValue("what", out string itemID) && EntityManager.Instance.FindItem(itemID, out EntityItem entityItem))
            {
                string log = $"{inworldCharacter.Name} examines {entityItem.DisplayName}: {entityItem.Description}";
                log = parameters.Aggregate(log, (current, param) => current + $"\n{param.Key}: {param.Value}");
                InworldAI.Log(log);
                EntityManager.Instance.CompleteTask(this, inworldCharacter, parameters);
            } else 
                EntityManager.Instance.FailTask(this, inworldCharacter, $"Failed to find item: {itemID}.", parameters);
            yield break;
        }
    }
}
