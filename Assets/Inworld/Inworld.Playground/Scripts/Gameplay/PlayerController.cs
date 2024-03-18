/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using Inworld.Sample;
using UnityEngine;

namespace Inworld.Playground
{
    /// <summary>
    ///     Controls the player.
    ///     Inherits from Inworld's PlayerController.
    ///     Handles movement using Unity's legacy input system.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : PlayerController3D
    {
        private static PlayerController m_Instance;
        public new static PlayerController Instance
        {
            get
            {
                if (m_Instance)
                    return m_Instance;
                m_Instance = FindObjectOfType<PlayerController>();
                return m_Instance;
            }
        }
        
        public bool BlockKeyInput { get; set; }
        
        [Header("Movement")]
        [SerializeField] private float m_MoveSpeed = 5;
        [Range(0, 1)]
        [SerializeField] private float m_MoveInterpolationFactor = 0.6f;
        [SerializeField] private float m_RotSpeed = 2;
        
        private CharacterController m_CharacterController;
        private float horizontalAxis = 0, verticalAxis = 0;
        private bool inFocus;
        
        /// <summary>
        ///     Set whether this player is currently using push-to-talk to speak with Inworld characters.
        /// </summary>
        /// <param name="isOn">Whether to enable/disable push-to-talk (isOn == enable).</param>
        public void SetPushToTalk(bool isOn)
        {
            m_PushToTalk = isOn;
            
            InworldController.Audio.AutoPush = !m_ChatCanvas.activeSelf && !m_PushToTalk;
            InworldController.CharacterHandler.ManualAudioHandling = m_ChatCanvas.activeSelf || m_PushToTalk;
        }

        protected override void Awake()
        {
            base.Awake();
            m_CharacterController = GetComponent<CharacterController>();
        }

        protected override void Start()
        {
            
        }

        protected override void HandlePTT()
        {
            if (PlaygroundManager.Instance.Paused || InworldController.CurrentCharacter == null) return;
            
            if (Input.GetKeyDown(m_PushToTalkKey))
            {
                m_PTTKeyPressed = true;
                InworldController.Instance.StartAudio();
            }
            else if (Input.GetKeyUp(m_PushToTalkKey))
            {
                m_PTTKeyPressed = false;
                InworldController.Instance.PushAudio();
            }
        }

        protected override void HandleInput()
        {
            if(PlaygroundManager.Instance.Paused)
                return;

            if (!BlockKeyInput)
            {
                base.HandleInput();
            
                if (Input.GetKeyUp(KeyCode.BackQuote))
                {
                    if (!m_ChatCanvas.activeSelf)
                    {
                        CursorHandler.LockCursor();
                        PlaygroundManager.Instance.EnableAllWorldSpaceGraphicRaycasters();
                    }
                    else
                    {
                        CursorHandler.UnlockCursor();
                        PlaygroundManager.Instance.DisableAllWorldSpaceGraphicRaycasters();
                    }
                }
            }
            
            if(Input.GetMouseButtonDown(0) && InteractionSystem.Instance.IsHoveringInteractable)
                InteractionSystem.Instance.Interact();
            
            if (!m_ChatCanvas.activeSelf)
                HandleMovement();
        }

        protected void HandleMovement()
        {
            if (Time.timeScale == 0)
                return;
            
            Vector3 mouseDelta = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);

            Vector3 newEuler = transform.localEulerAngles + mouseDelta * m_RotSpeed;
            float xClamped = Mathf.Clamp(newEuler.x, newEuler.x <= 180 ? -89f : 271, newEuler.x <= 180 ? 89f : 360);
            transform.localEulerAngles = new Vector3(xClamped, newEuler.y, newEuler.z);

            if (BlockKeyInput) return;
            
            float newHorizontalAxis = 0, newVerticalAxis = 0;
            if (Input.GetKey(KeyCode.W))
                newVerticalAxis += 1;
            if (Input.GetKey(KeyCode.S))
                newVerticalAxis += -1;
            verticalAxis = Mathf.Lerp(verticalAxis, newVerticalAxis, m_MoveInterpolationFactor);
            
            if (Input.GetKey(KeyCode.A))
                newHorizontalAxis += -1;
            if (Input.GetKey(KeyCode.D))
                newHorizontalAxis += 1;
            horizontalAxis = Mathf.Lerp(horizontalAxis, newHorizontalAxis, m_MoveInterpolationFactor);
            Vector3 inputAxis = new Vector3(horizontalAxis, 0, verticalAxis);
            Vector3 direction = Matrix4x4.Rotate(transform.rotation) * inputAxis;
            direction = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
                            
            m_CharacterController.SimpleMove(direction * m_MoveSpeed);
        }
    }
}
