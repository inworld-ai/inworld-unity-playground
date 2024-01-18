/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections;
using Inworld.Packet;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Inworld.Playground
{
    /// <summary>
    ///     Generates subtitles for Inworld characters.
    /// </summary>
    public class Subtitle : SingletonBehavior<Subtitle>
    {
        [SerializeField] private float m_SubtitleTimeFactor = 100;
        [SerializeField] private Color m_CharacterTextColor = Color.white;
        [SerializeField] private Color m_PlayerTextColor = Color.yellow;
        [SerializeField] private TMP_Text m_TextElement;

        private Coroutine m_FadeCoroutine;
        private Color m_OriginalBackgroundColor;
        
        /// <summary>
        ///     Resets and clears the current subtitle.
        /// </summary>
        public void Clear()
        {
            if (m_FadeCoroutine != null)
                StopCoroutine(m_FadeCoroutine);

            m_TextElement.text = "";
            m_TextElement.color = Color.clear;
        }

        private void OnEnable()
        {
            InworldController.Instance.OnCharacterInteraction += OnInteraction;
        }

        private void OnDisable()
        {
            if (InworldController.Instance)
                InworldController.Instance.OnCharacterInteraction -= OnInteraction;
        }

        private void OnInteraction(InworldPacket packet)
        {
            if (packet is not TextPacket textPacket)
                return;
            if (textPacket.text == null || 
                string.IsNullOrEmpty(textPacket.text.text) || 
                string.IsNullOrWhiteSpace(textPacket.text.text))
                return;
            
            if (m_FadeCoroutine != null)
                StopCoroutine(m_FadeCoroutine);

            string agentName = "";
            
            switch (packet.routing.source.type.ToUpper())
            {
                case "AGENT":
                    var character = InworldController.CharacterHandler.GetCharacterDataByID(packet.routing.source.name);
                    agentName = character.givenName;
                    break;
                case "PLAYER":
                    agentName = InworldAI.User.Name;
                    break;
                default:
                    return;
            }
            
            m_FadeCoroutine = StartCoroutine(ISetSubtitle(agentName, textPacket.text.text));
        }

        private IEnumerator ISetSubtitle(string name, string text)
        {
            m_TextElement.text = $"{name.FirstCharacterToUpper()}: {text}";
            Color textColor = name == InworldAI.User.Name ? m_PlayerTextColor : m_CharacterTextColor;
        
            m_TextElement.color = textColor;

            yield return new WaitForSeconds(m_SubtitleTimeFactor * (text.Length / 1000f));

            const float fadeTime = 1;
            float timer = 0;
            while (m_TextElement.color.a > 0)
            {
                timer += Time.deltaTime;
                float alpha = Mathf.Lerp(textColor.a, 0, timer / fadeTime);
                m_TextElement.color = new Color(m_TextElement.color.r, m_TextElement.color.g, m_TextElement.color.b, alpha);
                yield return new WaitForEndOfFrame();
            }
        
            m_FadeCoroutine = null;
            Clear();
        }
    }
}
