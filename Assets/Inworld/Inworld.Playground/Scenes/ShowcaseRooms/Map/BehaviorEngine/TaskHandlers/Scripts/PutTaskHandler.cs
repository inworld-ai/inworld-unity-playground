using Inworld;
using Inworld.BehaviorEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Inworld.Map;
using Playground;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PutTaskHandler : ItemTaskHandler
{
    [SerializeField] private float m_ItemPlacementYOffset = 1;
    
    protected Inventory m_Inventory;
    protected Collider m_WhereItemCollider;
    
    public override bool Validate(Task task, InworldCharacter inworldCharacter, Dictionary<string, string> parameters, out string message)
    {
        if(!base.Validate(task, inworldCharacter, parameters, out message))
            return false;

        EntityItem whatItem = m_EntityItems["what"];
        
        m_Inventory = inworldCharacter.GetComponent<Inventory>();
        if (!m_Inventory || !m_Inventory.Contains(whatItem.ID))
        {
            message = $"{inworldCharacter.Name} does not have the item: {whatItem.ID}.";
            return false;
        }
        
        EntityItem whereItem = m_EntityItems["where"];
        
        if (!IsItemNearby(inworldCharacter, whereItem))
        {
            message = $"Item is too far away: {whereItem.ID}.";
            return false;
        }

        m_WhereItemCollider = whereItem.GetComponentInChildren<Collider>();
        if (!m_WhereItemCollider)
        {
            message = $"Item can not have objects placed on top of it: {whereItem.ID}.";
            return false;
        }
        
        return true;
    }

    public override IEnumerator Execute(InworldCharacter inworldCharacter)
    {
        EntityItem whatItem = m_EntityItems["what"];
        EntityItem whereItem = m_EntityItems["where"];
        
        Rigidbody whatItemRigidBody = whatItem.GetComponent<Rigidbody>();
        
        m_Inventory.RemoveItem(whatItem);
        whatItem.gameObject.SetActive(true);

        if(whatItemRigidBody)
            whatItemRigidBody.MovePosition(m_WhereItemCollider.bounds.center + new Vector3(0, m_WhereItemCollider.bounds.extents.y + m_ItemPlacementYOffset, 0));
        else
            whatItem.transform.position = m_WhereItemCollider.bounds.center + new Vector3(0, m_WhereItemCollider.bounds.extents.y + m_ItemPlacementYOffset, 0);

        InworldAI.Log($"{inworldCharacter.Name} placed item: {whatItem.DisplayName} on {whereItem.DisplayName}");
        whatItem.UpdateProperty("location", $"On {whereItem.DisplayName}.");
        Complete();
        yield break;
    }
}
