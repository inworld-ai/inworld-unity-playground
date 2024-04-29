/*************************************************************************************************
 * Copyright 2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using Inworld.Assets;

namespace Inworld.Playground
{
    /// <summary>
    ///     Body Animation handling for the Gesture showcase Inworld characters.
    ///     Removes Idle animation handling from the base InworldBodyAnimation class.
    /// </summary>
    public class InworldNoIdleBodyAnimation : InworldBodyAnimation
    {
        protected override void OnCharacterSelected(string braiName)
        {

        }
        protected override void OnCharacterDeselected(string braiName)
        {

        }
        protected override void OnCharacterStartSpeaking(string brainName)
        {
            
        }

        protected override void OnCharacterEndSpeaking(string brainName)
        {
            
        }
    }
}

