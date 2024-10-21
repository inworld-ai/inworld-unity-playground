/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using UnityEngine;

namespace Inworld.Playground
{
    /// <summary>
    ///     An interactable item that will apply an impulse force to the Rigidbody of this object on interaction.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class ApplyForceInteractable : InworldTriggerInteractable
    {
        [Header("Apply Force")]
        [SerializeField] private Vector3 m_Force;
        private Rigidbody m_Rigidbody;

        protected override void Awake()
        {
            base.Awake();
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        /// <summary>
        ///     Apply an impulse force to the RigidBody of this object at its current transform position.
        /// </summary>
        protected override void _Interact()
        {
            base._Interact();
            m_Rigidbody.AddForceAtPosition(m_Force, transform.position, ForceMode.Impulse);
        }
    }
}
