/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/
#if UNITY_EDITOR
using UnityEditor;
#endif

using Inworld.Playground.Data;
using UnityEngine;

namespace Inworld.Playground
{
    public class InworldPlayground : ScriptableObject
    {
        const string k_GlobalDataPath = "InworldPlayground";
        static InworldPlayground __inst;
        [SerializeField] PopulationData m_PopulationData;
        
        /// <summary>
        /// Gets an instance of InworldAI.
        /// By default, it is at `Assets/Inworld/Inworld.Playground/Resources/InworldPlayground.asset`.
        /// Please do not modify it.
        /// </summary>
        public static InworldPlayground Instance
        {
            get
            {
                if (__inst)
                    return __inst;
                __inst = Resources.Load<InworldPlayground>(k_GlobalDataPath);
                return __inst;
            }
        }
        public PopulationData PopulationData => m_PopulationData;
    }
}