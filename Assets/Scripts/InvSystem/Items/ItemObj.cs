using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


    public enum ItemType
    {
        Empty,
        Traps,
        Consumable,
        Equipment,
    }

    public enum Attributes
    {
        Stun,
        Slow,
        Distraction,
        Throwable,
        CloseRange,
        LongRange,
    }

    public abstract class ItemObj : ScriptableObject
    {
        public int id;
        public Sprite uiDisplay;
        public ItemType itemType;
        [TextArea(15, 20)] public string description;
        public GameObject prefab;
        public int itemLimit;
        public float itemRarity;
        public Classification[] categories;
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
        [TextArea(15, 20)] public string Description;
        public Sprite UiDisplay;
        public ItemType ItemType;
        public GameObject Prefab;
        public int ItemLimit;
        public float ItemRarity;
        public Classification[] Categories;
        public Item(ItemObj item)
        {
            Name = item.name;
            Id = item.id;
            UiDisplay = item.uiDisplay;
            ItemType = item.itemType;
            Prefab = item.prefab;
            ItemLimit = item.itemLimit;
            ItemRarity = item.itemRarity;
            Categories = new Classification[item.categories.Length];
            for (int i = 0; i < item.categories.Length; i++)
            {
                Categories[i] = new Classification(item.categories[i].Attribute)
                {
                    Attribute = item.categories[i].Attribute
                };
            }
            Description = item.description;
        }
    }

    [System.Serializable]
    public class Classification 
    {
        public Attributes Attribute;

        public Classification(Attributes attribute)
        {
            Attribute = attribute;
        }
    }


    
