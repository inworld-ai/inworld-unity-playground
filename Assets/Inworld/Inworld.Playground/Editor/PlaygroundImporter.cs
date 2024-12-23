/*************************************************************************************************
 * Copyright 2022-2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace Inworld.Playground
{
    [InitializeOnLoad]
    public class PlaygroundImporter : AssetPostprocessor
    {
        const string k_LegacyPkgName = "Inworld.AI";
        const string k_InworldPath = "Assets/Inworld";
        const string k_PlaygroundPath = "Inworld.Playground";
        const string k_ErrorTitle = "Install Playground Failed";
        //TODO(Yan): Replace to inworld-unity-core package once Playground is fully detached by full package.
        //           For the UserData Checking, let's add once EditorApplication starts playing.
        const string k_NoSDKDescription = "Inworld SDK is not found\nPlease download and install the InworldAI.Full.unitypackage first.\nThen select Inworld > Playground > Reinstall.";
        const string k_UpgradeContent = "Legacy Inworld SDK found.\nPlease delete the folder Assets/Inworld, and import the newest Inworld Unity SDK package\nThen select Inworld > Playground > Reinstall.";
        const string k_MissingPackage = "Missing playground unitypackage.\nPlease download the InworldPlayground.unitypackage and reimport.";
        const string k_PkgName = "InworldPlayground";
        const string k_AssetPkgName = "InworldPlaygroundAssets";
        const string k_DependencyPackage = "com.inworld.unity.core";
        
        static PlaygroundImporter()
        {
            AssetDatabase.importPackageCompleted += name =>
            {
                switch (name)
                {
                    case k_PkgName:
                        CheckDependencies();
                        break;
                    case k_AssetPkgName:
                        Debug.Log($"{k_PkgName} installed.");
                        break;
                }
            };
        }
        [MenuItem("Inworld/Playground/Reinstall")]
        public static async void CheckDependencies()
        {
            Debug.Log("Checking Playground Dependencies...");
            if (Directory.Exists($"Assets/{k_LegacyPkgName}") || Directory.Exists($"{k_InworldPath}/{k_LegacyPkgName}"))
            {
                EditorUtility.DisplayDialog(k_ErrorTitle, k_UpgradeContent, "OK");
                return;
            } 
            ListRequest listRequest = UnityEditor.PackageManager.Client.List();

            while (!listRequest.IsCompleted)
            {
                await Task.Yield();
            }
            if (listRequest.Status != StatusCode.Success)
            {
                Debug.LogError(listRequest.Error.ToString());
                return;
            }
            if (listRequest.Result.All(x => x.name != k_DependencyPackage))
            {
                EditorUtility.DisplayDialog(k_ErrorTitle, k_NoSDKDescription, "OK");
                return;
            }
            if (Directory.Exists($"{k_InworldPath}/{k_PlaygroundPath}/Runtime"))
                return;
            if (!File.Exists($"{k_InworldPath}/{k_PlaygroundPath}/{k_AssetPkgName}.unitypackage"))
            {
                EditorUtility.DisplayDialog(k_ErrorTitle, k_MissingPackage, "OK");
                return;
            }
            Debug.Log("Import Playground Dependency Packages...");
            AssetDatabase.ImportPackage($"{k_InworldPath}/{k_PlaygroundPath}/{k_AssetPkgName}.unitypackage", false);
        }
    }
}