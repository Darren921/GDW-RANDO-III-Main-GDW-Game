using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


    public enum ItemType
    {
        Empty,
        Stun,
        Traps,
        Slow,
        Distraction,
        CloseRange,
        LongRange,
        Consumable,
        Throwable,
    }

    public enum Attributes
    {
        Agility,
        Intellect,
        Stamina,
        Strength
    }

    public abstract class ItemObj : ScriptableObject
    {
        public int id;
        public Sprite uiDisplay;
        public ItemType itemType;
        [TextArea(15, 20)] public string description;
        public GameObject thisGameObject;
        public int itemLimit;
        public float itemRarity;
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
        public ItemType ItemType;
        public GameObject ThisGameobject;
        public int ItemLimit;
        public float ItemRarity;
        public Item(ItemObj item)
        {
            Name = item.name;
            Id = item.id;
            icon = item.uiDisplay;
            ItemType = item.itemType;
            ThisGameobject = item.thisGameObject;
            ItemLimit = item.itemLimit;
            ItemRarity = item.itemRarity;
        }
    }


    
