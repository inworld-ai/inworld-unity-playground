/*************************************************************************************************
 * Copyright 2022-2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Inworld.Editors;
using Inworld.Entities;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;


namespace Inworld.Playground
{
    public class PlaygroundEditorLoadingGameData : IEditorState
    {
        const string k_CloneWorkspace = "No workspace is detected.\nPlease follow the instruction to clone and click \"Refresh\" to continue.";
        const string k_APIKeyMissing = "API key/secret is missing.\nPlease follow the instruction below to create one key/secret in your workspace.\nAnd click \"Refresh\" after worlds";
        const string k_Completed = "Game Data found.\nPlease click \"Next\" to continue.";
        InworldWorkspaceData m_PlaygroundWorkspaceData;
        InworldKeySecret m_PlaygroundKeySecret;
        Texture2D m_CurrentTexture;

        bool m_KeySecretMissing;
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
            if (m_PlaygroundWorkspaceData == null)
            {
                _DrawCloneWorkspace();
                return;
            }
            if (m_KeySecretMissing)
                _DrawFetchAPIKeySecrets();
            else
            {
                EditorGUILayout.LabelField(k_Completed, InworldEditor.Instance.TitleStyle);
            }
        }
        void _GetPlaygroundWorkspace()
        {
            m_KeySecretMissing = false;
            m_PlaygroundWorkspaceData = InworldAI.User.Workspace.FirstOrDefault(ws =>
                ws.displayName.ToLower().Contains("inworld") &&
                ws.displayName.ToLower().Contains("playground"));
            if (m_PlaygroundWorkspaceData != null)
            {
                _ListKeys();
            }
        }

        void _DrawCloneWorkspace()
        {
            if (m_CurrentTexture != PlaygroundEditor.Instance.InstructionCloneWS)
            {
                _CreateInstructionButton(PlaygroundEditor.Instance.InstructionCloneWS, k_CloneWorkspace, PlaygroundEditor.CloneToken);
            }
        }
        void _DrawFetchAPIKeySecrets()
        {
            if (m_CurrentTexture != PlaygroundEditor.Instance.InstructionAPIKey)
            {
                _CreateInstructionButton(PlaygroundEditor.Instance.InstructionAPIKey, k_APIKeyMissing, PlaygroundEditor.Instance.GetInstructionAPIKeyURL(m_PlaygroundWorkspaceData.name));
            }
        }

        void _CreateInstructionButton(Texture2D instruction, string text, string url)
        {
            GUIStyle guiStyle = new GUIStyle(GUI.skin.button)
            {
                fixedHeight = 600,
                fixedWidth = 600,
                margin = new RectOffset(10, 10, 10, 10),
                alignment = TextAnchor.LowerCenter,
                fontStyle = FontStyle.Bold,
                normal = new GUIStyleState
                {
                    background = instruction
                }
            };
            m_CurrentTexture = instruction;
            EditorGUILayout.LabelField(text, InworldEditor.Instance.TitleStyle);
            if (GUILayout.Button(text, guiStyle))
                Help.BrowseURL(url);
        }

        public void DrawButtons()
        {
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Back", InworldEditor.Instance.BtnStyle))
            {
                InworldEditor.Instance.Status = EditorStatus.Init;
            }
            if (GUILayout.Button("Refresh", InworldEditor.Instance.BtnStyle))
            {
                _GetPlaygroundWorkspace();
            }
            if (m_PlaygroundKeySecret != null && !string.IsNullOrEmpty(m_PlaygroundKeySecret.key) && !string.IsNullOrEmpty(m_PlaygroundKeySecret.secret))
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Next", InworldEditor.Instance.BtnStyle))
                {
                    _CreatePlaygroundGameData();
                    PlaygroundEditor.Instance.State = EditorState.GameConfig;
                }
            }
            GUILayout.EndHorizontal();
        }

        public void OnExit()
        {
            
        }

        public void OnEnter()
        {
            _GetPlaygroundWorkspace();
        }

        void _ListKeys()
        {
            string wsFullName = InworldAI.User.GetWorkspaceFullName(m_PlaygroundWorkspaceData.displayName);
            if (string.IsNullOrEmpty(wsFullName))
                return;
            InworldEditorUtil.SendWebGetRequest(InworldEditor.ListKeyURL(wsFullName), true, _ListKeyCompleted);
        }

        void _CreatePlaygroundGameData()
        {
            // Create a new SO.
            InworldGameData gameData = ScriptableObject.CreateInstance<InworldGameData>();
            InworldWorkspaceData ws = m_PlaygroundWorkspaceData;
            if (ws != null)
            {
                gameData.Init(ws.name, ws.keySecrets[0]);
            }
            gameData.capabilities = new Capabilities(InworldAI.Capabilities);
            if (string.IsNullOrEmpty(InworldEditorUtil.UserDataPath))
            {
                InworldEditor.Instance.Error = "Failed to save game data: Current User Setting is null.";
                return;
            }
            if (!Directory.Exists($"{InworldEditorUtil.UserDataPath}/{PlaygroundEditor.Instance.DataPath}"))
            {
                Directory.CreateDirectory($"{InworldEditorUtil.UserDataPath}/{PlaygroundEditor.Instance.DataPath}");
            }
            string newAssetPath = $"{InworldEditorUtil.UserDataPath}/{PlaygroundEditor.Instance.DataPath}/{InworldAI.User.Name}'s Playground.asset";
            AssetDatabase.CreateAsset(gameData, newAssetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            InworldPlayground.GameData = gameData;
        }
        
        void _ListKeyCompleted(AsyncOperation obj)
        {
            UnityWebRequest uwr = InworldEditorUtil.GetResponse(obj);
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                InworldEditor.Instance.Error = $"List Key Failed: {InworldEditor.GetError(uwr.error)}";
                EditorUtility.ClearProgressBar();
                return;
            }
            ListKeyResponse resp = JsonUtility.FromJson<ListKeyResponse>(uwr.downloadHandler.text);
            if (resp.apiKeys.Count == 0)
                m_KeySecretMissing = true;
            if (m_PlaygroundWorkspaceData.keySecrets == null)
                m_PlaygroundWorkspaceData.keySecrets = new List<InworldKeySecret>();
            m_PlaygroundWorkspaceData.keySecrets.Clear();
            m_PlaygroundWorkspaceData.keySecrets.AddRange(resp.apiKeys); 
            m_PlaygroundKeySecret = resp.apiKeys[0];
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