/*************************************************************************************************
 * Copyright 2024 Theai, Inc. dba Inworld AI
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using UnityEngine;

namespace Inworld.Playground
{
    /// <summary>
    ///     Handler for the Environmental Triggers showcase.
    /// </summary>
    public class EnvironmentalTriggersMovementShowcase : MovementShowcase
    {
        [SerializeField]
        Transform m_AreaATransform;
        [SerializeField]
        Transform m_AreaBTransform;

        protected override void OnInworldCharacterGoalCompleted(string triggerName)
        {
            switch (triggerName)
            {
                case "motion_area_a":
                case "request_area_a":
                    MoveTo(m_AreaATransform.position);
                    break;
                case "motion_area_b":
                case "request_area_b":
                    MoveTo(m_AreaBTransform.position);
                    break;
            }
        }
    }

}
