/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using Inworld.Assets;
using Inworld.Packet;
using TMPro;
using UnityEngine;

namespace Inworld.Playground
{
    /// <summary>
    ///     Control UI Panel for setting Inworld characters emotions.
    /// </summary>
    public class EmotionControlUIPanel : ControlUIPanel
    {
        [SerializeField] private List<InworldCharacter> m_InworldCharacters;
        [SerializeField] private EmotionMap m_EmotionMap;

        private int currentEmotionIndex;
        
        private void Awake()
        {
            m_Dropdown.options.Clear();
            foreach (var emotion in m_EmotionMap.data)
                m_Dropdown.options.Add(new TMP_Dropdown.OptionData(emotion.name.ToString()));

            currentEmotionIndex = 0;
        }
        
        protected override void OnDropdownValueChanged(int value)
        {
            if (value == currentEmotionIndex)
                return;

            currentEmotionIndex = value;
            foreach (var inworldCharacter in m_InworldCharacters)
                inworldCharacter.SendTrigger($"emotion_{m_EmotionMap.data[value].name}");
        }
    }
}
