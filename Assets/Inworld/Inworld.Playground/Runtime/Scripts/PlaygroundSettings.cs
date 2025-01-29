/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using Newtonsoft.Json;

namespace Inworld.Playground
{
    [Serializable]
    public class AudioSettings
    {
        public string microphoneDevice;
        public MicSampleMode interactionMode;
        public bool enableAEC;
        public bool enableVAD;
    }
    [Serializable]
    public class PlaygroundSettings
    {
        public string playerName = "Player";
        public string workspaceName;
        public string sceneName;
        public bool enableGroupChat;
        public AudioSettings audioSettings;
    }
}