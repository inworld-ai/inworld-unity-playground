/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inworld.Playground
{
    /// <summary>
    ///     An interactable item that will send an Inworld Trigger on interaction.
    /// </summary>
    [RequireComponent(typeof(MeshRenderer))]
    public class InworldTriggerInteractable : Interactable
    {
        [Header("Inworld Trigger")]
        [SerializeField] private InworldCharacter m_InworldCharacter;
        [SerializeField] private string m_TriggerName;
        [SerializeField] private List<DictionaryItem> m_TriggerParameterList;

        private Dictionary<string, string> m_TriggerParameters;

        void Awake()
        {
            m_TriggerParameters = new Dictionary<string, string>();
            foreach (var triggerParameter in m_TriggerParameterList)
                m_TriggerParameters.Add(triggerParameter.Key, triggerParameter.Value);
        }

        /// <summary>
        ///     Send the trigger to the set Inworld Character.
        /// </summary>
        protected override void _Interact()
        {
            if (!m_InworldCharacter) return;
            
            m_InworldCharacter.SendTrigger(m_TriggerName, true, m_TriggerParameters);
        }
        
    }
}
