/*************************************************************************************************
 * Copyright 2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System;
using UnityEngine;

namespace Inworld.Map
{
    public class AnimatorBoolParamHandler : MonoBehaviour
    {
        [SerializeField] protected string m_ParameterName;
        
        protected Animator m_Animator;
        protected int m_ParameterNameHash;

        protected virtual void Awake()
        {
            m_Animator = GetComponent<Animator>();
            m_ParameterNameHash = Animator.StringToHash(m_ParameterName);
        }

        public virtual bool SetTrue()
        {
            if (m_Animator.GetBool(m_ParameterNameHash))
                return false;
            m_Animator.SetBool(m_ParameterNameHash, true);
            return true;
        }

        public virtual bool SetFalse()
        {
            if (!m_Animator.GetBool(m_ParameterNameHash))
                return false;
            m_Animator.SetBool(m_ParameterNameHash, false);
            return true;
        }
    }
}
