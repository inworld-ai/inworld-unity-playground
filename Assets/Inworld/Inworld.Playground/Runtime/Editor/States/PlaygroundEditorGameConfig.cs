/*************************************************************************************************
 * Copyright 2022-2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

#if UNITY_EDITOR
using Inworld.Editors;
using UnityEditor;

namespace Inworld.Playground
{
    public class PlaygroundEditorGameConfig : IEditorState
    {
        public void OnOpenWindow()
        {
            
        }

        public void DrawTitle()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField($"Now you're all set!", InworldEditor.Instance.TitleStyle);
            EditorGUILayout.Space();
        }

        public void DrawContent()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField($"You can close the panel now.");
            EditorGUILayout.Space();
        }

        public void DrawButtons()
        {
            
        }

        public void OnExit()
        {
            
        }

        public void OnEnter()
        {
            
        }

        public void PostUpdate()
        {
            
        }

        public void OnClose()
        {
            
        }
    }
}
#endif