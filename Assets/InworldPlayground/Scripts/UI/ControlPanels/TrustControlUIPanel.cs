/*************************************************************************************************
 * Copyright 2022-2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using UnityEngine;


namespace Inworld.Playground
{
    /// <summary>
    ///     Handles UI for a the Trust Control Panel.
    /// </summary>
    public class TrustControlUIPanel : MonoBehaviour
    {
        [SerializeField] private InworldCharacter m_InworldCharacter;
        
        public void OnSuspiciousToggleValueChanged(bool value)
        {
            if (!value) return;

            m_InworldCharacter.SendTrigger("trust_level1");
        }

        public void OnUncertainToggleValueChanged(bool value)
        {
            if (!value) return;
            
            m_InworldCharacter.SendTrigger("trust_level2");
        }
        
        public void OnTrustingToggleValueChanged(bool value)
        {
            if (!value) return;
            
            m_InworldCharacter.SendTrigger("trust_level3");
        }
    }
}
