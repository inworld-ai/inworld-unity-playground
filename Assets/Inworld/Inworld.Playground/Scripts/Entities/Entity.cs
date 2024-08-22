/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace Inworld.Map
{
    [CreateAssetMenu(fileName = "Entity", menuName = "Inworld/Entity Item")]
    public class Entity : ScriptableObject
    {
        public string ID => id;
        public List<Task> Tasks => tasks;

        [SerializeField] private string id;
        [SerializeField] private List<Task> tasks;
    }
}
