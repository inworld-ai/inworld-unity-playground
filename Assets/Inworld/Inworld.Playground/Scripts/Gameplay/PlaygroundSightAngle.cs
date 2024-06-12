/*************************************************************************************************
 * Copyright 2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using Inworld.Sample;
using UnityEngine;

namespace Inworld.Playground
{
    /// <summary>
    ///     Inworld SightAngle component for determining character priority.
    ///     Defaults the PlayerTransform to Camera.main.
    /// </summary>
    public class PlaygroundSightAngle : SightAngle
    {
        public override Transform PlayerTransform
        {
            get
            {
                if (m_PlayerTransform)
                    return m_PlayerTransform;
                if (Camera.main)
                    m_PlayerTransform = Camera.main.transform;
                return m_PlayerTransform;
            }
        }
    }
}
