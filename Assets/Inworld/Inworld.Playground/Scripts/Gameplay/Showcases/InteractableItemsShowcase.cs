/*************************************************************************************************
 * Copyright 2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace Inworld.Playground
{
    /// <summary>
    ///     Handler for the Interactable Items showcase.
    /// </summary>
    public class InteractableItemsShowcase : MonoBehaviour
    {
        [SerializeField] private List<Interactable> m_Interactables;
        [SerializeField] private InworldCharacter m_InworldCharacter;

        void OnEnable()
        {
            m_InworldCharacter.Event.onGoalCompleted.AddListener(OnInworldCharacterGoalComplete);
        }

        void OnDisable()
        {
            m_InworldCharacter.Event.onGoalCompleted.RemoveListener(OnInworldCharacterGoalComplete);
        }

        void OnInworldCharacterGoalComplete(string brainName, string triggerName)
        {
            switch (triggerName)
            {
                case "enable_interactables":
                    foreach (var interactable in m_Interactables)
                        interactable.IsActive = true;
                    break;
            }
        }
    }
}

