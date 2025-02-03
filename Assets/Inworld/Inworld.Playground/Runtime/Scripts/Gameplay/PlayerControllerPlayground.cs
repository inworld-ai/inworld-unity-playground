/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using Inworld.Assets;
using Inworld.Sample;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inworld.Playground
{
    /// <summary>
    ///     Controls the player.
    ///     Inherits from Inworld's PlayerController.
    ///     Handles movement using Unity's legacy input system.
    /// </summary>
    public class PlayerControllerPlayground : PlayerController3D
    {
        protected void OnEnable()
        {
            onPlayerSpeaks.AddListener(OnPlayerSpeaks);
        }


        protected void OnDisable()
        {
            onPlayerSpeaks.RemoveListener(OnPlayerSpeaks);
        }

        void OnPlayerSpeaks(string text)
        {
            Subtitle.Instance.SetSubtitle(InworldAI.User.Name, text);
        }

        protected override void HandleInput()
        {
            if(Input.GetMouseButtonDown(0) && InteractionSystem.Instance.IsHoveringInteractable)
                InteractionSystem.Instance.Interact();
        }

    }
}
