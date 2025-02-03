/*************************************************************************************************
 * Copyright 2022-2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *
 * Derivative of Inworld.Sample.StatusPanel
 *************************************************************************************************/

using Inworld.Sample;
using UnityEngine;
using UnityEngine.UI;

namespace Inworld.Playground
{
    /// <summary>
    ///     Handles the status panel for connection and scene changes.
    /// </summary>
    // TODO(Yan): Investigate Playground Status Panel.
    public class PlaygroundStatusPanel : StatusPanel
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            // PlaygroundManagerBak.Instance.OnPlay += OnEndSceneChange;
            // PlaygroundManagerBak.Instance.OnStartInworldSceneChange += OnStartInworldSceneChange;
            // PlaygroundManagerBak.Instance.OnEndInworldSceneChange += OnEndInworldSceneChange;
        }

        protected override void OnDisable()
        {
            // if (!InworldController.Instance || !PlaygroundManagerBak.Instance)
            //     return;
            // base.OnDisable();
            // PlaygroundManagerBak.Instance.OnPlay += OnEndSceneChange;
            // PlaygroundManagerBak.Instance.OnStartInworldSceneChange += OnStartInworldSceneChange;
            // PlaygroundManagerBak.Instance.OnEndInworldSceneChange += OnEndInworldSceneChange;
        }
        
        void OnEndSceneChange()
        {
            if (m_Board)
                m_Board.SetActive(false);
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
