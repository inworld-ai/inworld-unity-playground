/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Inworld.Playground
{
    /// <summary>
    ///     A UI control panel for handling sending triggers to the Rhyme Bot for the Parameters showcase.
    /// </summary>
    public class ParametersControlUIPanel : MonoBehaviour
    {
        [SerializeField] private InworldCharacter m_InworldCharacter;
        [SerializeField] private TMP_InputField m_InputField;

        void OnEnable()
        {
            m_InputField.onSelect.AddListener(InputFieldOnSelect);
            m_InputField.onDeselect.AddListener(InputFieldOnDeselect);
        }

        void OnDisable()
        {
            m_InputField.onSelect.RemoveListener(InputFieldOnSelect);
            m_InputField.onDeselect.RemoveListener(InputFieldOnDeselect);
        }
        
        void InputFieldOnSelect(string text)
        {
            PlayerController.Instance.BlockKeyInput = true;
        }
        
        void InputFieldOnDeselect(string text)
        {
            PlayerController.Instance.BlockKeyInput = false;
        }

        /// <summary>
        ///     Send a trigger to active the "rhyme" goal on the Inworld Character with the m_InputField text as
        ///      the parameter "phrase".
        /// </summary>
        public void SendTrigger()
        {
            m_InworldCharacter.SendTrigger("rhyme", true, new Dictionary<string, string>()
            {
                {"phrase", m_InputField.text} 
            });
        }
    }
}

