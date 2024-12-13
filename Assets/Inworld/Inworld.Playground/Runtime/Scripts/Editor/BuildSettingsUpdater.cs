/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/
#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Inworld.Playground
{
    public static class BuildSettingsUpdater
    {
        public static bool UpdateBuildSettings()
        {
            Debug.Log("Adding Playground scenes to Build Settings...");

            List<string> sceneGUIDs = new List<string>(AssetDatabase.FindAssets("t:scene", new[] { "Assets/Inworld/Inworld.Playground" }));

            if (sceneGUIDs.Count == 0)
            {
                Debug.LogError("Could not find any Playground scenes. Please manually add the Playground " +
                               "scenes to the Build Settings");
                return false;
            }

            sceneGUIDs.RemoveAll((sceneGUID) => EditorBuildSettings.scenes.FirstOrDefault((scene) => scene.guid.ToString() == sceneGUID) != null);
            
            if (sceneGUIDs.Count == 0)
            {
                Debug.Log("Build Settings already contains all Playground scenes.");
                return true;
            }

            int totalNumberScenes = sceneGUIDs.Count + EditorBuildSettings.scenes.Length;
            EditorBuildSettingsScene[] editorBuildSettingScenes = new EditorBuildSettingsScene[totalNumberScenes];
            
            for(int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                editorBuildSettingScenes[i] = EditorBuildSettings.scenes[i];
            }
            for(int i = 0; i < sceneGUIDs.Count; i++)
            {
                GUID guid = new GUID(sceneGUIDs[i]);
                editorBuildSettingScenes[EditorBuildSettings.scenes.Length + i] = new EditorBuildSettingsScene(guid, true);
            }
            
            EditorBuildSettings.scenes = editorBuildSettingScenes;
            Debug.Log("Completed adding scenes to Build Settings.");
            return true;
        }

    }
}
#endif
