using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Inv", menuName = "Inventory System/Inventory")]
public class InventoryObj : ScriptableObject 
{
    public string savePath;
    public ItemDatabaseObject database;
    public Inventory Container;
    public InventorySlot[] GetSlots
    {
        get { return Container.Slots; }
    }

 
    public bool AddItem(Item _item, int _amount)
    {
        if (EmptySlotCount <= 0)
        {
            return false;
        }

        var slot = FindItemOnInventory(_item);
        if (!database.ItemObjects[_item.Id].stackable || slot == null)
        {
            SetEmptySlot(_item, _amount);
            return true;
        }
        if (slot.amount + _amount > slot.item.Limit) return false;
        slot.addAmount(_amount);
        return true;
    }

    public void RemoveItem(Item _item, int _amount, int slotnumber)
    {

        for (var i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i] != GetSlots[slotnumber]) continue;
            Debug.Log(i + slotnumber);
            Debug.Log(GetSlots[slotnumber].amount - _amount);
            if (GetSlots[i].amount - _amount > 0)
            {
                Debug.Log("RemoveAmount");
                GetSlots[i].removeAmount(_amount);
            }
            else
            {
                Debug.Log("RemoveSlot");

                GetSlots[i].RemoveSlot();
            }
        }
    }
 
    
    public int EmptySlotCount
    {
        get
        {
            var counter = 0;
            for (var i = 0; i < GetSlots.Length; i++)
            {
                if (GetSlots[i].item.Id <= -1)
                {
                    counter++;
                }   
            }
            return counter;
        }
    }
    private InventorySlot FindItemOnInventory(Item _item)
    {
        for (var i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id == _item.Id)
            {
                return GetSlots[i];
            }
        }

        return null;
    }


    private InventorySlot SetEmptySlot(Item _item, int _amount)
    {
        for (var i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].item.Id <= -1)
            {
                GetSlots[i].UpdateSlot( _item ,_amount);
                return GetSlots[i];
            }
        }
        //set up fiuctionaly when inv full
        return null;
    }

    public void SwapItems(InventorySlot item1, InventorySlot item2)
    {
        var temp = new InventorySlot( item2.item, item2.amount);
        item2.UpdateSlot( item1.item, item1.amount);
         item1.UpdateSlot( temp.item, temp.amount);
         
    }
    [ContextMenu("Save")]
    public void Save()
    {
        var fullPath = Path.Combine(Application.persistentDataPath, savePath);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
            var saveData = JsonUtility.ToJson(this, true);
            using var stream = new FileStream(fullPath, FileMode.Create);
            using var writer = new StreamWriter(stream);
            writer.Write(saveData);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    [ContextMenu("Load")]
    public void Load()
    {
        var fullPath = Path.Combine(Application.persistentDataPath, savePath);
        if (File.Exists(fullPath))
        {
            try
            {
                var dataToload = "";
                using (var stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        dataToload = reader.ReadToEnd();
                    }
                }
                JsonUtility.FromJsonOverwrite(dataToload, this);
                foreach (var slot in Container.Slots)
                {
                    slot.UpdateSlot( slot.item, slot.amount);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
    [ContextMenu("Clear")]
    public void Clear()
    {
       Container.Clear();
    }

    [System.Serializable]
    public class Inventory
    {
        public InventorySlot[] Slots = new InventorySlot[4];

        public void Clear()
        {
            for (int i = 0; i < Slots.Length; i++)
            {
                Slots[i].UpdateSlot(new Item(), 0);
            }
        }
    }

    public delegate void SlotUpdated (InventorySlot slot);
    [System.Serializable]
    public class InventorySlot
    {
        [System.NonSerialized]
        public GameObject slotDisplay;
        [System.NonSerialized]
        public UserInterface parent;
        [System.NonSerialized] public SlotUpdated OnBeforeUpdate;
        [System.NonSerialized] public SlotUpdated OnAfterUpdate;

        
        public Item item;
        public int amount;

        public ItemObj ItemObj
        {
            get
            {
                if (item.Id >= 0)
                {
                    return parent.inventory.database.ItemObjects[item.Id];
                }

                return null;
                
            }
        }
        public InventorySlot()
        {
            UpdateSlot(new Item(), 0);
        }

        public InventorySlot(Item _item, int _amount )
        {
            UpdateSlot(_item, _amount);
          
        }
        public void UpdateSlot( Item _item, int _amount )
        {
            OnBeforeUpdate?.Invoke(this);
            item = _item;
            amount = _amount;
            OnAfterUpdate?.Invoke(this);
        }
       
      
        public void addAmount(int value)
        {
            UpdateSlot(item , amount += value);
        }
        public void removeAmount(int value)
        {
            UpdateSlot(item , amount -= value);
        }
        public void RemoveSlot()
        {
            OnBeforeUpdate?.Invoke(this);
            item.Id= -1;
            item = new Item();
            item.UiDisplay = null;
            amount = 0;
            OnAfterUpdate?.Invoke(this);
        }
      

       

       
    }

    
    
}
