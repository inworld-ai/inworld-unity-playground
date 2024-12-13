/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Inworld.Playground
{
    /// <summary>
    ///     An interactable item that will trigger a Unity event when interacted with.
    /// </summary>
    [RequireComponent(typeof(MeshRenderer))]
    public class EventInteractable : Interactable
    {
        public UnityEvent InteractionEvent;    
        
        /// <summary>
        ///     Invoke the interaction event.
        /// </summary>
        protected override void _Interact()
        {
            InteractionEvent?.Invoke();
        }
        
    }
}
