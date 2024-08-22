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
    public class EntityManager : SingletonBehavior<EntityManager>
    {
        [SerializeField] private List<Entity> entities;

        private Dictionary<string, Task> tasks;
        private List<EntityItem> items;

        public bool FindTask(string id, out Task task)
        {
            return tasks.TryGetValue(id, out task);
        }
        
        private void Awake()
        {
            items = new List<EntityItem>(FindObjectsOfType<EntityItem>());
            tasks = new Dictionary<string, Task>();
            entities.ForEach(entity =>
            {
                entity.Tasks.ForEach(task => tasks.Add(task.ID, task));
            });
        }

        private void OnEnable()
        {
            InworldController.CharacterHandler.CurrentCharacters.ForEach(character => 
                character.Event.onTaskReceived.AddListener(OnTaskReceived));
        }

        private void OnDisable()
        {
            if(InworldController.CharacterHandler)
                InworldController.CharacterHandler.CurrentCharacters.ForEach(character => 
                    character.Event.onTaskReceived.RemoveListener(OnTaskReceived));
        }

        private void Start()
        {
            foreach (EntityItem item in items)
                InworldController.Client.CreateItems(new List<Packet.EntityItem>() { item.Get() }, item.GetEntityIDs());
        }

        private void OnTaskReceived(string brainName, string taskID)
        {
            InworldCharacter inworldCharacter = InworldController.CharacterHandler.GetCharacterByBrainName(brainName);
            if (FindTask(taskID, out Task task))
                task.PerformTask(inworldCharacter);
        }
    }
}
