/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
namespace Inworld.Playground
{
    /// <summary>
    ///     Settings for the Inworld Playground
    /// </summary>
    [Serializable]
    public class PlaygroundSettings
    {
        public const int WorkspaceVersion = 2;
        public string PlayerName = "Player";
        public string WorkspaceId;
        public string MicrophoneDevice;
        public MicrophoneMode InteractionMode;
        public bool EnableAEC = true;
    }
}
