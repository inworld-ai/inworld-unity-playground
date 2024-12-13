/*************************************************************************************************
 * Copyright 2024 Theai, Inc. dba Inworld AI
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
    ///     Handles UI for a the Mutations Control Panel.
    /// </summary>
    public class MutationControlUIPanel : MonoBehaviour
    {
        [SerializeField] private float m_SelectionIndicatorYOffset = 0.8f;
        [SerializeField] private InworldCharacter m_InworldCharacter;
        [SerializeField] private GameObject m_SelectionIndicator;
        [SerializeField] private GameObject m_CatObject;
        [SerializeField] private GameObject m_DogObject;
        
        private string m_CurrentSelectedAnimal;

        private void Awake()
        {
            m_CurrentSelectedAnimal = "cat";
            m_SelectionIndicator.transform.position = m_CatObject.transform.position + new Vector3(0, m_SelectionIndicatorYOffset, 0);
        }

        public void OnCurrentSelectedAnimalChanged(string animalName)
        {
            if (m_CurrentSelectedAnimal == animalName) return;

            m_CurrentSelectedAnimal = animalName;
            
            m_InworldCharacter.SendTrigger("change_favorite_animal", false, new Dictionary<string, string>()
            {
                {
                    "favorite_animal",
                    m_CurrentSelectedAnimal
                }
            });

            switch (m_CurrentSelectedAnimal)
            {
                case "cat":
                    m_SelectionIndicator.transform.position = m_CatObject.transform.position + new Vector3(0, m_SelectionIndicatorYOffset, 0);
                    break;
                case "dog":
                    m_SelectionIndicator.transform.position = m_DogObject.transform.position + new Vector3(0, m_SelectionIndicatorYOffset, 0);
                    break;
            }
        }
    }
}

