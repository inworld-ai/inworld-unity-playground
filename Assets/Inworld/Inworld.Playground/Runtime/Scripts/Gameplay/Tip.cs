/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using TMPro;
using UnityEngine;

namespace Inworld.Playground
{
    public class Tip : MonoBehaviour
    {
        [SerializeField] GameObject m_Icon;
        [SerializeField] GameObject m_Bubble;
        [SerializeField] float m_TipDistance = 1f;
        [SerializeField] TMP_Text m_TextPanel;
        [SerializeField] string m_Text;
        [SerializeField] bool m_Reverse;

        Vector3 m_LastPlayerPosition;

        void Awake()
        {
            if (m_TextPanel)
                m_TextPanel.text = m_Text;
        }

        void Update()
        {
            if (m_Icon)
                m_Icon.transform.LookAt(PlayerControllerPlayground.Instance.transform);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponent<PlayerControllerPlayground>() || !m_Bubble) 
                return;
            m_Bubble.SetActive(true);
            Transform playerTransform = PlayerControllerPlayground.Instance.transform;
            m_Bubble.transform.position = playerTransform.position + playerTransform.forward * m_TipDistance;
            m_Bubble.transform.LookAt(playerTransform);
        }
        private void OnTriggerExit(Collider other)
        {
            if (!other.GetComponent<PlayerControllerPlayground>() || !m_Bubble) 
                return;
            m_Bubble.SetActive(false);
        }
    }
}