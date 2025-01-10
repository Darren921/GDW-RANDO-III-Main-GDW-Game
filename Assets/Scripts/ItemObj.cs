using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum ItemType
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

    public enum Attributes
    {
        Agility,
        Intellect,
        Stamina,
        Strength
    }

    public abstract class ItemObj : ScriptableObject
    {
        public int Id;
        public Sprite uiDisplay;
        public ItemType type;
        [TextArea(15, 20)] public string description;

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

        public Item(ItemObj item)
        {
            Name = item.name;
            Id = item.Id;

        }
    }


    
