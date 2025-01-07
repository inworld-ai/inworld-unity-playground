/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

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
        [SerializeField] InworldGameData m_GameData;
        [Header("Default Settings:")]
        [SerializeField] InworldGameData m_DefaultGameData;
        [SerializeField] PopulationData m_PopulationData;

        const string k_InstancePath = "InworldPlayground";
        static InworldPlayground __inst;
        
        /// <summary>
        /// Gets an instance of Playground.
        /// By default, it is at `Assets/Inworld/Inworld.Playground/Runtime/Resources/InworldPlayground.asset`.
        /// Please do not modify it.
        /// </summary>
        public static InworldPlayground Instance
        {
            get
            {
                if (__inst)
                    return __inst;
                __inst = Resources.Load<InworldPlayground>(k_InstancePath);
                return __inst;
            }
        }
        /// <summary>
        /// Get/Set the game data indicating the cloned playground workspace.
        /// </summary>
        public static InworldGameData GameData
        {
            get => Instance.m_GameData ?? Instance.m_DefaultGameData;
            set => Instance.m_GameData = value;
        }
        public InworldGameData DefaultGameData => m_DefaultGameData;
        /// <summary>
        /// Gets if the game data is default.
        /// </summary>
        public static bool IsDefaultGameData => GameData == Instance.m_DefaultGameData;
        /// <summary>
        /// Gets the Population Data.
        /// </summary>
        public PopulationData PopulationData => m_PopulationData;
    }
}