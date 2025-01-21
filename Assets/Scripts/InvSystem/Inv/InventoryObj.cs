using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inv", menuName = "Inventory System/Inventory")]
public class InventoryObj : ScriptableObject 
{
    public string savePath;
    public ItemDatabaseObject database;
    public Inventory Container;

 
    public void AddItem(Item _item, int _amount)
    {
        if (_item.Categories.Length > 0)
        {
           SetEmptySlot(_item, _amount);
            return;
        }
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].ID == _item.Id)
            {
                Container.Items[i].addAmount(_amount);
             return;
            }
        }
        SetEmptySlot(_item, _amount);
    }

    public InventorySlot SetEmptySlot(Item _item, int _amount)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].ID <= -1)
            {
                Container.Items[i].UpdateSlot(_item.Id, _item ,_amount);
                return Container.Items[i];
            }
        }
        //set up fiuctionaly when inv full
        return null;
    }

    public void MoveItem(InventorySlot item1, InventorySlot item2)
    {
        InventorySlot temp = new InventorySlot(item2.ID, item2.item, item2.amount);
        item2.UpdateSlot(item1.ID, item1.item, item1.amount);
        item1.UpdateSlot(temp.ID, temp.item, temp.amount);
    }
    [ContextMenu("Save")]
    public void Save()
    {
       string saveData = JsonUtility.ToJson(this, true);
       BinaryFormatter bf = new BinaryFormatter();
       FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
       bf.Serialize(file,saveData);
       file.Close();
    }
    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
        }
    }
    [ContextMenu("Clear")]
    public void Clear()
    {
       Container = new Inventory();
    }

    [System.Serializable]
    public class Inventory
    {
        public InventorySlot[] Items = new InventorySlot[4];
    }


    [System.Serializable]
    public class InventorySlot
    {
        public int ID = -1;
        public Item item;
        public int amount;
        public InventorySlot()
        {
            ID = -1;
            item = null;
            amount = 0;
        }

        public InventorySlot(int _id, Item _item, int _amount )
        {
            ID = _id;
            item = _item;
            amount = _amount;
        }
        public void UpdateSlot(int _id, Item _item, int _amount )
        {
            ID = _id;
            item = _item;
            amount = _amount;
        }
      
        public void addAmount(int value)
        {
            amount += value;
        }
    }

    
    
}
