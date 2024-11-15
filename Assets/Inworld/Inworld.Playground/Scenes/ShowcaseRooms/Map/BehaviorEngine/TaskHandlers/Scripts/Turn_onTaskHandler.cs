using Inworld;
using Inworld.BehaviorEngine;
using System.Collections;
using System.Collections.Generic;
using Inworld.Map;
using Playground;

public class Turn_onTaskHandler : ItemTaskHandler
{
    protected AnimatorBoolParamHandler m_AnimatorBoolParamHandler;
    
    public override bool Validate(Task task, InworldCharacter inworldCharacter, Dictionary<string, string> parameters, out string message)
    {
        if(!base.Validate(task, inworldCharacter, parameters, out message))
            return false;

        EntityItem whatItem = m_EntityItems["what"];
        
        if (!IsItemNearby(inworldCharacter, whatItem))
        {
            message = $"Item is too far away: {whatItem.ID}.";
            return false;
        }
        
        EntityItemLock entityEntityItemLock = whatItem.GetComponent<EntityItemLock>();
        if (entityEntityItemLock)
        {
            message = $"Could not turn on {whatItem.ID}: {whatItem.GetPropertyValue("lock")}.";
            return false;
        }
        
        m_AnimatorBoolParamHandler = whatItem.GetComponent<AnimatorBoolParamHandler>();
        if (!m_AnimatorBoolParamHandler)
        {
            message = $"Could not turn on: {whatItem.ID}.";
            return false;
        }

        return true;
    }

    public override IEnumerator Execute(InworldCharacter inworldCharacter)
    {
        EntityItem whatItem = m_EntityItems["what"];
        
        if (m_AnimatorBoolParamHandler.SetTrue())
        {
            whatItem.UpdateProperty("state", "Flipped down (on).");
            Complete();
        }
        else
            Fail($"{whatItem.ID} is already on.");
        yield break;
    }
}
