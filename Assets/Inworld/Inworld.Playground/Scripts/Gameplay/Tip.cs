/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections;
using TMPro;
using UnityEngine;

namespace Inworld.Playground
{
    /// <summary>
    ///     Handles a tip object.
    /// </summary>
    public class Tip : MonoBehaviour
    {
        [SerializeField] 
        private float m_DestroyDistance = 0.5f;

        [SerializeField] 
        private TMP_Text m_TextElement;

        private Vector3 m_PlayerInitialPosition;
        private Coroutine m_DestroyCoroutine;
        
        /// <summary>
        ///     Set the text of the tip.
        /// </summary>
        /// <param name="text">The text string to set as the tip.</param>
        public void SetTip(string text)
        {
            m_TextElement.text = text;
        }
        
        private void Start()
        {
            m_PlayerInitialPosition = PlayerControllerPlayground.Instance.transform.position;
        }

        private void Update()
        {
            if (Vector3.Distance(m_PlayerInitialPosition,
                    PlayerControllerPlayground.Instance.transform.position) >
                m_DestroyDistance)
            {
                if (m_DestroyCoroutine == null)
                    m_DestroyCoroutine = StartCoroutine(IDestroy());
            }
        }

        private IEnumerator IDestroy()
        {
            yield return new WaitForSecondsRealtime(1);
            Destroy(gameObject);
        }
    }
}
