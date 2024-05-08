/*************************************************************************************************
 * Copyright 2022-2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using System.Linq;
using Inworld.Assets;
using Inworld.Packet;
using Inworld.Sample.Innequin;
using UnityEngine;

namespace Inworld.Playground
{
    public class InworldFaceAnimationInnequinV2 : InworldFacialAnimation
    {
        [SerializeField] Animator m_EmoteAnimator;
        [SerializeField] SkinnedMeshRenderer m_BrowsMesh;
        [SerializeField] SkinnedMeshRenderer m_EyesMesh;
        [SerializeField] SkinnedMeshRenderer m_MouthMesh;
        [SerializeField] SkinnedMeshRenderer m_NoseMesh;
        [SerializeField] FaceTransformData m_FaceTransformData;
        [SerializeField] LipsyncMap m_FaceAnimData;
        [SerializeField] Texture m_DefaultMouth;
        [SerializeField] Material m_Facial;
        [Range(-1, 1)][SerializeField] float m_BlinkRate;
        List<Texture> m_LipsyncTextures = new List<Texture>();
        List<PhonemeInfo> m_CurrentPhoneme = new List<PhonemeInfo>();

        Texture m_CurrentEyeOpen;
        Texture m_CurrentEyeClosed;
        
        Material m_matEyeBlow;
        Material m_matEye;
        Material m_matNose;
        Material m_matMouth;
        
        bool m_IsBlinking;
        float m_CurrentAudioTime;
        static readonly int s_Emotion = Animator.StringToHash("Emotion");
        const int k_VisemeSil = 0;
        const int k_VisemeCount = 15;

        void Start()
        {
            _InitMaterials();
        }

        void _InitMaterials()
        {
            m_matEyeBlow = _CreateFacialMaterial($"eyeBlow_{m_Character.Data.brainName.GetHashCode()}");
            m_matEye = _CreateFacialMaterial($"eye_{m_Character.Data.brainName.GetHashCode()}");
            m_matNose = _CreateFacialMaterial($"nose_{m_Character.Data.brainName.GetHashCode()}");
            m_matMouth = _CreateFacialMaterial($"mouth_{m_Character.Data.brainName.GetHashCode()}");
            m_BrowsMesh.material = m_matEyeBlow;
            m_EyesMesh.material = m_matEye;
            m_NoseMesh.material = m_matNose;
            m_MouthMesh.material = m_matMouth;
            _MorphFaceEmotion(FacialEmotion.Neutral);
        }
        Material _CreateFacialMaterial(string matName)
        {
            Material instance = Instantiate(m_Facial);
            instance.name = matName;
            return instance;
        }
        void _MorphFaceEmotion(FacialEmotion emotion)
        {
            FaceTransform facialData = m_FaceTransformData[emotion.ToString()];
            if (facialData == null)
                return;
            m_matEyeBlow.mainTexture = facialData.eyeBrow;
            m_CurrentEyeOpen = facialData.eye;
            m_CurrentEyeClosed = facialData.eyeClosed;
            m_matNose.mainTexture = facialData.nose;
            m_DefaultMouth = facialData.mouthDefault;
            m_LipsyncTextures = facialData.mouth;
        }
        void _ProcessEmotion(string emotionBehavior)
        {
            EmotionMapData emoMapData = m_EmotionMap[emotionBehavior];
            if (emoMapData == null)
            {
                Debug.LogError($"Unhandled emotion {emotionBehavior}");
                return;
            }
            if (m_EmoteAnimator)
                m_EmoteAnimator.SetInteger(s_Emotion, (int)emoMapData.emoteAnimation);
            _MorphFaceEmotion(emoMapData.facialEmotion);
        }
        protected override void BlinkEyes()
        {
            m_IsBlinking = Mathf.Sin(Time.time) < m_BlinkRate;
            m_matEye.mainTexture = m_IsBlinking ? m_CurrentEyeClosed : m_CurrentEyeOpen;
        }
        protected override void Reset()
        {
            if (m_LipsyncTextures.Count == k_VisemeCount)
                m_matMouth.mainTexture = m_LipsyncTextures[k_VisemeSil];
            else
                m_matMouth.mainTexture = m_DefaultMouth;
        }
        protected override void ProcessLipSync()
        {
            if (!m_Character.IsSpeaking)
            {
                Reset();
                return;
            }
            m_CurrentAudioTime += Time.fixedDeltaTime;
            PhonemeInfo data = m_CurrentPhoneme?.LastOrDefault(p => p.startOffset < m_CurrentAudioTime);
            if (data == null || string.IsNullOrEmpty(data.phoneme))
            {
                Reset();
                return;
            }
            PhonemeToViseme p2v = m_FaceAnimData.p2vMap.FirstOrDefault(v => v.phoneme == data.phoneme);
            if (p2v == null)
            {
                Debug.LogError($"Not Found! {data.phoneme}");
                return;
            }
            if (p2v.visemeIndex >= 0 && p2v.visemeIndex < m_LipsyncTextures.Count)
                m_matMouth.mainTexture = m_LipsyncTextures[p2v.visemeIndex];
        }

        protected override void HandleLipSync(AudioPacket audioPacket)
        {
            m_CurrentAudioTime = 0;
            m_CurrentPhoneme = audioPacket.dataChunk.additionalPhonemeInfo;
        }
        protected override void HandleEmotion(EmotionPacket packet)
        {
            _ProcessEmotion(packet.emotion.behavior.ToUpper());
        }
    }
}
