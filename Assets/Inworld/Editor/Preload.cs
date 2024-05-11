/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Inworld.Playground
{
    [InitializeOnLoad]
    public static class Preload
    {
        static Preload()
        {
            AssetDatabase.importPackageCompleted += async packageName => {
                Debug.Log($"Import Package Complete: {packageName}.");
                switch (packageName)
                {
                    case "InworldAI.Playground":
                        InstallPlayground();
                        break;
                    case "InworldPlaygroundAssets":
                        EditorUtility.DisplayProgressBar("Inworld Playground", "Updating Build Settings...", 0.8f);
                        if (!BuildSettingsUpdater.UpdateBuildSettings())
                        {
                            EditorUtility.ClearProgressBar();
                            EditorUtility.DisplayDialog("Inworld Playground", "Installation Failed.", "Ok");
                        }
     
                        EditorUtility.ClearProgressBar();
                        
                        if(await DependencyImporter.CheckDependencies())
                            EditorUtility.DisplayDialog("Inworld Playground", "Installation Complete.", "Ok");
                        else
                            EditorUtility.DisplayDialog("Inworld Playground", "Installation incomplete, missing dependency packages.", "Ok");
                        break;
                }
            };
        }
        
        [MenuItem("Inworld/Playground/Install", priority = 100)]
        static async void InstallPlayground()
        {
            if (!EditorUtility.DisplayDialog("Inworld Playground", "Click 'Ok' to begin installing the Inworld Playground project.", "Ok", "Cancel"))
                return;
            
            if (await DependencyImporter.CheckDependencies())
                Debug.Log("Found all dependencies.");
            else if (!await DependencyImporter.InstallDependencies())
            {
                EditorUtility.DisplayDialog("Inworld Playground", "Installation Failed.", "Ok");
                return;
            }
            
            QualitySettings.pixelLightCount = 8;
                        
            if (File.Exists($"Assets/Inworld/InworldPlaygroundAssets.unitypackage"))
            {
                Debug.Log("Importing Inworld Playground Assets.");
                AssetDatabase.ImportPackage("Assets/Inworld/InworldPlaygroundAssets.unitypackage", true);
            }
            else
            {
                Debug.LogError("Could not find InworldPlaygroundAssets.unitypackage.");
                EditorUtility.DisplayDialog("Inworld Playground", "Installation Failed.", "Ok");
            }
        }
    }
}
#endif