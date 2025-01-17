/*************************************************************************************************
 * Copyright 2022-2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/
#if UNITY_EDITOR
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Inworld.Playground
{
    [InitializeOnLoad]
    public class PlaygroundEditorUtil : IPreprocessBuildWithReport
    {
        public int callbackOrder { get; }
        const string k_PlaygroundPath = "Inworld.Playground";
        const string k_NoDataTitle = "Cannot find Playground data";
        const string k_DefaultData = "Cannot find Playground data. Will use the default one.";
        const string k_NoDataDescription = "Do you want to create your own Playground data, or use the default one?";

        static PlaygroundEditorUtil() => EditorApplication.playModeStateChanged += newMode =>
        {
            if (!SceneManager.GetActiveScene().path.Contains(k_PlaygroundPath))
                return;
            if (newMode != PlayModeStateChange.ExitingEditMode)
                return;
            if (!InworldPlayground.IsDefaultGameData)
                return;
            if (EditorPrefs.GetBool("InworldPlayground.DoNotClone"))
                return;
            if (!EditorUtility.DisplayDialog(k_NoDataTitle, k_NoDataDescription, "Clone", "Use Default"))
            {
                EditorPrefs.SetBool("InworldPlayground.DoNotClone", true); 
                return;
            }
            PlaygroundEditorPanel.Instance.ShowPanel();
            EditorApplication.isPlaying = false;
        };
        
        public void OnPreprocessBuild(BuildReport report)
        {
            if (InworldPlayground.IsDefaultGameData)
            {
                Debug.LogWarning(k_DefaultData);
            }
        }
        
        [MenuItem("Inworld/Playground Settings", false, 1)]
        static void TopMenuPlaygroundShowPanel() => PlaygroundEditorPanel.Instance.ShowPanel();
        
        [MenuItem("Assets/Inworld/Playground Settings", false, 1)]
        static void PlaygroundShowPanel() => PlaygroundEditorPanel.Instance.ShowPanel();
        
        [MenuItem("Inworld/Playground/CombineMesh", false, 1)]
        static void CombineMeshes()
        {
            // 获取所有子对象中的 MeshFilter
            MeshFilter[] meshFilters = Selection.activeGameObject.GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];

            for (int i = 0; i < meshFilters.Length; i++)
            {
                // 跳过没有网格的物体
                if (meshFilters[i].sharedMesh == null) continue;

                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].gameObject.SetActive(false); // 隐藏原始物体
            }

            // 创建新的合并网格
            MeshFilter combinedMeshFilter = Selection.activeGameObject.AddComponent<MeshFilter>();
            combinedMeshFilter.sharedMesh = new Mesh();
            combinedMeshFilter.sharedMesh.CombineMeshes(combine, true); // true 表示合并为一个 Submesh

            // 添加 MeshRenderer 并复用材质
            MeshRenderer meshRenderer = Selection.activeGameObject.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = meshFilters[0].GetComponent<MeshRenderer>().sharedMaterial;

            // 激活新网格
            Selection.activeGameObject.SetActive(true);
        }
    }
}
#endif