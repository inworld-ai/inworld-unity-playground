/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using Inworld.Entities;
using Inworld.Packet;
using UnityEngine;

namespace Inworld.Playground
{
    /// <summary>
    ///     A Inworld character which will not process Emotion packets.
    /// </summary>
    public class InworldCharacterIgnoreEmotions : InworldPlaygroundCharacter
    {
        protected override void ProcessPacket(InworldPacket incomingPacket)
        {
            onPacketReceived.Invoke(incomingPacket);
            
            if(incomingPacket is not EmotionPacket)
                InworldController.Instance.CharacterInteract(incomingPacket);
            
            switch (incomingPacket)
            {
                case ActionPacket actionPacket:
                    HandleAction(actionPacket);
                    break;
                case AudioPacket audioPacket: // Already Played.
                    HandleLipSync(audioPacket);
                    break;
                case ControlPacket controlPacket: // Interaction_End
                    break;
                case TextPacket textPacket:
                    HandleText(textPacket);
                    break;
                case EmotionPacket emotionPacket:
                    HandleEmotion(emotionPacket);
                    break;
                case CustomPacket customPacket:
                    HandleTrigger(customPacket);
                    break;
                default:
                    InworldAI.LogError($"Received Unknown {incomingPacket}");
                    break;
            }
        }
    }
}
