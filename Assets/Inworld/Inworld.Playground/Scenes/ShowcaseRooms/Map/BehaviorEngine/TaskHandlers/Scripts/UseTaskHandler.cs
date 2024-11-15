using Inworld;
using Inworld.BehaviorEngine;
using System.Collections;
using Inworld.Map;

public class UseTaskHandler : TaskHandler
{
    public override IEnumerator Execute(InworldCharacter inworldCharacter)
    {
        EntityItem whatEntity = m_EntityItems["what"];
        
        Inventory inventory = inworldCharacter.GetComponent<Inventory>();
        if (!inventory || !inventory.Contains(whatEntity.ID))
        {
            Fail($"{inworldCharacter.Name} does not have the item: {whatEntity.ID}.");
            yield break;
        }

        EntityItemKey entityItemKey = whatEntity.GetComponent<EntityItemKey>();
        if (entityItemKey)
            entityItemKey.Use(inworldCharacter);
            
        InworldAI.Log($"{inworldCharacter.Name} uses {whatEntity.DisplayName}.");
        Complete();
    }
}
