/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inworld.Playground
{
    public class WorldSpaceGraphicRaycaster : GraphicRaycaster
    {
        public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
        {
            // Ensures that raycasts from this GraphicRaycaster always originate from the center of the screen.
            eventData.position = new Vector2(Screen.width / 2f, Screen.height / 2f);   
            base.Raycast(eventData, resultAppendList);
        }
    }
}
