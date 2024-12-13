/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using UnityEngine;
using UnityEngine.UI;

namespace Inworld.Playground
{
    /// <summary>
    ///     Handles updating the reticle image.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class Reticle : MonoBehaviour
    {
        [SerializeField] private Color m_InteractColor = Color.green;
        [SerializeField] private Color m_DefaultColor = Color.grey;
        private Image m_Image;

        private void Awake()
        {
            m_Image = GetComponent<Image>();
        }

        private void Update()
        {
            if (InteractionSystem.Instance.IsHoveringUI || InteractionSystem.Instance.IsHoveringInteractable)
                m_Image.color = m_InteractColor;
            else
                m_Image.color = m_DefaultColor;
        }
    }
}
