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
    ///     Visualizes audio from an Audio Source using the emission property of a MeshRenderer's primary material.
    /// </summary>
    [RequireComponent(typeof(MeshRenderer))]
    public class EmissionAudioSync : MonoBehaviour
    {
        private const string emissiveProperty = "_EmissionColor";

        [Header("Properties")]
        [SerializeField] private Color m_EmissionColor;
        [SerializeField] private float ampScalingFactor = 60f;
        [SerializeField] private float interpolationFactor = 0.005f;
        [Header("References")]
        [SerializeField] private AudioSource m_AudioSource;
        private MeshRenderer m_MeshRenderer;

        private Color m_DefaultColor;
        private float[] m_AudioOutput;
        private float m_Amplitude;

        private void Awake()
        {
            m_MeshRenderer = GetComponent<MeshRenderer>();
            m_AudioOutput = new float[2];
            m_DefaultColor = m_MeshRenderer.material.GetColor(emissiveProperty);
        }

        private void Update()
        {
            m_AudioSource.GetOutputData(m_AudioOutput, 0);
            m_Amplitude = Mathf.Lerp(m_Amplitude, Mathf.Max(-m_AudioOutput[0], 0), interpolationFactor);
            Color color = Color.Lerp(m_DefaultColor, m_EmissionColor, m_Amplitude * ampScalingFactor);

            m_MeshRenderer.material.SetColor(emissiveProperty, color);
        }
    }
}
