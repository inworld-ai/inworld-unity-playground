/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using Inworld.Entities;
using Inworld.Packet;
using UnityEngine.Events;

namespace Inworld.Playground
{   
    /// <summary>
    ///     An Inworld character in the Playground.
    ///     This character type will not automatically set itself as the Current Character on registration.
    /// </summary>
    public class InworldPlaygroundCharacter : InworldCharacter
    {
        /// <summary>
        /// Event is invoked when this character receives a trigger response.
        /// Includes the trigger name and parameters as a dictionary. 
        /// </summary>
        public UnityEvent<string, Dictionary<string, string>> onServerTrigger;

        protected virtual void Start()
        {
            InworldController.CharacterHandler.Register(this);
        }
        
        protected override void HandleText(TextPacket packet)
        {
            base.HandleText(packet);
            Subtitle.Instance.SetSubtitle(Name, packet.text.text);
        }
        
        protected override void HandleTrigger(CustomPacket customPacket)
        {
            base.HandleTrigger(customPacket);
            
            if (customPacket.Message == InworldMessage.RelationUpdate)
                return;
            
            var parameterDictionary = new Dictionary<string, string>();
            foreach (var parameter in customPacket.custom.parameters)
                parameterDictionary.Add(parameter.name, parameter.value);
            onServerTrigger.Invoke(customPacket.TriggerName, parameterDictionary);
        }
    }
}
