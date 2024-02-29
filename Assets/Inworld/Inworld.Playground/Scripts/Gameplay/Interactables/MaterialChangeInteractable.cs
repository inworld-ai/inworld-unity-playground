/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace Inworld.Playground
{
    /// <summary>
    ///     An interactable item that will change its mesh's material on interaction.
    /// </summary>
    [RequireComponent(typeof(MeshRenderer))]
    public class MaterialChangeInteractable : InworldTriggerInteractable
    {
        [Header("Material Change")]
        [SerializeField] private List<Material> m_Materials;
        private MeshRenderer m_MeshRenderer;
        private Material m_CurrentMaterial;

        void Awake()
        {
            m_MeshRenderer = GetComponent<MeshRenderer>();
            SetMaterial();
        }

        /// <summary>
        ///     Randomly swap the material of this mesh.
        /// </summary>
        protected override void _Interact()
        {
            base._Interact();
            SetMaterial();
        }

        void SetMaterial()
        {
            if (m_Materials != null && m_Materials.Count > 0)
            {
                var materials = new List<Material>(m_Materials);
                materials.Remove(m_CurrentMaterial);
                
                m_CurrentMaterial = materials[Random.Range(0, materials.Count)];
                m_MeshRenderer.material = m_CurrentMaterial;
            }
        }
    }
}
