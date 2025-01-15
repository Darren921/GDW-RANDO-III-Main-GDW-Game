using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    enum ItemType
    {
        Stun,
        Traps,
        Slow,
        Distraction,
        CloseRange,
        LongRange,
        Consumable,
        Throwable,
    }

    public class Item
    {
        ItemType type;
        public int amount;
        public int maxAmount;
        public string itemName;
        public string itemDescription;
    }
  
}
