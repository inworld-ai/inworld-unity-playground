/*************************************************************************************************
 * Copyright 2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections;
using UnityEngine;

namespace Inworld.Playground
{
    /// <summary>
    ///     Handler for the Update Player Name showcase.
    /// </summary>
    public class PlayerNameShowcase : InputShowcase
    {
        private Coroutine m_UpdatePlayerNameCoroutine;
        
        public void SendPlayerNameChangeRequest()
        {
            if (PlaygroundManager.Instance.GetPlayerName() == m_InputField.text || m_UpdatePlayerNameCoroutine != null)
                return;

            m_UpdatePlayerNameCoroutine = StartCoroutine(UpdatePlayerNameEnumerator());
        }
        
        IEnumerator UpdatePlayerNameEnumerator()
        {
            if(InworldController.Status != InworldConnectionStatus.Connected)
                yield return PlaygroundManager.Instance.Connect();
            
            PlaygroundManager.Instance.SetPlayerName(m_InputField.text);
            InworldController.Client.SendSessionConfig(false);

            m_UpdatePlayerNameCoroutine = null;
        }
    }
}
