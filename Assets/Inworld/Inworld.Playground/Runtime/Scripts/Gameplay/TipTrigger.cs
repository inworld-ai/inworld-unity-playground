/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using UnityEngine;

namespace Inworld.Playground
{
    /// <summary>
    ///     Triggers the activation of a tip.
    /// </summary>
    public class TipTrigger : MonoBehaviour
    {
        [SerializeField] 
        private string m_TipText = "Enter tip text";
        [SerializeField] 
        private float m_TipDistance = 1;
        [SerializeField] 
        private GameObject m_TipPrefab;
        
        /// <summary>
        ///     Instantiate a new tip with the given prefab and text at the given distance in front of the player.
        ///     Then this object is destroyed.
        /// </summary>
        public void Activate()
        {
            var playerTransform = Camera.main.transform;
            var position = playerTransform.position + playerTransform.forward * m_TipDistance;
            var tipGameObject = Instantiate(m_TipPrefab, position, Quaternion.identity);
            Tip tip = tipGameObject.GetComponent<Tip>();
            tip.SetTip(m_TipText);
            
            Destroy(gameObject);
        }
    }
}
