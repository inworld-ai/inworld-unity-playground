/*************************************************************************************************
 * Copyright 2022-2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/
#if UNITY_EDITOR
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
            if (!EditorUtility.DisplayDialog(k_NoDataTitle, k_NoDataDescription, "Create", "Cancel", DialogOptOutDecisionType.ForThisMachine, "DontClone")) 
                return;
            PlaygroundEditorPanel.Instance.ShowPanel();
            EditorApplication.isPlaying = false;
        };
        
        public void OnPreprocessBuild(BuildReport report)
        {
            if (InworldPlayground.IsDefaultGameData)
            {
                Debug.LogError(k_DefaultData);
            }
        }
        
        [MenuItem("Inworld/Playground Settings", false, 1)]
        static void TopMenuPlaygroundShowPanel() => PlaygroundEditorPanel.Instance.ShowPanel();
        
        [MenuItem("Assets/Inworld/Playground Settings", false, 1)]
        static void PlaygroundShowPanel() => PlaygroundEditorPanel.Instance.ShowPanel();
    }
}
#endif