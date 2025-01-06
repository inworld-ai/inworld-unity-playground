/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

#if UNITY_EDITOR
using System.Collections.Generic;
using Inworld.Editors;
using UnityEditor;
using UnityEngine;

namespace Inworld.Playground
{
    public class PlaygroundEditor : ScriptableObject
    {
        [SerializeField] EditorState m_CurrentState = EditorState.Init;
        EditorState m_LastState = EditorState.Init;

        const string k_InstancePath = "Assets/Inworld/Inworld.Playground/Runtime/Data/PlaygroundEditor.asset";
        const string k_CloneToken = "p9ZYx2gBsFHH";

        static PlaygroundEditor __inst;
                
        string m_ErrorMsg;
        string m_InputUserName;
        Dictionary<EditorState, IEditorState> m_PlaygroundEditorStates = new Dictionary<EditorState, IEditorState>();
        /// <summary>
        /// Gets an instance of PlaygroundEditor.
        /// By default, it is at `Assets/Inworld/Inworld.Playground/Runtime/Data/PlaygroundEditor.asset`.
        /// Please do not modify it.
        /// </summary>
        public static PlaygroundEditor Instance
        {
            get
            {
                if (__inst)
                    return __inst;
                __inst = AssetDatabase.LoadAssetAtPath<PlaygroundEditor>(k_InstancePath);
                return __inst;
            }
        }
        /// <summary>
        /// Get the clone token.
        /// </summary>
        public static string CloneToken => k_CloneToken;

        /// <summary>
        /// Gets/Sets the current state of Inworld Editor.
        /// </summary>
        public EditorState State
        {
            get => m_CurrentState;
            set
            {
                m_LastState = m_CurrentState;
                m_CurrentState = value;
                LastState?.OnExit();
                CurrentState?.OnEnter();
            }
        }
        /// <summary>
        /// Gets the last Editor State.
        /// </summary>
        public IEditorState LastState => m_PlaygroundEditorStates[m_LastState];
        /// <summary>
        /// Gets the current Editor State.
        /// </summary>
        public IEditorState CurrentState => m_PlaygroundEditorStates[m_CurrentState];

        /// <summary>
        /// Gets/Sets the current Error message.
        /// If setting, also set the current status of InworldEditor.
        /// </summary>
        public string Error
        {
            get => m_ErrorMsg;
            set
            {
                Debug.LogError(value);
                State = EditorState.Error;
                m_ErrorMsg = value;
            }
        }

        void OnEnable()
        {
            m_PlaygroundEditorStates[EditorState.Init] = new PlaygroundEditorInit();
            m_PlaygroundEditorStates[EditorState.LoadingGameData] = new PlaygroundEditorLoadingGameData();
            m_PlaygroundEditorStates[EditorState.GameConfig] = new PlaygroundEditorGameConfig();
            m_PlaygroundEditorStates[EditorState.Error] = new PlaygroundEditorError();
        }
    }
}
#endif