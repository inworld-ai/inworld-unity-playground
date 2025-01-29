/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using UnityEngine;
namespace Inworld.Playground
{
    /// <summary>
    ///     An object that can be interacted with by the Player.
    /// </summary>
    [Obsolete]
    public abstract class Interactable : MonoBehaviour
    {
        /// <summary>
        ///     Determines if you are able to interact with this object.
        /// </summary>
        public bool IsActive;
        /// <summary>
        ///     Interact with this object. Typically used to perform some kind of action.
        /// </summary>
        public void Interact()
        {
            if (!IsActive) return;

            _Interact();
        }

        protected abstract void _Interact();
    }
}
