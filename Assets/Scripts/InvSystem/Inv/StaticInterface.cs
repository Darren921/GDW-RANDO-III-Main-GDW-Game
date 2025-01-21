    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticInterface : UserInterface
{
    public GameObject[] slots;
    protected override void CreateSlots()
    {
        itemsDisplayed = new Dictionary<GameObject,InventoryObj.InventorySlot>();
        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            var obj = slots[i];
        }
    }
}
