using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inworld.Playground
{
    public class TestMove1 : MonoBehaviour
    {
        CharacterController m_CharacterController;
        InputAction m_Move;
        InputAction m_Rotate;

        [SerializeField] TMP_Text m_TextArea;

        InputAction m_Options;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            InworldAI.InputActions.Enable();
            m_CharacterController = GetComponent<CharacterController>();
            m_Move = InworldAI.InputActions["Move"];
            m_Rotate = InworldAI.InputActions["Rotate"];
            m_Options = InworldAI.InputActions["Options"];
        }

        // Update is called once per frame
        void Update()
        {
            Vector2 move = m_Move.ReadValue<Vector2>();
            Vector2 rotate = m_Rotate.ReadValue<Vector2>();
            if (m_TextArea)
                m_TextArea.text = $"Move: {move.ToString()} Rotate: {rotate.ToString()}";
            if (m_Options.IsPressed())
                m_TextArea.text += " Options Pressed";
            m_CharacterController.SimpleMove(new Vector3(move.x , 0, move.y));
        }
    }
}
