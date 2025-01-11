using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Test Item ", menuName = "Inventory System/Items/TestItem")]

public class TestItem : ItemObj
{
    private void Awake()
    {
        itemType = ItemType.Empty;
    }
}
