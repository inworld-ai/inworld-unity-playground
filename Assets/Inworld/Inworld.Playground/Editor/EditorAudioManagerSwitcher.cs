/*************************************************************************************************
 * Copyright 2022-2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

#if UNITY_EDITOR
using Inworld.Audio;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Inworld.Playground
{
    [InitializeOnLoad]
    public class AudioManagerSwitcher
    {
        InworldAudioManager m_AudioManager;
        InworldAudioManager m_AudioManagerMobile;
        InworldAudioManager m_AudioManagerWebGL;

        const string k_AECPath = "Assets/Inworld/Inworld.Native/Prefabs/AECAudioManager.prefab";
        const string k_WebGLPath = "Assets/Inworld/Inworld.Assets/Prefabs/Audio/WebGLAudioManager.prefab";
        const string k_MobilePath = "Assets/Inworld/Inworld.Assets/Prefabs/Audio/TurnBasedAudioManager.prefab";
        
        static BuildTarget s_CurrentBuildTarget;
        static AudioManagerSwitcher()
        {
            s_CurrentBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            
            EditorApplication.update += CheckBuildTarget;
        }

        static void _SwitchAudioManagerInScene()
        {
            if (!InworldController.Instance)
                return;
            GameObject newPrefab = GetAudioManagerPrefab();
            if (newPrefab == null)
            {
                Debug.LogError($"Cannot get the AudioManager prefab.");
                return;
            }

            if (InworldController.Audio)
            {
                GameObject prevAudioManager = InworldController.Audio.gameObject;
                Object.DestroyImmediate(prevAudioManager);
            }
            PrefabUtility.InstantiatePrefab(newPrefab, InworldController.Instance.transform);
        }
        static void _SwitchAudioManagers()
        {
            Scene currScene = SceneManager.GetActiveScene();
            string[] data = AssetDatabase.FindAssets("t:SceneAsset", new[] { "Assets/Inworld/Inworld.Playground" });
            
            foreach (string str in data)
            {
                string scenePath = AssetDatabase.GUIDToAssetPath(str);
                Debug.Log($"Process on {scenePath}");
                Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
                _SwitchAudioManagerInScene();
                EditorSceneManager.MarkSceneDirty(scene);
                EditorSceneManager.SaveScene(scene);
            }
            EditorSceneManager.OpenScene(currScene.path, OpenSceneMode.Single);
        }
        static void CheckBuildTarget()
        {
            if (s_CurrentBuildTarget == EditorUserBuildSettings.activeBuildTarget || !InworldController.Instance) 
                return;
            Debug.Log($"Build platform changed from {s_CurrentBuildTarget} to {EditorUserBuildSettings.activeBuildTarget}");
            s_CurrentBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            _SwitchAudioManagers();
        }

        static GameObject GetAudioManagerPrefab()
        {
            switch (s_CurrentBuildTarget)
            {
                case BuildTarget.Android:
                case BuildTarget.iOS:
                    return AssetDatabase.LoadAssetAtPath<GameObject>(k_MobilePath);
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                case BuildTarget.StandaloneOSX:
                    return AssetDatabase.LoadAssetAtPath<GameObject>(k_AECPath);
                case BuildTarget.WebGL:
                    return AssetDatabase.LoadAssetAtPath<GameObject>(k_WebGLPath);
            }
            return null;
        }
    }
}
#endif