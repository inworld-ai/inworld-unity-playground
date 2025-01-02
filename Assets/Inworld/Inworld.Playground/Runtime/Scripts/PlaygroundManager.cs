/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using Inworld.Packet;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Inworld.Playground
{
    /// <summary>
    ///     Manages the Playground: changing scenes, updating settings, play/pausing.
    /// </summary>
    public class PlaygroundManager : SingletonBehavior<PlaygroundManager>
    {
        [SerializeField] List<InworldSceneMapping> m_InworldSceneMapping;
        [SerializeField] PlaygroundSettings m_Settings;

        string m_CurrentInworldScene;
        Scene m_CurrentScene;
        EventSystem m_EventSystem;

        Dictionary<string, string> m_InworldSceneMappingDictionary;
        Coroutine m_SceneChangeCoroutine;
        
        public bool Paused { get; private set; }

        public Scene CurrentScene => m_CurrentScene;

        /// <summary>
        ///     Loads the current Game Data object into the Inworld Controller for the Playground Workspace.
        /// </summary>
        public void LoadData()
        {
            if (m_InworldSceneMappingDictionary.TryGetValue(SceneManager.GetActiveScene().name,
                    out string inworldSceneName))
                InworldPlayground.GameData.sceneFullName = $"workspaces/{m_Settings.WorkspaceId}/scenes/{inworldSceneName}";
            InworldController.Instance.LoadData(InworldPlayground.GameData);
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
            //TODO(Yan): Put CreateGameData to EditorUtil to avoid building error.
            //           Also get rid of Serialization class.
            InworldPlayground.GameData = Serialization.CreateGameData(key, secret, m_Settings.WorkspaceId);
        }

        /// <summary>
        ///     Changes the current scene in Unity.
        /// </summary>
        /// <param name="sceneName">The name of the scene to load.</param>
        public void ChangeScene(string sceneName)
        {
            if (m_SceneChangeCoroutine == null)
                m_SceneChangeCoroutine = StartCoroutine(ChangeSceneEnumerator(sceneName));
        }

        /// <summary>
        ///     Connects to Inworld's servers.
        /// </summary>
        public Coroutine Connect()
        {
            return StartCoroutine(ConnectToServer());
        }

        /// <summary>
        ///     Initialize and start the Playground demo.
        /// </summary>
        public void Play()
        {
            // if (m_CurrentScene.name == SetupSceneName)
            // {
            //     SceneManager.LoadScene(LobbySceneName);
            //     return;
            // }

            if (!CheckNetworkComponent())
                throw new MissingComponentException("Missing Inworld client.");

            if (!CheckAudioComponent())
                throw new MissingComponentException("Missing PlaygroundAECAudioCapture component.");

            CursorHandler.LockCursor();
            InworldController.Audio.ChangeInputDevice(m_Settings.MicrophoneDevice);
            if (InworldController.Audio.SampleMode != MicSampleMode.NO_MIC)
                switch (m_Settings.InteractionMode)
                {
                    case MicrophoneMode.Interactive:
                        InworldController.Audio.SampleMode = MicSampleMode.AEC;
                        break;
                    case MicrophoneMode.PushToTalk:
                        InworldController.Audio.SampleMode = MicSampleMode.PUSH_TO_TALK;
                        break;
                    case MicrophoneMode.TurnByTurn:
                        InworldController.Audio.SampleMode = MicSampleMode.TURN_BASED;
                        break;
                }

            EnableAllWorldSpaceGraphicRaycasters();

            Paused = false;
            OnPlay?.Invoke();
        }

        /// <summary>
        ///     Updates the Brain Name of an Inworld character with the current Workspace ID.
        /// </summary>
        /// <param name="character">The Inworld character to update.</param>
        public void UpdateCharacterBrain(InworldCharacter character)
        {
            character.Data.brainName =
                $"workspaces/{m_Settings.WorkspaceId}/characters/{character.Data.brainName.Split('/')[3]}";
        }

        /// <summary>
        ///     Enables all WorldSpaceGraphicRaycasters in the scene.
        /// </summary>
        public void EnableAllWorldSpaceGraphicRaycasters()
        {
#if UNITY_2022_3_OR_NEWER
            WorldSpaceGraphicRaycaster[] worldSpaceGraphicRaycasters =
                FindObjectsByType<WorldSpaceGraphicRaycaster>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
#else
            var worldSpaceGraphicRaycasters = FindObjectsOfType<WorldSpaceGraphicRaycaster>();
#endif
            foreach (WorldSpaceGraphicRaycaster worldSpaceGraphicRaycaster in worldSpaceGraphicRaycasters)
                worldSpaceGraphicRaycaster.enabled = true;
        }

        /// <summary>
        ///     Disables all WorldSpaceGraphicRaycasters in the scene.
        /// </summary>
        public void DisableAllWorldSpaceGraphicRaycasters()
        {
#if UNITY_2022_3_OR_NEWER
            WorldSpaceGraphicRaycaster[] worldSpaceGraphicRaycasters =
                FindObjectsByType<WorldSpaceGraphicRaycaster>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
#else
            var worldSpaceGraphicRaycasters = FindObjectsOfType<WorldSpaceGraphicRaycaster>();
#endif
            foreach (WorldSpaceGraphicRaycaster worldSpaceGraphicRaycaster in worldSpaceGraphicRaycasters)
                worldSpaceGraphicRaycaster.enabled = false;
        }

        public void Pause()
        {
            if (Paused)
                return;
            DisableAllWorldSpaceGraphicRaycasters();
            Subtitle.Instance.Clear();
            Paused = true;
            OnPause?.Invoke();
        }

        bool CheckAudioComponent()
        {
            return InworldController.Instance.GetComponent<PlaygroundAECAudioCapture>();
        }

        bool CheckNetworkComponent()
        {
            return InworldController.Instance.GetComponent<InworldClient>();
        }

        void SetCharacterBrains()
        {
#if UNITY_2022_3_OR_NEWER
            InworldCharacter[] characters =
                FindObjectsByType<InworldCharacter>(FindObjectsInactive.Include, FindObjectsSortMode.None);
#else
            var characters = FindObjectsOfType<InworldCharacter>(true);
#endif
            foreach (InworldCharacter character in characters)
                UpdateCharacterBrain(character);
        }

        void RegisterCharacters()
        {
#if UNITY_2022_3_OR_NEWER
            InworldCharacter[] characters =
                FindObjectsByType<InworldCharacter>(FindObjectsInactive.Include, FindObjectsSortMode.None);
#else
            var characters = FindObjectsOfType<InworldCharacter>(true);
#endif
            foreach (InworldCharacter character in characters) InworldController.CharacterHandler.Register(character);
        }

        #region Event

        public event Action OnPlay;
        public event Action OnPause;
        public event Action OnStartSceneChange;
        public event Action OnEndSceneChange;
        public event Action OnStartInworldSceneChange;
        public event Action OnEndInworldSceneChange;

        #endregion

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

        void Awake()
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
            foreach (InworldSceneMapping sceneMapping in m_InworldSceneMapping)
                m_InworldSceneMappingDictionary.Add(sceneMapping.UnitySceneName, sceneMapping.InworldSceneName);

            // TODO(Yan): If GameData is null, it should be invoked by PlaygroundPanel. 
            // if (GameData == null && SceneManager.GetActiveScene().name != SetupSceneName)
            // {
            //     InworldAI.Log("The Playground GameData could not be found, switching to Setup scene.");
            //     SceneManager.LoadScene(SetupSceneName);
            //     m_SwitchingToSetup = true;
            //     return;
            // }

            if (InworldPlayground.GameData != null)
                m_Settings.WorkspaceId = InworldPlayground.GameData.sceneFullName.Split('/')[1];

            if (string.IsNullOrEmpty(m_Settings.PlayerName))
                SetPlayerName("Player");
            PlayerControllerPlayground.Instance.onCanvasOpen.AddListener(Pause);
            PlayerControllerPlayground.Instance.onCanvasClosed.AddListener(Play);
            Paused = true;
        }

        void Start()
        {
            m_EventSystem.enabled = true;
        }

        void OnEnable()
        {
            //TODO(Yan): OnSceneLoaded Should be handled by PlaygroundPanel.
            InworldController.Client.OnStatusChanged += OnStatusChanged;
            InworldController.Client.OnPacketReceived += OnPacketReceived;
        }

        void OnDisable()
        {
            if (InworldController.Client)
            {
                InworldController.Client.OnStatusChanged -= OnStatusChanged;
                InworldController.Client.OnPacketReceived -= OnPacketReceived;
            }
            // if (!PlayerControllerPlayground.Instance)
            //     return;
            // PlayerControllerPlayground.Instance.onCanvasOpen.RemoveListener(Pause);
            // PlayerControllerPlayground.Instance.onCanvasClosed.RemoveListener(Play);
        }

        void Update()
        {
            if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.F4))
                Application.Quit();
        }

        #endregion

        #region Event Callback Functions

        void OnStatusChanged(InworldConnectionStatus status)
        {
            Debug.Log("Status Changed: " + status);
        }

        void OnPacketReceived(InworldPacket incomingPacket)
        {
            if (incomingPacket is ControlPacket controlPacket &&
                controlPacket.control is CurrentSceneStatusEvent currentSceneStatusEvent)
            {
                InworldAI.Log("Inworld Scene Changed: " + currentSceneStatusEvent.currentSceneStatus.sceneName);
                m_CurrentInworldScene = currentSceneStatusEvent.currentSceneStatus.sceneName;
            }
        }
        //TODO(Yan): OnSceneLoaded Should be handled by PlaygroundPanel.


        #endregion

        #region Enumerators

        IEnumerator ConnectToServer()
        {
            while (InworldController.Client.Status != InworldConnectionStatus.Connected)
            {
                if (InworldController.Client.Status == InworldConnectionStatus.Idle)
                    InworldController.Instance.Init();
                else if (InworldController.Client.Status == InworldConnectionStatus.Initialized)
                    InworldController.Instance.Reconnect();
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }

        IEnumerator ChangeInworldSceneEnumerator(string sceneName, bool pause = true)
        {
            string sceneToLoad = $"workspaces/{m_Settings.WorkspaceId}/scenes/{sceneName}";
            if (m_CurrentInworldScene == sceneToLoad)
                yield break;

            OnStartInworldSceneChange?.Invoke();
            if (pause)
                Pause();
            InworldCharacter currentCharacter = InworldController.CharacterHandler.CurrentCharacter;
            InworldController.CharacterHandler.CurrentCharacter = null;

            if (InworldController.Client.Status != InworldConnectionStatus.Connected)
            {
                InworldController.Client.CurrentScene = sceneToLoad;
                yield return StartCoroutine(ConnectToServer());
            }
            else
            {
                string currentInworldScene = m_CurrentInworldScene;
                InworldController.Client.LoadScene(sceneToLoad);
                yield return new WaitUntil(() =>
                    currentInworldScene != m_CurrentInworldScene ||
                    InworldController.Client.Status != InworldConnectionStatus.Connected);
            }

            if (currentCharacter)
                InworldController.CurrentCharacter = currentCharacter;

            if (pause)
                Play();
            OnEndInworldSceneChange?.Invoke();
        }

        IEnumerator ChangeSceneEnumerator(string sceneName)
        {
            OnStartSceneChange?.Invoke();
            Pause();

            InworldAI.Log("Starting scene load: " + sceneName);
            AsyncOperation sceneLoadOperation = SceneManager.LoadSceneAsync(sceneName);
            sceneLoadOperation.allowSceneActivation = false;

            yield return new WaitUntil(() => sceneLoadOperation.progress >= 0.9f);
            sceneLoadOperation.allowSceneActivation = true;

            yield return new WaitUntil(() => sceneLoadOperation.isDone);
            InworldAI.Log("Completed scene load: " + sceneName);

            m_SceneChangeCoroutine = null;
            OnEndSceneChange?.Invoke();
        }

        IEnumerator SetupScene()
        {
            LoadData();
            SetCharacterBrains();

            if (!m_InworldSceneMappingDictionary.TryGetValue(SceneManager.GetActiveScene().name,
                    out string inworldSceneName))
                InworldAI.LogException("Missing scene in InworldSceneMappingDictionary: " +
                                       SceneManager.GetActiveScene().name);

            if (InworldController.Status != InworldConnectionStatus.Connected)
                InworldController.Client.CurrentScene =
                    $"workspaces/{m_Settings.WorkspaceId}/scenes/{inworldSceneName}";
            else
                yield return StartCoroutine(ChangeInworldSceneEnumerator(inworldSceneName, false));
            Play();
        }

        #endregion
    }
}