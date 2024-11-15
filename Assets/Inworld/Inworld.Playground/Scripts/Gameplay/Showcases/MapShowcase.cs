/*************************************************************************************************
 * Copyright 2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/
using System;
using Inworld.Map;
using UnityEngine;

namespace Inworld.Playground
{
    public class MapShowcase : MonoBehaviour
    {
        [SerializeField] private InworldCharacter m_InworldCharacter;
        private void Start()
        {
            InworldController.CurrentCharacter = m_InworldCharacter;
        }
        
    }
}
