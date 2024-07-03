/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inworld.Playground
{
    /// <summary>
    ///     Handles the Setup UI.
    /// </summary>
    public class SetupUI : MonoBehaviour
    {
        [SerializeField] private string m_WorkspaceCloningToken;
        [Header("Main")] 
        [SerializeField] private List<GameObject> pages;
        [SerializeField] private Button m_NextButton;
        [SerializeField] private Button m_BackButton;
        [SerializeField] private Button m_PlayButton;
        [Header("Page 0")]
        [SerializeField]
        private TMP_InputField m_NameInputField;
        [Header("Page 1")]
        [SerializeField]
        private Button m_WorkspaceCloningButton;
        [Header("Page 2")]
        [SerializeField]
        private TMP_InputField m_WorkspaceInputField;
        [Header("Page 3")]
        [SerializeField]
        private Button m_IntegrationsLinkButton;
        [SerializeField]
        private TMP_InputField m_AuthInputField;
        
        private int m_CurrentPage;
        private string m_InworldStudioIntegrationsURL;
        private string m_Key, m_Secret;
        private PlaygroundManager m_PlaygroundManager;

        #region UI Callback Functions
        public void CloneWorkspace()
        {
            if(Regex.IsMatch(m_WorkspaceCloningToken, "^[a-zA-Z0-9]+$"))
                Application.OpenURL($"https://studio.inworld.ai/workspaces?workspaceCloningToken={m_WorkspaceCloningToken}");
        }

        public void OpenIntegrationsTabHttp()
        {
            string workspaceId = m_PlaygroundManager.GetWorkspaceId();
            
            if(Regex.IsMatch(workspaceId,"^[a-z0-9-_]+$"))
                Application.OpenURL($"https://studio.inworld.ai/workspaces/{workspaceId}/integrations");
        }
        
        public void NameInputValueChanged()
        {
            m_PlaygroundManager.SetPlayerName(m_NameInputField.text);
            UpdateControlButtons();
        }

        public void WorkspaceInputValueChanged()
        {
            string workspaceId = m_WorkspaceInputField.text;
            if (workspaceId.StartsWith("workspaces/"))
                workspaceId = workspaceId.Substring(11);

            workspaceId = Regex.Replace(workspaceId, "[^a-z0-9-_]", "");
                
            m_PlaygroundManager.SetWorkspaceId(workspaceId);
            m_InworldStudioIntegrationsURL = $"https://studio.inworld.ai/workspaces/{workspaceId}/integrations";
            m_IntegrationsLinkButton.GetComponentInChildren<TMP_Text>().text = m_InworldStudioIntegrationsURL;
            UpdateControlButtons();
        }

        public void AuthInputValueChanged()
        {
            m_Key = "";
            m_Secret = "";
            string base64AuthToken = m_AuthInputField.text;
            try
            {
                var authBytes = Convert.FromBase64String(base64AuthToken);
                var authString = Encoding.ASCII.GetString(authBytes);
                m_Key = authString.Substring(0, authString.IndexOf(':'));
                m_Secret = authString.Substring(authString.IndexOf(':') + 1, authString.Length - (m_Key.Length + 1));
                
                m_Key = Regex.Replace(m_Key, "[^a-zA-Z0-9]", "");
                m_Secret = Regex.Replace(m_Secret, "[^a-zA-Z0-9]", "");
            }
            catch
            {
                // ignored
            }

            UpdateControlButtons();
        }
        
        public void Next()
        {
            if (m_CurrentPage == pages.Count - 1) return;
            
            if(m_CurrentPage == 0)
                m_BackButton.gameObject.SetActive(true);
            
            switch (m_CurrentPage)
            {
                case 0:
                    if (m_PlaygroundManager.GameData != null)
                    {
                        string workspaceId = m_PlaygroundManager.GameData.sceneFullName.Substring(m_PlaygroundManager.
                            GameData.sceneFullName.IndexOf('/') + 1);
                        workspaceId = workspaceId.Substring(0, workspaceId.IndexOf('/'));
                        m_PlaygroundManager.SetWorkspaceId(workspaceId);
                        
                        int vIndex = workspaceId.IndexOf('v');
                        if (vIndex == -1 || int.Parse(workspaceId.Substring(vIndex + 1)) != PlaygroundSettings.WorkspaceVersion)
                            break;
                        
                        m_WorkspaceInputField.text = "workspaces/" + m_PlaygroundManager.GetWorkspaceId();

                        m_Key = m_PlaygroundManager.GameData.apiKey;
                        m_Secret = m_PlaygroundManager.GameData.apiSecret;
                        m_AuthInputField.text = Convert.ToBase64String(Encoding.ASCII.GetBytes(m_Key + ":" + m_Secret));
                        
                        pages[m_CurrentPage].SetActive(false);
                        m_CurrentPage = 3;
                    }
                    break;
                case 3:
                    CreateGameData();
                    break;
            }
            
            pages[m_CurrentPage++].SetActive(false);
            pages[m_CurrentPage].SetActive(true);

            UpdateControlButtons();

            if (m_CurrentPage == pages.Count - 1)
            {
                m_NextButton.gameObject.SetActive(false);
                m_PlayButton.gameObject.SetActive(true);
            }
        }
        
        public void Back()
        {
            if (m_CurrentPage == 0) return;

            if (m_CurrentPage == pages.Count - 1)
            {
                m_NextButton.gameObject.SetActive(true);
                m_PlayButton.gameObject.SetActive(false);
                InworldController.Instance.Disconnect();
            }
            
            pages[m_CurrentPage--].SetActive(false);
            pages[m_CurrentPage].SetActive(true);
            
            UpdateControlButtons();
            
            if(m_CurrentPage == 0)
                m_BackButton.gameObject.SetActive(false);
        }
        #endregion 
        
        #region Unity Event Functions
        private void Awake()
        {
            m_PlaygroundManager = PlaygroundManager.Instance;

            TMP_Text workspaceCloningButtonText = m_WorkspaceCloningButton.GetComponentInChildren<TMP_Text>();
            if (workspaceCloningButtonText)
                workspaceCloningButtonText.text = $"https://studio.inworld.ai/workspaces?workspaceCloningToken={m_WorkspaceCloningToken}";
            
            foreach (var page in pages)
                page.SetActive(false);
            pages[m_CurrentPage].SetActive(true);
            m_BackButton.gameObject.SetActive(false);
            m_PlayButton.gameObject.SetActive(false);
            
            m_PlaygroundManager.SetPlayerName("");
            UpdateControlButtons();
        }

        private void OnEnable()
        {
            m_IntegrationsLinkButton.onClick.AddListener(OnIntegrationsLinkClick);
        }

        private void OnDisable()
        {
            m_IntegrationsLinkButton.onClick.RemoveListener(OnIntegrationsLinkClick);
        }
        #endregion
        
        private void CreateGameData()
        {
            if (string.IsNullOrEmpty(m_Key) || string.IsNullOrEmpty(m_Secret)) return;
            
            m_PlaygroundManager.CreateGameData(m_Key, m_Secret);
        }

        private void UpdateControlButtons()
        {
            switch (m_CurrentPage)
            {
                case 0:
                    m_NextButton.interactable = !string.IsNullOrEmpty(m_PlaygroundManager.GetPlayerName());
                    break;
                case 2:
                    m_NextButton.interactable = !string.IsNullOrEmpty(m_PlaygroundManager.GetWorkspaceId());
                    break;
                case 3:
                    m_NextButton.interactable = !string.IsNullOrEmpty(m_Key) && !string.IsNullOrEmpty(m_Secret);
                    break;
                default:
                    m_NextButton.interactable = true;
                    break;
            }
        }
        
        private void OnIntegrationsLinkClick()
        {
            Application.OpenURL(m_InworldStudioIntegrationsURL);
        }
    }
}

