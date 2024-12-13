/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inworld.Playground
{
    /// <summary>
    ///     Handles UI for a Control Panel.
    /// </summary>
    public class ControlUIPanel : MonoBehaviour
    {
        [SerializeField] protected TMP_Dropdown m_Dropdown;
        [SerializeField] protected Button m_PlayButton;
        
        protected int m_SelectionIndex;
        
        protected virtual void OnEnable()
        {
            if(m_Dropdown)
                m_Dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
            if(m_PlayButton)
                m_PlayButton.onClick.AddListener(OnPlayButtonClicked);
        }

        protected virtual void OnDisable()
        {
            if(m_Dropdown)
                m_Dropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
            if(m_PlayButton)
                m_PlayButton.onClick.RemoveListener(OnPlayButtonClicked);
        }
        
        protected virtual void OnDropdownValueChanged(int value)
        {
            m_SelectionIndex = value;
        }
        
        protected virtual void OnPlayButtonClicked()
        {
            
        }
    }
}
