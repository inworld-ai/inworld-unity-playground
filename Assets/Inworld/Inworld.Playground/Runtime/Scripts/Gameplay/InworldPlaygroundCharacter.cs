/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections.Generic;
using Inworld.Entities;
using Inworld.Packet;
using UnityEngine;
using UnityEngine.Events;

namespace Inworld.Playground
{
    
    /// <summary>
    ///     An Inworld character in the Playground.
    ///     This character type will not automatically set itself as the Current Character on registration.
    /// </summary>
    public class InworldPlaygroundCharacter : InworldCharacter
    {
        #region Events
        /// <summary>
        /// Event is invoked when this character receives a trigger response.
        /// Includes the trigger name and parameters as a dictionary. 
        /// </summary>
        public event Action<string, Dictionary<string, string>> OnServerTrigger;
        

        #endregion"

        public void SendTrigger(string triggerName)
        {
            base.SendTrigger(triggerName);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            m_CharacterEvents.onCharacterDeselected.AddListener(OnCharacterDeselected);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            m_CharacterEvents?.onCharacterDeselected.RemoveListener(OnCharacterDeselected);
        }

        protected virtual void OnCharacterDeselected(string brainName)
        {
            CancelResponse();
        }

        protected virtual void Start()
        {
            InworldController.CharacterHandler.Register(this);
        }
        
        protected override bool HandleText(TextPacket packet)
        {
            if (packet.text == null || string.IsNullOrWhiteSpace(packet.text.text))
                return false;
            
            base.HandleText(packet);
            
            if (packet.Source == SourceType.AGENT && packet.IsSource(ID))
                Subtitle.Instance.SetSubtitle(Name, packet.text.text);
            return true;
        }
        
        protected override bool HandleTrigger(CustomPacket customPacket)
        {
            if (customPacket.Message == InworldMessage.RelationUpdate || customPacket.Message == InworldMessage.Task)
                return base.HandleTrigger(customPacket);
            
            var parameterDictionary = new Dictionary<string, string>();
            foreach (var parameter in customPacket.custom.parameters)
                parameterDictionary.Add(parameter.name, parameter.value);
            //OnServerTrigger(customPacket.TriggerName, parameterDictionary);
            
            return base.HandleTrigger(customPacket);
        }
    }
}
