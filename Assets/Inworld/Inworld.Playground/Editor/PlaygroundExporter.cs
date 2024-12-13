/*************************************************************************************************
 * Copyright 2022-2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Inworld.Playground
{
    public static class PlaygroundExporter
    {
        const string k_FullPackagePath = "Assets/Inworld";
        const string k_ExportPath = "Inworld.Playground";
        static readonly string s_PlaygroundAssetPath = $"{k_FullPackagePath}/{k_ExportPath}/InworldPlaygroundAssets.unitypackage";
        static readonly string s_PlaygroundPackagePath = $"{k_FullPackagePath}/InworldPlayground.unitypackage";
        static readonly string s_PlayGroundScenePath = $"{k_FullPackagePath}/{k_ExportPath}/Scenes/Lobby.unity";
        
        [MenuItem("Inworld/Export Package/Playground/Playground Assets", false, 110)]
        public static void ExportPlaygroundAssets()
        {
            string[] assetPaths =
            {
                $"{k_FullPackagePath}/{k_ExportPath}/Runtime", 
            }; 
            AssetDatabase.ExportPackage(assetPaths, s_PlaygroundAssetPath, ExportPackageOptions.Recurse); 
        }
        
        [MenuItem("Inworld/Export Package/Playground/Playground Package", false, 111)]
        public static void ExportPlaygroundPackage()
        {
            string[] assetPaths =
            {
                $"{k_FullPackagePath}/{k_ExportPath}/Editor",
                s_PlaygroundAssetPath
            }; 
            AssetDatabase.ExportPackage(assetPaths, s_PlaygroundPackagePath, ExportPackageOptions.Recurse); 
        }
        
        public static void BuildPlaygroundScene()
        {
            string[] scenes = { s_PlayGroundScenePath };

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = $"{EditorUserBuildSettings.activeBuildTarget}/BuildTest", // YAN: As a build test, we don't care the extension name
                target = EditorUserBuildSettings.activeBuildTarget,
                options = BuildOptions.None
            };

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            switch (summary.result)
            {
                case BuildResult.Succeeded:
                    Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
                    EditorApplication.Exit(0);
                    break;
                case BuildResult.Failed:
                    Debug.LogError("Build failed");
                    EditorApplication.Exit(101);
                    break;
                case BuildResult.Cancelled:
                    Console.WriteLine("Build cancelled!");
                    EditorApplication.Exit(102);
                    break;
                case BuildResult.Unknown:
                default:
                    Console.WriteLine("Build result is unknown!");
                    EditorApplication.Exit(103);
                    break;
            }
        }
    }
}
