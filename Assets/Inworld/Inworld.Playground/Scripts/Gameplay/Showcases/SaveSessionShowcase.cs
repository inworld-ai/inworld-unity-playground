/*************************************************************************************************
 * Copyright 2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inworld.Playground
{
    /// <summary>
    ///     Handles the Save Session showcase.
    /// </summary>
    public class SaveSessionShowcase : MonoBehaviour
    {
        [SerializeField] protected Button m_SaveButton;
        [SerializeField] protected Button m_RestartButton;
        [SerializeField] protected Button m_ClearButton;
        [SerializeField] protected TMP_Text m_SaveDataText;
        [SerializeField] protected GameObject m_InnequinCharacterPrefab;
        [SerializeField] protected InworldCharacter m_InworldCharacter;

        private void Awake()
        {
            m_ClearButton.interactable = false;
        }

        private void OnEnable()
        {
            InworldController.Client.OnStatusChanged += ClientOnStatusChanged;
            
            m_SaveButton.interactable = InworldController.Status == InworldConnectionStatus.Connected;
            m_RestartButton.interactable = InworldController.Status == InworldConnectionStatus.Connected;
        }


        private void OnDisable()
        {
            if(InworldController.Client)
                InworldController.Client.OnStatusChanged -= ClientOnStatusChanged;
        }

        public void OnRestartButtonPress()
        {
            StartCoroutine(RestartSession());
        }

        public void OnSaveButtonPress()
        {
            m_ClearButton.interactable = true;
            InworldController.Client.GetHistoryAsync(InworldController.Instance.CurrentScene);
            m_SaveDataText.text = "Conversation Saved at: " + DateTime.Now;
        }
        
        public void OnClearButtonPress()
        {
            m_ClearButton.interactable = false;
            InworldController.Client.SessionHistory = "";
            m_SaveDataText.text = "No Save Data";
        }

        private IEnumerator RestartSession()
        {
            InworldController.CharacterHandler.Unregister(m_InworldCharacter);
            InworldController.Instance.Disconnect();

            Transform characterTransform = m_InworldCharacter.transform;
            Vector3 characterPosition = characterTransform.position;
            Quaternion characterRotation = characterTransform.rotation;
            Transform parentTransform = characterTransform.parent;
            Destroy(m_InworldCharacter.gameObject);

            yield return new WaitWhile(() => InworldController.Status == InworldConnectionStatus.Connected);

            m_InworldCharacter = Instantiate(m_InnequinCharacterPrefab, characterPosition, characterRotation, parentTransform).GetComponent<InworldCharacter>();

            yield return null;
            
            InworldController.CharacterHandler.Register(m_InworldCharacter);
        }
        
        private void ClientOnStatusChanged(InworldConnectionStatus status)
        {
            m_SaveButton.interactable = status == InworldConnectionStatus.Connected;
            m_RestartButton.interactable = status == InworldConnectionStatus.Connected;
        }

    }
}
