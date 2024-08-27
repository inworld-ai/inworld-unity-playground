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

namespace Inworld.Map
{
    [CreateAssetMenu(fileName = "OpenTask", menuName = "Inworld/Tasks/OpenTask")]
    public class OpenTask : Task
    {
        public override IEnumerator Perform(InworldCharacter inworldCharacter, List<TriggerParameter> parameters)
        {
            Dictionary<string, string> parameterDictionary = ParseParameters(parameters);
            if (!parameterDictionary.TryGetValue("what", out string itemID) || !EntityManager.Instance.FindItem(itemID, out EntityItem entityItem))
            {
                EntityManager.Instance.FailTask(this, inworldCharacter, $"Failed to find item: {itemID}.");
                yield break;
            }
            
            AnimatorBoolParamHandler animatorBoolParamHandler = entityItem.GetComponent<AnimatorBoolParamHandler>();
            if (!animatorBoolParamHandler)
            {
                EntityManager.Instance.FailTask(this, inworldCharacter, $"Could not open {itemID}.");
                yield break;
            }
            
            if(animatorBoolParamHandler.SetTrue())
                EntityManager.Instance.CompleteTask(this, inworldCharacter);
            else
                EntityManager.Instance.FailTask(this, inworldCharacter, $"{itemID} is already open.");
        }
    }
}
