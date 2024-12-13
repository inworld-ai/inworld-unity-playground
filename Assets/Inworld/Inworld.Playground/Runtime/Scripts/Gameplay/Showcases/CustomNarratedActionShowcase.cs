/*************************************************************************************************
 * Copyright 2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using UnityEngine;
namespace Inworld.Playground
{
    /// <summary>
    ///     Handles the Custom Narrated Action showcase which allows a custom narrated action input
    ///      by the player to be sent to an Inworld character.
    /// </summary>
    public class CustomNarratedActionShowcase : InputShowcase
    {
        [SerializeField] protected InworldCharacter m_InworldCharacter;
        
        /// <summary>
        ///     Sends a narrated action to m_InworldCharacter using the input from m_InputField.
        /// </summary>
        public void SendNarratedAction()
        {
            if (string.IsNullOrEmpty(m_InputField.text)) return;
            
            m_InworldCharacter.SendNarrative(m_InputField.text);
            m_InworldCharacter.SendTrigger("narrated-action-comment");
        }
    }
}
