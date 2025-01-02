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
    /// <summary>
    /// The Data class for holding all the references.
    /// Please put the asset under k_InstancePath and do not modify it.
    /// </summary>
    public class InworldPlayground : ScriptableObject
    {
        const string k_InstancePath = "Assets/Inworld/Inworld.Playground/Runtime/InworldPlayground.asset";
        const string k_CloneToken = "p9ZYx2gBsFHH";
        static InworldPlayground __inst;

        [SerializeField] InworldGameData m_GameData;
        [SerializeField] PopulationData m_PopulationData;
        
        /// <summary>
        /// Gets an instance of InworldAI.
        /// By default, it is at `Assets/Inworld/Inworld.Playground/Runtime/InworldPlayground.asset`.
        /// Please do not modify it.
        /// </summary>
        public static InworldPlayground Instance
        {
            get
            {
                if (__inst)
                    return __inst;
                __inst = AssetDatabase.LoadAssetAtPath<InworldPlayground>(k_InstancePath);
                return __inst;
            }
        }
        /// <summary>
        /// Get/Set the game data indicating the cloned playground workspace.
        /// </summary>
        public static InworldGameData GameData
        {
            get => Instance.m_GameData;
            internal set => Instance.m_GameData = value;
        } 
        /// <summary>
        /// Gets the clone token
        /// </summary>
        public static string CloneToken => k_CloneToken;
        public PopulationData PopulationData => m_PopulationData;
    }
}