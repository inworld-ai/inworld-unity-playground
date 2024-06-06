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
    ///     Handles Player interactions with objects.
    /// </summary>
    public class InteractionSystem : SingletonBehavior<InteractionSystem>
    {
        [Header("Item Interaction")]
        [SerializeField] private float m_Range = 3;
        
        public bool IsHoveringInteractable => m_CurrentInteractable != null && m_CurrentInteractable.IsActive;
        public bool IsHoveringUI => m_IsHoveringUI;
        
        private List<RaycastResult> m_RaycastResults = new List<RaycastResult>();
        private Camera m_Camera;
        
        private bool m_IsHoveringUI;
        private Interactable m_CurrentInteractable;

        public void Interact()
        {
            if (m_CurrentInteractable == null) return;
            
            m_CurrentInteractable.Interact();
        }

        void Awake()
        {
            m_Camera = Camera.main;
        }
        
        void Update()
        {
            var graphicRaycasters = FindObjectsOfType<WorldSpaceGraphicRaycaster>(false);
            m_IsHoveringUI = false;
            foreach (var graphicRaycaster in graphicRaycasters)
            {
                m_RaycastResults.Clear();
                graphicRaycaster.Raycast(new PointerEventData(EventSystem.current), m_RaycastResults);
                
                foreach (var result in m_RaycastResults)
                {
                    Selectable selectable = result.gameObject.GetComponentInParent<Selectable>();
                    if (selectable && selectable.interactable)
                    {
                        m_IsHoveringUI = true;
                        break;
                    }
                }

                if (m_IsHoveringUI)
                    break;
            }

            m_CurrentInteractable = null;
            if (Physics.Raycast(m_Camera.transform.position, m_Camera.transform.forward, out RaycastHit hitInfo, m_Range))
                m_CurrentInteractable = hitInfo.collider.GetComponentInParent<Interactable>();
        }
    }
}
