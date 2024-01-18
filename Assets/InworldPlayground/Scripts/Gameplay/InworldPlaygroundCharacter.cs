/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using Inworld.Entities;
using UnityEngine;

namespace Inworld.Playground
{   
    /// <summary>
    ///     An Inworld character in the Playground.
    ///     This character type will not automatically set itself as the Current Character on registration.
    /// </summary>
    public class InworldPlaygroundCharacter : InworldCharacter
    {
        /// <summary>
        /// Register live session. Get the live session ID for this character, and also assign it to this character's components.
        /// </summary>
        public override void RegisterLiveSession()
        {
            m_Interaction.LiveSessionID = Data.agentId = InworldController.CharacterHandler.GetLiveSessionID(this);
        }
        
        protected override void OnCharRegistered(InworldCharacterData charData)
        {
            if (charData.brainName == Data.brainName)
                RegisterLiveSession();
        }
    }
}
