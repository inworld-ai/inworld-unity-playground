/*************************************************************************************************
 * Copyright 2022-2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

#if UNITY_EDITOR
using System.Linq;
using Inworld.Editors;
using Inworld.Entities;
using UnityEditor;

namespace Inworld.Playground
{
    public class PlaygroundEditorLoadingGameData : IEditorState
    {
        InworldWorkspaceData m_PlaygroundData;
        public void OnOpenWindow()
        {
            
        }

        public void DrawTitle()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField($"Welcome, {InworldAI.User.Name}", InworldEditor.Instance.TitleStyle);
            EditorGUILayout.Space();
        }

        public void DrawContent()
        {
            if (m_PlaygroundData == null)
            {
                _DrawCloneWorkspace();
                return;
            }
        }

        InworldWorkspaceData _GetPlaygroundWorkspace() => InworldAI.User.Workspace.FirstOrDefault(ws =>
            ws.displayName.ToLower().Contains("inworld") &&
            ws.displayName.ToLower().Contains("playground"));

        void _DrawCloneWorkspace()
        {
            throw new System.NotImplementedException();
        }

        public void DrawButtons()
        {
            
        }

        public void OnExit()
        {
            
        }

        public void OnEnter()
        {
            m_PlaygroundData = _GetPlaygroundWorkspace();
            if (m_PlaygroundData != null)
            {
                _ListKeys();
            }
                
            _CreatePlaygroundGameData();
        }

        void _ListKeys()
        {
            throw new System.NotImplementedException();
        }

        void _CreatePlaygroundGameData()
        {
            throw new System.NotImplementedException();
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