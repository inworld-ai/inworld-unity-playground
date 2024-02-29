/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using UnityEngine;
using UnityEngine.Events;

namespace Inworld.Playground
{
    /// <summary>
    ///     Actives the OnTriggerEnterEvent when OnTriggerEnter is called.
    ///     For use with Trigger Collider objects.
    /// </summary>
    public class OnTrigger : MonoBehaviour
    {
        public UnityEvent OnTriggerEnterEvent;
        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterEvent?.Invoke();
        }

    }
}
