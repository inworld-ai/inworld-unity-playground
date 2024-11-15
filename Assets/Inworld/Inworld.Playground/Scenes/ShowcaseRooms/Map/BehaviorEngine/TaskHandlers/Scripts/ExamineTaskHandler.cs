using Inworld;
using Inworld.BehaviorEngine;
using System.Collections;
using System.Collections.Generic;
using Inworld.Map;
using Playground;

public class ExamineTaskHandler : ItemTaskHandler
{
    public override bool Validate(Task task, InworldCharacter inworldCharacter, Dictionary<string, string> parameters, out string message)
    {
        if(!base.Validate(task, inworldCharacter, parameters, out message))
            return false;

        EntityItem whatItem = m_EntityItems["what"];
        
        Inventory inventory = inworldCharacter.GetComponent<Inventory>();
        if (inventory && inventory.Contains(whatItem.ID))
            return true;
        
        if (!IsItemNearby(inworldCharacter, whatItem))
        {
            message = $"Item is too far away: {whatItem.ID}.";
            return false;
        }
        
        return true;
    }
    
    public override IEnumerator Execute(InworldCharacter inworldCharacter)
    {
        EntityItem whatEntity = m_EntityItems["what"];
        string log = $"{inworldCharacter.Name} examines {whatEntity.DisplayName}: {whatEntity.Description}";
        InworldAI.Log(log);
        Complete();

        yield break;
    }
}
