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
            "Assets/Inworld/Inworld.NDK",
            "Assets/Inworld/Inworld.Samples.Innequin",
            "Assets/Inworld/Inworld.Samples.RPM",
            "Assets/Inworld/InworldUnitySDKManual.pdf",
            "Assets/Inworld/Inworld.Playground/Graphics",
            "Assets/Inworld/Inworld.Playground/Prefabs",
            "Assets/Inworld/Inworld.Playground/Resources",
            "Assets/Inworld/Inworld.Playground/Scenes",
            "Assets/Inworld/Inworld.Playground/Scripts"
        };
        
        static string[] s_CompletePackageAssetPathNames = 
        {
            "Assets/Inworld/Editor",
            "Assets/Inworld/InworldPlaygroundAssets.unitypackage"
        };
        
        [MenuItem("Inworld/Playground/Export", priority = 101)]
        static void ExportPlaygroundPackage()
        {
            AssetDatabase.ExportPackage(s_PlaygroundAssetPathNames, $"{Application.dataPath}/Inworld/InworldPlaygroundAssets.unitypackage", ExportPackageOptions.Recurse);
            AssetDatabase.ExportPackage(s_CompletePackageAssetPathNames, $"{Application.dataPath}/Exports/InworldAI.Playground.unitypackage", ExportPackageOptions.Recurse);
        }
    }
}
