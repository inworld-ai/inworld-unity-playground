/*************************************************************************************************
 * Copyright 2022-2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *
 * Derivative of Inworld.Sample.StatusPanel
 *************************************************************************************************/

using System.Collections;
using Inworld.Sample;
using UnityEngine;
using UnityEngine.UI;

namespace Inworld.Playground
{
    /// <summary>
    ///     Handles the status panel for connection and scene changes.
    /// </summary>
    public class PlaygroundStatusPanel : StatusPanel
    {
        [SerializeField] private Color m_StatusPanelBackgroundColor;
        [SerializeField] private Image m_StatusPanelBackground;
        private void Awake()
        {
            if (m_Board)
                m_Board.SetActive(true);
            if (m_Indicator)
                m_Indicator.text = "Loading";
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            PlaygroundManager.Instance.OnPlay.AddListener(OnEndSceneChange);
            PlaygroundManager.Instance.OnStartInworldSceneChange.AddListener(OnStartInworldSceneChange);
            PlaygroundManager.Instance.OnEndInworldSceneChange.AddListener(OnEndInworldSceneChange);
        }

        protected override void OnDisable()
        {
            if (!InworldController.Instance || !PlaygroundManager.Instance)
                return;
            base.OnDisable();
            PlaygroundManager.Instance.OnPlay.RemoveListener(OnEndSceneChange);
            PlaygroundManager.Instance.OnStartInworldSceneChange.RemoveListener(OnStartInworldSceneChange);
            PlaygroundManager.Instance.OnEndInworldSceneChange.RemoveListener(OnEndInworldSceneChange);
        }
        
        void OnEndSceneChange()
        {
            if (m_Board)
                m_Board.SetActive(false);
            m_StatusPanelBackground.color = m_StatusPanelBackgroundColor;
        }
        
        void OnStartInworldSceneChange()
        {
            if (m_Board)
                m_Board.SetActive(true);
            if (m_Indicator)
                m_Indicator.text = "Changing Inworld Scene";
        }
        
        void OnEndInworldSceneChange()
        {
            if (m_Board)
                m_Board.SetActive(false);
        }
    }
}
