/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using Inworld.Entities;
using Inworld.Interactions;
using Inworld.Packet;
using Inworld.Sample;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Inworld.Playground
{
    /// <summary>
    ///     Manages the Playground: changing scenes, updating settings, play/pausing.
    /// </summary>
    public class PlaygroundManager : SingletonBehavior<PlaygroundManager>
    {
        public UnityEvent OnClientChanged;
        public UnityEvent OnPlay;
        public UnityEvent OnPause;
        public UnityEvent OnStartSceneChange;
        public UnityEvent OnEndSceneChange;
        public UnityEvent OnStartInworldSceneChange;
        public UnityEvent OnEndInworldSceneChange;
        
        public InworldGameData GameData => m_GameData;
        public bool Paused => m_Paused;
        public Scene CurrentScene => m_CurrentScene;
        
        public const string LobbySceneName = "Lobby";
        public const string SetupSceneName = "Setup";

        [SerializeField]
        private List<InworldSceneMapping> m_InworldSceneMapping;
        
        [SerializeField]
        private PlaygroundSettings m_Settings;

        [SerializeField] 
        private GameObject m_GameMenu;

        private Dictionary<string, string> m_InworldSceneMappingDictionary;
        private InworldGameData m_GameData;
        private EventSystem m_EventSystem;
        private Coroutine m_SceneChangeCoroutine;
        private Scene m_CurrentScene;
        private string m_CurrentInworldScene;
        private bool m_Paused;
        
        /// <summary>
        ///     Loads the current Game Data object into the Inworld Controller for the Playground Workspace.
        /// </summary>
        public void LoadData()
        {
            if(m_InworldSceneMappingDictionary.TryGetValue(SceneManager.GetActiveScene().name, out string inworldSceneName))
                m_GameData.sceneFullName = $"workspaces/{m_Settings.WorkspaceId}/scenes/{inworldSceneName}";
            InworldController.Instance.LoadData(m_GameData);
        }
        
        /// <summary>
        ///     Changes the current Inworld Scene to the one specified by sceneName.
        /// </summary>
        /// <param name="sceneName">The name of the Inworld Scene to load.</param>
        public void ChangeInworldScene(string sceneName)
        {
            StartCoroutine(ChangeInworldSceneEnumerator(sceneName));
        }
        
        /// <summary>
        ///     Creates a new Game Data object using the provided key and secret.
        /// </summary>
        /// <param name="key">The API key for the Playground Workspace.</param>
        /// <param name="secret">The API secret for the Playground Workspace.</param>
        public void CreateGameData(string key, string secret)
        {
            m_GameData = Serialization.CreateGameData(key, secret, m_Settings.WorkspaceId);
        }
        
        /// <summary>
        ///     Changes the current scene in Unity.
        /// </summary>
        /// <param name="sceneName">The name of the scene to load.</param>
        public void ChangeScene(string sceneName)
        {
            if(m_SceneChangeCoroutine == null)
                m_SceneChangeCoroutine = StartCoroutine(ChangeSceneEnumerator(sceneName));
        }
        
        /// <summary>
        ///     Initialize and start the Playground demo.
        /// </summary>
        public void Play()
        {
            if (m_CurrentScene.name == SetupSceneName)
            {
                SceneManager.LoadScene(LobbySceneName);
                return;
            }
            
            m_GameMenu.SetActive(false);
            
            if (!CheckNetworkComponent())
                throw new MissingComponentException("Missing Inworld client.");
            
            if (!CheckAudioComponent())
                throw new MissingComponentException("Missing PlaygroundAECAudioCapture component.");
            
            CursorHandler.LockCursor();

            // Time.timeScale = 1;
            
            InworldController.Audio.enabled = true;
            InworldController.Audio.ChangeInputDevice(m_Settings.MicrophoneDevice);

            PlaygroundAECAudioCapture audioCapture = (PlaygroundAECAudioCapture)InworldController.Audio;
            switch (m_Settings.InteractionMode)
            {
                case MicrophoneMode.Interactive:
                    audioCapture.UpdateDefaultSampleMode(MicSampleMode.AEC);
                    break;
                case MicrophoneMode.PushToTalk:
                    audioCapture.UpdateDefaultSampleMode(MicSampleMode.PUSH_TO_TALK);
                    break;
                case MicrophoneMode.TurnByTurn:
                    audioCapture.UpdateDefaultSampleMode(MicSampleMode.TURN_BASED);
                    break;
            }

            ResumeAllCharacterInteractions();
            
            EnableAllWorldSpaceGraphicRaycasters();

            m_Paused = false;
            OnPlay?.Invoke();
        }
        
        /// <summary>
        ///     Enables all WorldSpaceGraphicRaycasters in the scene.
        /// </summary>
        public void EnableAllWorldSpaceGraphicRaycasters()
        {
#if UNITY_2022_3_OR_NEWER
            var worldSpaceGraphicRaycasters = FindObjectsByType<WorldSpaceGraphicRaycaster>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
#else
            var worldSpaceGraphicRaycasters = FindObjectsOfType<WorldSpaceGraphicRaycaster>();
#endif
            foreach (var worldSpaceGraphicRaycaster in worldSpaceGraphicRaycasters)
            {
                worldSpaceGraphicRaycaster.enabled = true;
            }
        }
        
        /// <summary>
        ///     Disables all WorldSpaceGraphicRaycasters in the scene.
        /// </summary>
        public void DisableAllWorldSpaceGraphicRaycasters()
        {
#if UNITY_2022_3_OR_NEWER
            var worldSpaceGraphicRaycasters = FindObjectsByType<WorldSpaceGraphicRaycaster>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
#else
            var worldSpaceGraphicRaycasters = FindObjectsOfType<WorldSpaceGraphicRaycaster>();
#endif
            foreach (var worldSpaceGraphicRaycaster in worldSpaceGraphicRaycasters)
            {
                worldSpaceGraphicRaycaster.enabled = false;
            }
        }
        
        #region Settings Getters/Setters

        public string GetPlayerName()
        {
            return m_Settings.PlayerName;
        }
        
        public string GetWorkspaceId()
        {
            return m_Settings.WorkspaceId;
        }
        
        public string GetMicrophoneDevice()
        {
            return m_Settings.MicrophoneDevice;
        }
        
        public MicrophoneMode GetInteractionMode()
        {
            return m_Settings.InteractionMode;
        }
        
        public bool GetEnableAEC()
        {
            return m_Settings.EnableAEC;
        }

        public void SetPlayerName(string playerName)
        {
            m_Settings.PlayerName = playerName;
            InworldAI.User.Name = m_Settings.PlayerName;
        }

        public void SetWorkspaceId(string workspaceId)
        {
            m_Settings.WorkspaceId = workspaceId;
        }

        public void SetMicrophoneDevice(string deviceName)
        {
            m_Settings.MicrophoneDevice = deviceName;
        }
        
        public void UpdateMicrophoneMode(MicrophoneMode microphoneMode)
        {
            m_Settings.InteractionMode = microphoneMode;
        }

        public void UpdateEnableAEC(bool enableAEC)
        {
            m_Settings.EnableAEC = enableAEC;
        }
        #endregion
        
        #region Unity Event Functions
        private void Awake()
        {
            m_EventSystem = GetComponentInChildren<EventSystem>();
            if (Instance != this)
            {
                gameObject.SetActive(false);
                Destroy(gameObject);
                InworldAI.Log("Destroying duplicate instance of PlaygroundManager.");
                return;
            }
            
            DontDestroyOnLoad(gameObject);
            
            m_InworldSceneMappingDictionary = new Dictionary<string, string>();
            foreach (var sceneMapping in m_InworldSceneMapping)
                m_InworldSceneMappingDictionary.Add(sceneMapping.UnitySceneName, sceneMapping.InworldSceneName);
            
            m_GameData = Serialization.GetGameData();
            if (m_GameData == null && SceneManager.GetActiveScene().name != SetupSceneName)
            {
                InworldAI.Log("The Playground GameData could not be found, switching to Setup scene.");
                SceneManager.LoadScene(SetupSceneName);
                return;
            }

            if (m_GameData != null)
            {
                string workspaceId = m_GameData.sceneFullName.Split('/')[1];

                int vIndex = workspaceId.IndexOf('v');
                if (vIndex == -1 || int.Parse(workspaceId.Substring(vIndex + 1)) != PlaygroundSettings.WorkspaceVersion)
                {
                    InworldAI.LogWarning($"The Playground workspace is outdated, switching to Setup scene. Please follow the setup process to clone the latest Playground workspace: inworld-playground-v{PlaygroundSettings.WorkspaceVersion}");
                    SceneManager.LoadScene(SetupSceneName);
                    return;
                }
                m_Settings.WorkspaceId = workspaceId;
            }

            if (string.IsNullOrEmpty(m_Settings.PlayerName))
                SetPlayerName("Player");

            m_Paused = true;
        }

        private void Start()
        {
            m_EventSystem.enabled = true;
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            InworldController.Client.OnStatusChanged += OnStatusChanged;
            InworldController.Client.OnPacketReceived += OnPacketReceived;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            if (InworldController.Client)
            {
                InworldController.Client.OnStatusChanged -= OnStatusChanged;
                InworldController.Client.OnPacketReceived -= OnPacketReceived;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if(!m_GameMenu.activeSelf)
                    Pause();
            }
            
            if((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.F4))
                Application.Quit();
        }
        #endregion
        
        #region Event Callback Functions
        private void OnStatusChanged(InworldConnectionStatus status)
        {
            Debug.Log("Status Changed: " + status);
            if (status == InworldConnectionStatus.Initialized)
            {
                InworldController.Client.StartSession();
            }
        }
        
        private void OnPacketReceived(InworldPacket incomingPacket)
        {
            if (incomingPacket is ControlPacket controlPacket &&
                controlPacket.control is CurrentSceneStatusEvent currentSceneStatusEvent)
            {
                InworldAI.Log("Inworld Scene Changed: " + currentSceneStatusEvent.currentSceneStatus.sceneName);
                m_CurrentInworldScene = currentSceneStatusEvent.currentSceneStatus.sceneName;
            }
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            m_CurrentScene = scene;
            if (m_CurrentScene.name != SetupSceneName && m_GameData != null)
                StartCoroutine(SetupScene());
        }
        #endregion
        
        #region Enumerators
        private IEnumerator ChangeInworldSceneEnumerator(string sceneName, bool pause = true)
        {
            string sceneToLoad = $"workspaces/{m_Settings.WorkspaceId}/scenes/{sceneName}";
            if(m_CurrentInworldScene == sceneToLoad)
                yield break;
            
            OnStartInworldSceneChange?.Invoke();
            if(pause)
                Pause(false);
            var currentCharacter = InworldController.CharacterHandler.CurrentCharacter;
            InworldController.CharacterHandler.CurrentCharacter = null;
            
            string currentInworldScene = m_CurrentInworldScene;
            InworldController.Client.LoadScene(sceneToLoad);
            
            yield return new WaitUntil(() => currentInworldScene != m_CurrentInworldScene || InworldController.Client.Status != InworldConnectionStatus.Connected);
            
            if(currentCharacter)
                InworldController.CurrentCharacter = currentCharacter;
            
            if(pause)
                Play();
            OnEndInworldSceneChange?.Invoke();
        }
        
        private IEnumerator ChangeSceneEnumerator(string sceneName)
        {
            OnStartSceneChange?.Invoke();
            Pause(false);
            
            InworldAI.Log("Starting scene load: " + sceneName);
            var sceneLoadOperation = SceneManager.LoadSceneAsync(sceneName);
            sceneLoadOperation.allowSceneActivation = false;
            
            yield return new WaitUntil(() => sceneLoadOperation.progress >= 0.9f);
            sceneLoadOperation.allowSceneActivation = true;

            yield return new WaitUntil(() => sceneLoadOperation.isDone);
            InworldAI.Log("Completed scene load: " + sceneName);
            
            m_SceneChangeCoroutine = null;
            OnEndSceneChange?.Invoke();
        }
        
        private IEnumerator SetupScene()
        {
            LoadData();
            SetCharacterBrains();

            if (!m_InworldSceneMappingDictionary.TryGetValue(SceneManager.GetActiveScene().name, out string inworldSceneName))
                InworldAI.LogException("Missing scene in InworldSceneMappingDictionary: " + SceneManager.GetActiveScene().name);
            
            if (InworldController.Status != InworldConnectionStatus.Connected)
                InworldController.Client.CurrentScene = $"workspaces/{m_Settings.WorkspaceId}/scenes/{inworldSceneName}";
            else
                yield return StartCoroutine(ChangeInworldSceneEnumerator(inworldSceneName, false));

            Play();
        }
        #endregion
        
        private void Pause(bool showGameMenu = true)
        {
            if(m_Paused)
                return;
            
            DisableAllWorldSpaceGraphicRaycasters();

            PauseAllCharacterInteractions();

            InworldController.Audio.enabled = false;
            
            Subtitle.Instance.Clear();

            if (showGameMenu)
            {
                CursorHandler.UnlockCursor();
                m_GameMenu.SetActive(true);
            }
            
            m_Paused = true;
            OnPause?.Invoke();
        }
        
        private bool CheckAudioComponent()
        {
            return InworldController.Instance.GetComponent<PlaygroundAECAudioCapture>();
        }

        private bool CheckNetworkComponent()
        {
            return InworldController.Instance.GetComponent<InworldClient>();
        }
        
        private void PauseAllCharacterInteractions()
        {
#if UNITY_2022_3_OR_NEWER
            var interactions = FindObjectsByType<InworldInteraction>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
#else
            var interactions = FindObjectsOfType<InworldInteraction>();
#endif
            foreach (var interaction in interactions)
            {
                interaction.CancelResponse();
                interaction.enabled = false;
                if (interaction is InworldAudioInteraction)
                {
                    interaction.GetComponent<AudioSource>().Stop();
                }
            }
        }

        private void ResumeAllCharacterInteractions()
        {
#if UNITY_2022_3_OR_NEWER
            var interactions = FindObjectsByType<InworldInteraction>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
#else
            var interactions = FindObjectsOfType<InworldInteraction>();
#endif
            foreach (var interaction in interactions)
            {
                interaction.enabled = true;
            }
        }

        private void SetCharacterBrains()
        {
#if UNITY_2022_3_OR_NEWER
            var characters = FindObjectsByType<InworldCharacter>(FindObjectsInactive.Include, FindObjectsSortMode.None);
#else
            var characters = FindObjectsOfType<InworldCharacter>(true);
#endif
            foreach (var character in characters)
                character.Data.brainName = $"workspaces/{m_Settings.WorkspaceId}/characters/{character.Data.brainName.Split('/')[3]}";
        }
        
        private void RegisterCharacters()
        {
#if UNITY_2022_3_OR_NEWER
            var characters = FindObjectsByType<InworldCharacter>(FindObjectsInactive.Include, FindObjectsSortMode.None);
#else
            var characters = FindObjectsOfType<InworldCharacter>(true);
#endif
            foreach (var character in characters)
            {
                InworldController.CharacterHandler.Register(character);
            }
        }
    }
}
