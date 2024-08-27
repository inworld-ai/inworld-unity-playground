/*************************************************************************************************
 * Copyright 2024 Theai, Inc. (DBA Inworld)
 *
 * Use of this source code is governed by the Inworld.ai Software Development Kit License Agreement
 * that can be found in the LICENSE.md file or at https://www.inworld.ai/sdk-license
 *************************************************************************************************/

using System.Collections.Generic;
using Inworld.Entities;
using Inworld.Packet;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Inworld.Map
{
    public class EntityManager : SingletonBehavior<EntityManager>
    {
        public UnityEvent<string, string> onTaskStart;
        public UnityEvent<string, string> onTaskComplete;
        public UnityEvent<string, string> onTaskFail;
        
        [SerializeField] private List<Entity> m_Entities;

        private Dictionary<string, Task> m_Tasks;
        private List<EntityItem> m_Items;

        public bool FindTask(string id, out Task task)
        {
            return m_Tasks.TryGetValue(id, out task);
        }

        public bool FindItem(string id, out EntityItem item)
        {
            item = m_Items.Find((entityItem => entityItem.ID == id));
            return item != null;
        }
        
        protected virtual void Awake()
        {
            m_Items = new List<EntityItem>(FindObjectsOfType<EntityItem>());
            m_Tasks = new Dictionary<string, Task>();
            m_Entities.ForEach(entity =>
            {
                entity.Tasks?.ForEach(task => { m_Tasks.TryAdd(task.ID, task); });
            });
        }

        protected virtual void OnEnable()
        {
            InworldController.CharacterHandler.CurrentCharacters.ForEach(character => 
                character.Event.onTaskReceived.AddListener(OnTaskReceived));
        }

        protected virtual void OnDisable()
        {
            if(InworldController.CharacterHandler)
                InworldController.CharacterHandler.CurrentCharacters.ForEach(character => 
                    character.Event.onTaskReceived.RemoveListener(OnTaskReceived));
        }

        protected virtual void Start()
        {
            foreach (EntityItem item in m_Items)
                InworldController.Client.CreateItems(new List<Packet.EntityItem>() { item.Get() }, item.GetEntityIDs());
        }

        protected virtual void OnTaskReceived(string brainName, string taskID, List<TriggerParameter> parameters)
        {
            InworldCharacter inworldCharacter = InworldController.CharacterHandler.GetCharacterByBrainName(brainName);
            if (FindTask(taskID, out Task task))
                StartTask(task, inworldCharacter, parameters);
            else
                InworldAI.LogWarning($"Unsupported task: {taskID}");
        }

        internal void StartTask(Task task, InworldCharacter inworldCharacter, List<TriggerParameter> parameters)
        {
            StartCoroutine(task.Perform(inworldCharacter, parameters));
            onTaskStart?.Invoke(inworldCharacter.BrainName, task.ID);
            InworldAI.Log($"{inworldCharacter.Name} started task: {task.name}");
        }
        
        internal void CompleteTask(Task task, InworldCharacter inworldCharacter)
        {
            InworldMessenger.SendTaskSucceeded(task.ID, inworldCharacter.BrainName);
            onTaskComplete?.Invoke(inworldCharacter.BrainName, task.ID);
            InworldAI.Log($"{inworldCharacter.Name} completed task: {task.name}");
        }
        
        internal void FailTask(Task task, InworldCharacter inworldCharacter, string reason)
        {
            InworldMessenger.SendTaskFailed(task.ID, reason, inworldCharacter.BrainName);
            onTaskFail?.Invoke(inworldCharacter.BrainName, task.ID);
            InworldAI.Log($"{inworldCharacter.Name} failed task: {task.name} because: {reason}");
        }
    }
}
