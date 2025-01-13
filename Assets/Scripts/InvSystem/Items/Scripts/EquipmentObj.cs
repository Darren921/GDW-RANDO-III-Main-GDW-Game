using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Item ", menuName = "Inventory System/Items/Equipment", order = 1)]

public class EquipmentObj : ItemObj
{
    public float MaxHeld;
    // Start is called before the first frame update
    void Awake()
    {
        ItemType = ItemTypes.Equipment;
    }

 
}
