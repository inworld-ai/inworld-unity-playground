/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

namespace Inworld.Playground
{
    /// <summary>
    ///     An interactable item that will save the game and play an animation on interaction.
    /// </summary>
    public class SaveButtonInteractable : AnimationInteractable
    {
        /// <summary>
        ///     Set the 'm_TriggerName' trigger of the referenced Animator and fetch the session history.
        /// </summary>
        protected override void _Interact()
        {
            base._Interact();
            InworldController.Client.GetHistoryAsync(InworldController.Instance.CurrentScene);
        }
    }
}
