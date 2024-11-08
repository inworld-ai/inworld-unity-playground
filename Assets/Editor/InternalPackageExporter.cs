/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/
using UnityEditor;
using Application = UnityEngine.Application;
namespace Inworld.Playground
{
    public static class InternalPackageExporter
    {
        static string[] s_PlaygroundAssetPathNames = 
        {
            "Assets/Inworld/Inworld.Assets",
            "Assets/Inworld/Inworld.Editor",
            "Assets/Inworld/Inworld.Native",
            "Assets/Inworld/Inworld.Samples.Innequin",
            "Assets/Inworld/Inworld.Samples.RPM",
            "Assets/Inworld/InworldUnitySDKManual.pdf",
            "Assets/Inworld/Inworld.Playground/Graphics",
            "Assets/Inworld/Inworld.Playground/Prefabs",
            "Assets/Inworld/Inworld.Playground/Resources",
            "Assets/Inworld/Inworld.Playground/Scenes",
            "Assets/Inworld/Inworld.Playground/Scripts",
            "Assets/Inworld/Inworld.Playground/Settings",
        };
        
        static string[] s_CompletePackageAssetPathNames = 
        {
            "Assets/Inworld/Inworld.Playground/Scripts/Editor",
            "Assets/Inworld/InworldPlaygroundAssets.unitypackage",
            "Assets/Inworld/com.inworld.unity.core-3.6.0.tgz"
        };
        
        [MenuItem("Inworld/Playground/Export Assets", priority = 101)]
        static void ExportPlaygroundAssetsPackage()
        {
            AssetDatabase.ExportPackage(s_PlaygroundAssetPathNames, $"{Application.dataPath}/Inworld/InworldPlaygroundAssets.unitypackage", ExportPackageOptions.Recurse);
        }
        
        [MenuItem("Inworld/Playground/Export Complete Package", priority = 101)]
        static void ExportPlaygroundCompletePackage()
        {
            AssetDatabase.ExportPackage(s_CompletePackageAssetPathNames, $"{Application.dataPath}/InworldAI.Playground.unitypackage", ExportPackageOptions.Recurse);
        }
    }
}
