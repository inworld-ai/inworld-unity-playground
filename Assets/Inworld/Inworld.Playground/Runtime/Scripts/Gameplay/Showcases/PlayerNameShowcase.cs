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
        public void SendPlayerNameChangeRequest()
        {
            if(InworldController.Status != InworldConnectionStatus.Connected)
                return;
            InworldAI.User.Name = m_InputField.text;
            InworldController.Client.SendSessionConfig(false);
        }
    }
}
