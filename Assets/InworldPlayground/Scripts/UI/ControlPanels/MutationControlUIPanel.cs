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
    ///     Handles UI for a the Mutations Control Panel.
    /// </summary>
    public class MutationControlUIPanel : MonoBehaviour
    {
        [SerializeField] private InworldCharacter m_InworldCharacter;
        
        public void OnChocolateToggleValueChanged(bool value)
        {
            if (!value) return;
            
            m_InworldCharacter.SendTrigger("change_food_type", false, new Dictionary<string, string>()
            {
                {
                    "favorite_food",
                    "chocolate"
                }
            });
        }

        public void OnPizzaToggleValueChanged(bool value)
        {
            if (!value) return;
            
            m_InworldCharacter.SendTrigger("change_food_type", false, new Dictionary<string, string>()
            {
                {
                    "favorite_food",
                    "pizza"
                }
            });
        }
    }
}

