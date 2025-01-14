using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Consumable Item ", menuName = "Inventory System/Items/Consumable",order = 2)]
public class ConsumablesObj : ItemObj
{
    private void Awake()
    {
        itemType = ItemTypes.Consumable;
    }

}
