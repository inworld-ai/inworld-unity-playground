/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

#if UNITY_EDITOR
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Inworld.Playground
{
    [InitializeOnLoad]
    public class Preload : MonoBehaviour
    {
        static Preload()
        {
            AssetDatabase.importPackageCompleted += async packageName => {
                if (!packageName.StartsWith("InworldAI.Playground"))
                    return;
                BuildSettingsUpdater.UpdateBuildSettings();
                TMP_PackageResourceImporter.ImportResources(true, false, false);
                await DependencyImporter.InstallDependencies();
            };
        }
    }
}
#endif