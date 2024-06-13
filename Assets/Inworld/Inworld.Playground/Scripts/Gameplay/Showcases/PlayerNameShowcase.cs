/*************************************************************************************************
 * Copyright 2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using TMPro;
using UnityEngine;

namespace Inworld.Playground
{
    /// <summary>
    ///     Handler for the Update Player Name showcase.
    /// </summary>
    public class PlayerNameShowcase : InputShowcase
    {
        protected override void Awake()
        {
            base.Awake();
            m_Button.interactable = false;
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            InworldController.Client.OnStatusChanged += OnStatusChanged;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if(InworldController.Client)
                InworldController.Client.OnStatusChanged -= OnStatusChanged;
        }
        
        private void OnStatusChanged(InworldConnectionStatus status)
        {
            m_Button.interactable = status == InworldConnectionStatus.Connected;
        }

        public void SendPlayerNameChangeRequest()
        {
            if (PlaygroundManager.Instance.GetPlayerName() == m_InputField.text)
                return;
            PlaygroundManager.Instance.SetPlayerName(m_InputField.text);
            InworldController.Client.SendSessionConfig(false);
        }
    }
}
