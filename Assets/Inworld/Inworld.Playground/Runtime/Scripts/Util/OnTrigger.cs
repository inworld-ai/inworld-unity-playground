/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using UnityEngine;
using UnityEngine.Events;

namespace Inworld.Playground
{
    // TODO(Yan): Remove this class as it's too inefficient. All the MonoBehavior should handle OnTriggerEnter itself.
    /// <summary>
    ///     Actives the OnTriggerEnterEvent when OnTriggerEnter is called.
    ///     For use with Trigger Collider objects.
    /// </summary>
    [Obsolete]
    public class OnTrigger : MonoBehaviour
    {
        #region Actions

        public event Action OnTriggerEnterEvent;

        #endregion
        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterEvent?.Invoke();
        }

    }
}
