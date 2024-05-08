/*************************************************************************************************
 * Copyright 2024 Theai, Inc. dba Inworld AI
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
    ///     Handles a showcase that has an input field.
    /// </summary>
    public class InputShowcase : MonoBehaviour
    {
        [SerializeField] protected TMP_InputField m_InputField;
        [SerializeField] protected Button m_Button;

        protected virtual void Awake()
        {
            m_Button.interactable = false;
        }
        
        protected virtual void OnEnable()
        {
            m_InputField.onSelect.AddListener(InputFieldOnSelect);
            m_InputField.onDeselect.AddListener(InputFieldOnDeselect);
        }

        protected virtual void OnDisable()
        {
            m_InputField.onSelect.RemoveListener(InputFieldOnSelect);
            m_InputField.onDeselect.RemoveListener(InputFieldOnDeselect);
        }
        
        protected virtual void InputFieldOnSelect(string text)
        {
            PlayerController.Instance.BlockKeyInput = true;
        }
        
        protected virtual void InputFieldOnDeselect(string text)
        {
            PlayerController.Instance.BlockKeyInput = false;
        }
        
        public virtual void OnInputValueChanged()
        {
            m_Button.interactable = !string.IsNullOrEmpty(m_InputField.text);
        }
    }
}
