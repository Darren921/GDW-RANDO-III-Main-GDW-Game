using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


    public enum ItemTypes
    {
        Empty,
        Equipment,
        Traps,
        CloseRange,
        LongRange,
        Consumable,
        Throwable,
    }

    public enum Attributes
    {
        Stun,
        Distraction,
        Slow,
        Strength
    }

    public abstract class ItemObj : ScriptableObject
    {
        public int Id;
        public Sprite uiDisplay;
        public ItemTypes ItemType;
        [TextArea(15, 20)] public string description;
        public GameObject thisGameObject;
        public int itemLimit;
        public Item CreateItem()
        {
            Item newItem = new Item(this);
            return newItem;
        }
    }

    [System.Serializable]
    public class Item
    {
        public string Name;
        public int Id;
        public Sprite icon;
        public ItemTypes itemType;
        public GameObject thisGameobject;
        public int ItemLimit;
        public Item(ItemObj item)
        {
            Name = item.name;
            Id = item.Id;
            icon = item.uiDisplay;
            itemType = item.ItemType;
            thisGameobject = item.thisGameObject;
            ItemLimit = item.itemLimit;
            
        }
    }


    
