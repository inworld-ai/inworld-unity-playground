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
using UnityEngine.SceneManagement;

namespace Inworld.Playground
{
    [InitializeOnLoad]
    public class PlaygroundEditorUtil : IPreprocessBuildWithReport
    {
        public int callbackOrder { get; }
        const string k_PlaygroundPath = "Inworld.Playground";
        const string k_NoDataTitle = "Loading Playground Data failed";
        const string k_NoDataDescription = "Cannot find User Data. Please login first.";

        static PlaygroundEditorUtil() => EditorApplication.playModeStateChanged += newMode =>
        {
            if (!SceneManager.GetActiveScene().path.Contains(k_PlaygroundPath) 
                || newMode != PlayModeStateChange.ExitingEditMode || InworldPlayground.GameData) 
                return;
            if (EditorUtility.DisplayDialog(k_NoDataTitle, k_NoDataDescription, "OK"))
                PlaygroundEditorPanel.Instance.ShowPanel();
            EditorApplication.isPlaying = false;
        };
        
        public void OnPreprocessBuild(BuildReport report)
        {
            if (InworldPlayground.GameData == null)
                PlaygroundEditorPanel.Instance.ShowPanel();
        }
    }
}
#endif