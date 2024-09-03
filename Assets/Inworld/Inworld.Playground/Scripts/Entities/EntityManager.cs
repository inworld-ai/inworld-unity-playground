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
        public UnityEvent<string, string, Dictionary<string, string>> onTaskStart;
        public UnityEvent<string, string, Dictionary<string, string>> onTaskComplete;
        public UnityEvent<string, string, string, Dictionary<string, string>> onTaskFail;
        
        [SerializeField] private List<Entity> m_Entities;

        private Dictionary<string, Task> m_Tasks;
        private List<EntityItem> m_Items;

        public void AddItem(EntityItem entityItem)
        {
            if (m_Items.Contains(entityItem))
            {
                InworldAI.LogWarning($"Attempted to add an entity item that already exists: {entityItem.ID}");
                return;
            }
            
            m_Items.Add(entityItem);
            if(InworldController.Client)
                InworldController.Client.CreateOrUpdateItems(new List<Packet.EntityItem>() { entityItem.Get() }, entityItem.GetEntityIDs());
        }

        public void UpdateItem(EntityItem entityItem)
        {
            if (!m_Items.Contains(entityItem))
            {
                InworldAI.LogWarning($"Attempted to update an entity item that does not exist: {entityItem.ID}");
                return;
            }
            
            if(InworldController.Client)
                InworldController.Client.CreateOrUpdateItems(new List<Packet.EntityItem>() { entityItem.Get() }, entityItem.GetEntityIDs());
        }

        public void RemoveItem(EntityItem entityItem)
        {
            if (!m_Items.Contains(entityItem))
            {
                InworldAI.LogWarning($"Attempted to remove an entity item that does not exist: {entityItem.ID}");
                return;
            }

            m_Items.Remove(entityItem);
            if(InworldController.Client)
                InworldController.Client.DestroyItems(new List<string>() { entityItem.ID });
        }

        public bool FindTask(string taskName, out Task task)
        {
            return m_Tasks.TryGetValue(taskName, out task);
        }

        public bool FindItem(string id, out EntityItem item)
        {
            item = m_Items.Find((entityItem => entityItem.ID == id));
            return item != null;
        }
        
        protected virtual void Awake()
        {
            m_Items = new List<EntityItem>();
            m_Tasks = new Dictionary<string, Task>();
            m_Entities.ForEach(entity =>
            {
                entity.Tasks?.ForEach(task => { m_Tasks.TryAdd(task.TaskName, task); });
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

        protected virtual void OnTaskReceived(string brainName, string taskName, List<TriggerParameter> parameters)
        {
            InworldCharacter inworldCharacter = InworldController.CharacterHandler.GetCharacterByBrainName(brainName);
            if (FindTask(taskName, out Task task))
                StartTask(task, inworldCharacter, ParseParameters(parameters));
            else
                InworldAI.LogWarning($"Unsupported task: {taskName}");
        }
        
        protected Dictionary<string, string> ParseParameters(List<TriggerParameter> parameters)
        {
            Dictionary<string, string> parameterDictionary = new Dictionary<string, string>();
            foreach (TriggerParameter triggerParameter in parameters)
                parameterDictionary.Add(triggerParameter.name, triggerParameter.value);
            return parameterDictionary;
        }

        internal void StartTask(Task task, InworldCharacter inworldCharacter, Dictionary<string, string> parameters)
        {
            onTaskStart?.Invoke(inworldCharacter.BrainName, task.TaskName, parameters);
            InworldAI.Log($"{inworldCharacter.Name} started task: {task.name}");
            StartCoroutine(task.Perform(inworldCharacter, parameters));
        }
        
        internal void CompleteTask(Task task, InworldCharacter inworldCharacter, Dictionary<string, string> parameters)
        {
            InworldMessenger.SendTaskSucceeded(parameters["task_id"], inworldCharacter.BrainName);
            onTaskComplete?.Invoke(inworldCharacter.BrainName, task.TaskName, parameters);
            InworldAI.Log($"{inworldCharacter.Name} completed task: {task.name}");
        }
        
        internal void FailTask(Task task, InworldCharacter inworldCharacter, string reason, Dictionary<string, string> parameters)
        {
            InworldMessenger.SendTaskFailed(parameters["task_id"], reason, inworldCharacter.BrainName);
            onTaskFail?.Invoke(inworldCharacter.BrainName, task.TaskName, reason, parameters);
            InworldAI.Log($"{inworldCharacter.Name} failed task: {task.name} because: {reason}");
        }
    }
}
