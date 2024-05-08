/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using Inworld.Entities;
using UnityEngine;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using Inworld.Editors;
#endif

namespace Inworld.Playground
{
    /// <summary>
    ///     Utility class for handling serializing the Game Data object for the Playground Workspace.
    ///     Primarily for use in the Unity editor.
    /// </summary>
    public static class Serialization
    {
        const string playgroundDirectory = "Inworld/Inworld.Playground";
        const string sceneName = "inworld_playground_lobby";
        const string gameDataAssetName = "PlaygroundData";
        
#if UNITY_EDITOR
        /// <summary>
        ///     Gets the file system path where the Game Data object will be stored.
        /// </summary>
        /// <param name="onlyDirectory">Whether to include the file name or just directory path.</param>
        /// <returns>Returns the file system path for the Game Data object as a string.</returns>
        public static string GetGameDataPath(bool onlyDirectory = false)
        {
            if(onlyDirectory)
                return $"Assets/{playgroundDirectory}/{InworldEditor.GameDataPath}/";
            else
                return $"Assets/{playgroundDirectory}/{InworldEditor.GameDataPath}/{gameDataAssetName}.asset";
        }
#endif
        /// <summary>
        ///     Get the current Game Data object (if one exists).
        /// </summary>
        /// <returns>Returns either the Game Data object or null if one does not exist. Always returns null outside
        /// the Unity editor.</returns>
        public static InworldGameData GetGameData()
        {
            InworldGameData gameData = null;
#if UNITY_EDITOR
            gameData = AssetDatabase.LoadAssetAtPath<InworldGameData>(GetGameDataPath());
#endif
            return gameData;
        }
        /// <summary>
        ///     Creates a new/updates the current Game Data object using the given key, secret, and workspaceId.
        /// </summary>
        /// <param name="key">The API key for the Playground Workspace.</param>
        /// <param name="secret">The API secret for the Playground Workspace.</param>
        /// <param name="workspaceId">The ID of the Playground Workspace.</param>
        /// <returns>Returns the created/updated Game Data object.</returns>
        public static InworldGameData CreateGameData(string key, string secret, string workspaceId)
        {
            var gameData = GetGameData();
            if(gameData == null)
                gameData = ScriptableObject.CreateInstance<InworldGameData>();
            
            string sceneFullName = $"workspaces/{workspaceId}/scenes/{sceneName}";
            gameData.sceneFullName = sceneFullName;
            gameData.apiKey = key;
            gameData.apiSecret = secret;
            if(gameData.capabilities == null)
                gameData.capabilities = new Capabilities();
            gameData.capabilities.audio = true;
            gameData.capabilities.emotions = true;
            gameData.capabilities.interruptions = true;
            gameData.capabilities.narratedActions = true;
            gameData.capabilities.regenerateResponse = true;
            gameData.capabilities.text = true;
            gameData.capabilities.triggers = true;
            gameData.capabilities.phonemeInfo = true;
            gameData.capabilities.relations = true;
#if UNITY_EDITOR
            if (!AssetDatabase.Contains(gameData))
            {
                if (!Directory.Exists(GetGameDataPath(true)))
                    Directory.CreateDirectory(GetGameDataPath(true));
                AssetDatabase.CreateAsset(gameData, GetGameDataPath());
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
            return gameData;
        }
    }
}
