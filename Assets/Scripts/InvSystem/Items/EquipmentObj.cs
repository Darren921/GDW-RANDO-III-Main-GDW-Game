using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Item ", menuName = "Inventory System/Items/Equipment")]

public class EquipmentObj : ItemObj
{
    public int Limit;
    // Start is called before the first frame update
    void Start()
    {
        itemType = ItemType.Equipment;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
