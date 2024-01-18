/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Inworld.Playground
{
    public static class BuildSettingsUpdater
    {
        public static void UpdateBuildSettings()
        {
            Debug.Log("Adding Playground scenes to Build Settings...");

            string[] sceneGUIDs = AssetDatabase.FindAssets("t:scene", new[] { "Assets/InworldPlayground" });

            if (sceneGUIDs.Length == 0)
            {
                Debug.LogError("Could not find any Playground scenes. Please manually add the Playground " +
                               "scenes to the Build Settings");
                return;
            }
            
            var editorBuildSettingScenes = new EditorBuildSettingsScene[sceneGUIDs.Length];

            for(int i = 0; i < sceneGUIDs.Length; i++)
            {
                GUID guid = new GUID(sceneGUIDs[i]);
                editorBuildSettingScenes[i] = new EditorBuildSettingsScene(guid, true);
            }

            EditorBuildSettings.scenes = editorBuildSettingScenes;
            Debug.Log("Completed adding scenes to Build Settings.");
        }

    }
}
#endif
