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

        public bool stackable;
        [TextArea(15, 20)] public string description;
        public GameObject prefab;
        
        public Item data = new Item();
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
        public int Id = -1;
        [TextArea(15, 20)] public string Description;
        public Sprite UiDisplay;
        public ItemType ItemType;
        public GameObject Prefab;
        public int ItemSpawnLimit;
        public int Limit;
        public float ItemRarity;
        public Classification[] Categories;

        public Item()
        {
            Name = "";
            Id = -1;
        }
        public Item(ItemObj item)
        {
            Name = item.name;
            UiDisplay = item.data.UiDisplay;
            Prefab = item.prefab;
            ItemSpawnLimit = item.data.ItemSpawnLimit;
            ItemRarity = item.data.ItemRarity;
            Limit = item.data.Limit;
            Id = item.data.Id;
            Categories = new Classification[item.data.Categories.Length];
            for (int i = 0; i < item.data.Categories.Length; i++)
            {
                Categories[i] = new Classification(item.data.Categories[i].Attribute)
                {
                    Attribute = item.data.Categories[i].Attribute
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


    
