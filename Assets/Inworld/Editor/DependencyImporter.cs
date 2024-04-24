/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/
#if UNITY_EDITOR
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;


namespace Inworld.Playground
{
    public static class DependencyImporter
    {
        static readonly string[] s_DependencyPackagesInstall = 
        {
            "com.unity.probuilder",
            "com.unity.ai.navigation",
            "com.unity.sentis",
            $"file:{Application.dataPath}/Inworld/com.inworld.unity.core-3.3.2.tgz"
        };
        
        static readonly string[] s_DependencyPackagesCheck = 
        {
            "com.unity.probuilder",
            "com.unity.ai.navigation",
            "com.inworld.unity.core"
#if UNITY_2022_3_OR_NEWER
            ,"com.unity.sentis"
#endif
        };
        
        public static async Task<bool> InstallDependencies()
        {
            Debug.Log("Importing Dependency Packages...");
            AddAndRemoveRequest request = Client.AddAndRemove(s_DependencyPackagesInstall);
            
            while (!request.IsCompleted)
            {
                EditorUtility.DisplayProgressBar("Inworld Playground", "Importing Dependencies...", 0.2f);
                await Task.Yield();
            }
            EditorUtility.ClearProgressBar();
            
            if (request.Status != StatusCode.Success)
            {
                Debug.LogError($"Failed to add dependency packages. {request.Error}.");
                return false;
            }
            Debug.Log($"Importing Dependencies Completed.");
            return true;
        }

        public static async Task<bool> CheckDependencies()
        {
            ListRequest request = Client.List();
            
            while (!request.IsCompleted)
            {
                EditorUtility.DisplayProgressBar("Inworld Playground", "Fetching packages...", 0.2f);
                await Task.Yield();
            }
            EditorUtility.ClearProgressBar();
            
            if (request.Status != StatusCode.Success)
            {
                Debug.LogError($"Failed to fetch packages. {request.Error}.");
                return false;
            }

            return !s_DependencyPackagesCheck.Any(dependency =>
                request.Result.All(x => x.name != dependency));
        }
    }
}
#endif
