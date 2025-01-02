/*************************************************************************************************
 * Copyright 2022-2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;


namespace Inworld.Playground
{
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
    }
}
#endif