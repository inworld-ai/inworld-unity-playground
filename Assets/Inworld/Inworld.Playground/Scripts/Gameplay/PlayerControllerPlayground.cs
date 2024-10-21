/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using Inworld.Assets;
using Inworld.Sample;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inworld.Playground
{
    /// <summary>
    ///     Controls the player.
    ///     Inherits from Inworld's PlayerController.
    ///     Handles movement using Unity's legacy input system.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerControllerPlayground : PlayerController3D
    {
        public bool BlockKeyInput { get; set; }
        
        [Header("Movement")]
        [SerializeField] private float m_MoveSpeed = 5;
        [Range(0, 1)]
        [SerializeField] private float m_MoveInterpolationFactor = 0.6f;
        [SerializeField] private float m_RotationSpeed = 2;
        [SerializeField] bool m_InvertY;
        private CharacterController m_CharacterController;
        private Camera m_Camera;
        private float m_HorizontalAxis = 0, m_VerticalAxis = 0;
        private bool m_InFocus;
        
        InputAction m_LeftClickInputAction;
        InputAction m_MouseDeltaInputAction;
        InputAction m_SpeedUpInputAction;
        InputAction m_SpeedInputAction;
        InputAction m_MoveInputAction;

        protected new void Awake()
        {
            base.Awake();
            m_LeftClickInputAction = InworldAI.InputActions["LeftClick"];
            m_MouseDeltaInputAction = InworldAI.InputActions["MouseDelta"];
            m_SpeedUpInputAction = InworldAI.InputActions["SpeedUp"];
            m_SpeedInputAction = InworldAI.InputActions["Speed"];
            m_MoveInputAction = InworldAI.InputActions["Move"];
        }
        protected void OnEnable()
        {
            m_Camera = Camera.main;
            m_CharacterController = GetComponent<CharacterController>();
            onPlayerSpeaks.AddListener(OnPlayerSpeaks);
            onCanvasOpen.AddListener(OnCanvasOpen);
            onCanvasClosed.AddListener(OnCanvasClosed);
        }

        void OnCanvasClosed()
        {
            CursorHandler.LockCursor();
            PlaygroundManager.Instance.EnableAllWorldSpaceGraphicRaycasters();
        }

        void OnCanvasOpen()
        {
            CursorHandler.UnlockCursor();
            PlaygroundManager.Instance.DisableAllWorldSpaceGraphicRaycasters();
        }

        protected void OnDisable()
        {
            onPlayerSpeaks.RemoveListener(OnPlayerSpeaks);
            onCanvasOpen.RemoveListener(OnCanvasOpen);
            onCanvasClosed.RemoveListener(OnCanvasClosed);
        }

        void OnPlayerSpeaks(string text)
        {
            Subtitle.Instance.SetSubtitle(InworldAI.User.Name, text);
        }

        protected override void HandleInput()
        {
            if(PlaygroundManager.Instance.Paused)
                return;

            if (!BlockKeyInput)
            {
                base.HandleInput();
            }
            
            if(Input.GetMouseButtonDown(0) && InteractionSystem.Instance.IsHoveringInteractable)
                InteractionSystem.Instance.Interact();
            HandleMovement();
        }
        Vector3 GetInputTranslationDirection()
        {
            return m_MoveInputAction.ReadValue<Vector3>();
        }
        protected void HandleMovement()
        {
            if (PlaygroundManager.Instance.Paused)
                return;
            if (UILayer > 0)
                return;

            Vector3 mouseDelta = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
            mouseDelta.x = Mathf.Clamp(mouseDelta.x, -1, 1);
            mouseDelta.y = Mathf.Clamp(mouseDelta.y, -1, 1);
            Vector3 newEuler = m_Camera.transform.localEulerAngles + mouseDelta * m_RotationSpeed;
            float xClamped = Mathf.Clamp(newEuler.x, newEuler.x <= 180 ? -89f : 271, newEuler.x <= 180 ? 89f : 360);
            m_Camera.transform.localEulerAngles = new Vector3(xClamped, newEuler.y, newEuler.z);
            
            if (BlockKeyInput) return;
            
            float newHorizontalAxis = 0, newVerticalAxis = 0;
            if (Input.GetKey(KeyCode.W))
                newVerticalAxis += 1;
            if (Input.GetKey(KeyCode.S))
                newVerticalAxis += -1;
            m_VerticalAxis = Mathf.Lerp(m_VerticalAxis, newVerticalAxis, m_MoveInterpolationFactor);
            
            if (Input.GetKey(KeyCode.A))
                newHorizontalAxis += -1;
            if (Input.GetKey(KeyCode.D))
                newHorizontalAxis += 1;
            m_HorizontalAxis = Mathf.Lerp(m_HorizontalAxis, newHorizontalAxis, m_MoveInterpolationFactor);
            Vector3 inputAxis = new Vector3(m_HorizontalAxis, 0, m_VerticalAxis);
            Vector3 direction = Matrix4x4.Rotate(m_Camera.transform.rotation) * inputAxis;
            direction = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
            m_CharacterController.SimpleMove(direction * m_MoveSpeed);
        }

    }
}
