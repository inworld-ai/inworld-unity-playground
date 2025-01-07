/*************************************************************************************************
 * Copyright 2022-2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

#if UNITY_EDITOR
using Inworld.Editors;
using UnityEditor;
using UnityEngine;


namespace Inworld.Playground
{
    // TODO(Yan): The Editor panel will mostly copy the code of InworldEditor for now.
    //            We will replace both SDK and playground with UIElement, and make them reusable.
    //            (Adding Serializable sequences in InworldEditor)
    
    // TODO(Yan): Auto check User login status when open. 
    //            If not logged in, set back to Init. Also apply to InworldEditor.
    public class PlaygroundEditorPanel : EditorWindow
    {
        /// <summary>
        ///     Get Instance of the Playground Editor Panel.
        ///     It'll create a Playground Editor Panel if the panel hasn't opened.
        /// </summary>
        public static PlaygroundEditorPanel Instance => GetWindow<PlaygroundEditorPanel>("Inworld Playground");
        /// <summary>
        ///     Open the Playground Editor Panel
        ///     It will detect and pop import window if you dont have TMP imported.
        /// </summary>
        public void ShowPanel()
        {
            titleContent = new GUIContent("Inworld Playground");
            Show();
        }
        void OnEnable()
        {
            PlaygroundEditor.Instance.CurrentState.OnOpenWindow();
        }
        void OnDisable()
        {
            PlaygroundEditor.Instance.CurrentState.OnClose();
        }
        void OnGUI()
        {
            _DrawBanner();
            if (!PlaygroundEditor.Instance)
                return;
            PlaygroundEditor.Instance.CurrentState.DrawTitle();
            PlaygroundEditor.Instance.CurrentState.DrawContent();
            PlaygroundEditor.Instance.CurrentState.DrawButtons();
        }
        
        void _DrawBanner()
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.fieldWidth = 1200f;
            Texture2D banner = InworldEditor.Banner;
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 32, 
                fontStyle = FontStyle.BoldAndItalic,
                alignment = TextAnchor.LowerLeft, 
                wordWrap = false
            };
            GUILayout.Label(new GUIContent(banner), GUILayout.Width(banner.width * 0.08f), GUILayout.Height(banner.height * 0.08f));
            GUILayout.Label("Playground", labelStyle, GUILayout.Height(banner.height * 0.09f)); // Slightly Lower.
            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif