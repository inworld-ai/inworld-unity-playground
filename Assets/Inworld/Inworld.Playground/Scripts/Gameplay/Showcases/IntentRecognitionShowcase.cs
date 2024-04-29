/*************************************************************************************************
 * Copyright 2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using TMPro;
using UnityEngine;

namespace Inworld.Playground
{
    /// <summary>
    ///     Handler for the Intent Recognition showcase.
    /// </summary>
    public class IntentRecognitionShowcase : MovementShowcase
    {
        [SerializeField]
        TMP_Text m_LeftSignText;
        [SerializeField]
        TMP_Text m_RightSignText;
        [SerializeField]
        Transform m_LeftDestinationTransform;
        [SerializeField]
        Transform m_RightDestinationTransform;

        void Awake()
        {
            SetSignDisabled(m_LeftSignText, "Left");
            SetSignDisabled(m_RightSignText, "Right");
        }

        protected override void OnInworldCharacterGoalCompleted(string brainName, string triggerName)
        {
            switch (triggerName)
            {
                case "greeting":
                    SetSignEnabled(m_LeftSignText, "Left");
                    SetSignEnabled(m_RightSignText, "Right");
                    break;
                case "left":
                    SetSignDisabled(m_LeftSignText, "Left");
                    SetSignEnabled(m_RightSignText, "Right");
                    MoveTo(m_LeftDestinationTransform.position);
                    break;
                case "right":
                    SetSignEnabled(m_LeftSignText, "Left");
                    SetSignDisabled(m_RightSignText, "Right");
                    MoveTo(m_RightDestinationTransform.position);
                    break;
            }
        }

        void SetSignEnabled(TMP_Text signText, string goalName)
        {
            signText.text = $"{goalName} Goal\nEnabled";
            signText.color = Color.green;
        }

        void SetSignDisabled(TMP_Text signText, string goalName)
        {
            signText.text = $"{goalName} Goal\nDisabled";
            signText.color = Color.red;
        }
    }
}

