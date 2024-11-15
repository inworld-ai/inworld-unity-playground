using Inworld;
using Inworld.BehaviorEngine;
using System.Collections;
using System.Collections.Generic;
using Inworld.Map;
using Playground;

public class TakeTaskHandler : ItemTaskHandler
{
    protected Inventory m_Inventory;
    
    public override bool Validate(Task task, InworldCharacter inworldCharacter, Dictionary<string, string> parameters, out string message)
    {
        if(!base.Validate(task, inworldCharacter, parameters, out message))
            return false;

        EntityItem whatItem = m_EntityItems["what"];
        
        m_Inventory = inworldCharacter.GetComponent<Inventory>();
        if (!m_Inventory)
        {
            message = $"{inworldCharacter.Name} can not carry items.";
            return false;
        }
        
        if (m_Inventory.Contains(whatItem.ID))
        {
            message = $"{inworldCharacter.Name} already has item: {whatItem.ID}.";
            return false;
        }
        
        if (!IsItemNearby(inworldCharacter, whatItem))
        {
            message = $"Item is too far away: {whatItem.ID}.";
            return false;
        }
        
        return true;
    }

    public override IEnumerator Execute(InworldCharacter inworldCharacter)
    {
        EntityItem whatItem = m_EntityItems["what"];
        
        if (!m_Inventory.AddItem(whatItem))
        {
            Fail($"{inworldCharacter.Name} could not pick up item: {whatItem.ID}.");
            yield break;
        }
        whatItem.gameObject.SetActive(false);
        InworldAI.Log($"{inworldCharacter.Name} took item: {whatItem.DisplayName}.");
        whatItem.UpdateProperty("location", $"In {inworldCharacter.Name}'s inventory.");
        Complete();
    }
}
