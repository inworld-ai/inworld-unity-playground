/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using UnityEngine;

namespace Inworld.Playground
{
    public class UIOverlay : MonoBehaviour
    {
        [SerializeField] GameObject m_WinMacInstruction; 
        [SerializeField] GameObject m_MobileRotate;
        [SerializeField] GameObject m_MobileMove;
        [SerializeField] bool m_MobileDebugMode;
        void Awake()
        {
            if (m_MobileDebugMode ||
                Application.platform == RuntimePlatform.Android ||
                Application.platform == RuntimePlatform.IPhonePlayer)
            {
                if (m_WinMacInstruction)
                    m_WinMacInstruction.SetActive(false);
                if (m_MobileRotate)
                    m_MobileRotate.SetActive(true);
                if (m_MobileMove)
                    m_MobileMove.SetActive(true);
            }
        }
    }
}