/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inworld.Playground
{
    /// <summary>
    ///     Handles updating the crosshair image.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class Crosshair : MonoBehaviour
    {
        [SerializeField] private Color m_InteractColor = Color.green;
        [SerializeField] private Color m_DefaultColor = Color.grey;
        private Image m_Image;
        private List<RaycastResult> m_RaycastResults = new List<RaycastResult>();

        private void Awake()
        {
            m_Image = GetComponent<Image>();
        }

        private void Update()
        {
            bool isHover = false;
            var graphicRaycasters = FindObjectsOfType<WorldSpaceGraphicRaycaster>(false);

            foreach (var graphicRaycaster in graphicRaycasters)
            {
                m_RaycastResults.Clear();
                graphicRaycaster.Raycast(new PointerEventData(EventSystem.current), m_RaycastResults);
                
                foreach (var result in m_RaycastResults)
                {
                    if (result.gameObject.GetComponentInParent<Selectable>())
                    {
                        isHover = true;
                        break;
                    }
                }

                if (isHover)
                    break;
            }
            
            if (isHover)
            {
                m_Image.color = m_InteractColor;
            }
            else
            {
                m_Image.color = m_DefaultColor;
            }
            
        }
    }
}
