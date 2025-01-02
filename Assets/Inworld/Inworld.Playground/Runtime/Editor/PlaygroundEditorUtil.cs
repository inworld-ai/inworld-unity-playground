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

namespace Inworld.Playground
{
    public class PlaygroundEditorUtil : IPreprocessBuildWithReport
    {
        public int callbackOrder { get; }
        
        public void OnPreprocessBuild(BuildReport report)
        {
            if (InworldPlayground.GameData == null)
                PlaygroundEditorPanel.Instance.ShowPanel();
        }
    }
}
#endif