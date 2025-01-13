using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Test Item ", menuName = "Inventory System/Items/TestItem",order = 0)]

public class TestItemObj : ItemObj
{
    private void Awake()
    {
        ItemType = ItemTypes.Empty;
    }
}
