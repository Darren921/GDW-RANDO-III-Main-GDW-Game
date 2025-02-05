using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHotbar : MonoBehaviour
{    
    [SerializeField] internal InventoryObj Hotbar;
    [SerializeField] internal List<EquipmentBase>  _equipmentBases;
    [SerializeField] private ItemObj[] EquipmentObjs;
    internal int batteryCount;
    internal int FuelCount;
    private int CurrentItem;
    public void ManageHotbar ()
    {
        isOpen = !isOpen;
    }

    internal bool isOpen { get; set; }
    private void Start()
    {
        CurrentItem = -1;

        if (!GameManager.firstLoad)
        {
            GameManager.firstLoad  = true;
            Hotbar.AddItem(new Item(EquipmentObjs[1]) , 1);
            Hotbar.AddItem(new Item(EquipmentObjs[2]) , 1);
        }

        foreach (var equipment in _equipmentBases)
        {
            equipment.gameObject.SetActive(false);
        }
        
    }

    private void OnApplicationQuit()
    {
        Hotbar.Container.Clear();
    }

    public void ChangeItem(float slot)
    {
      //  print (slot + " change");
        int inputtedSlot = (int)slot - 1 ;
        var targetSlot = Hotbar.Container.Slots[inputtedSlot]; // Get selected slot
      //  print(Hotbar.Container.Slots[inputtedSlot]);
        if(targetSlot.ItemObj == null) return;
        var targetId = targetSlot.ItemObj.data.Id; // Get item ID
    //    print(targetId);
        // Disable outlines for all slots
        foreach (var slots in Hotbar.Container.Slots)
        {
            var outline = slots.slotDisplay.transform.GetChild(0).gameObject.GetComponentInChildren<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
            }
        }
            
        // Activate/deactivate equipment based on selected item ID
        for (int i = 0; i < _equipmentBases.Count; i++)
        {
            var item = _equipmentBases[i];
            bool isActive = (item.ID == targetId);
          //  print (item.ID);
          //  print (targetId);
          //  print(isActive);
            item.gameObject.SetActive(isActive);
      
            if (item.GetComponent<LightEquipment>() != null)
            {
                if (targetId >= 1)
                {
                    var lightEquipment = item.GetComponent<LightEquipment>();
                    lightEquipment.lightObj?.gameObject?.SetActive(false);
                    lightEquipment.active = false;
                }
                else
                {
                    item.gameObject.SetActive(false);

                }
            }
        
            item.equipped = isActive;
        
            if (isActive)
            {
                CurrentItem = targetId;
            }
        }

        var selectedOutline = targetSlot.slotDisplay.transform.GetChild(0).gameObject.GetComponentInChildren<Outline>();
        if (selectedOutline != null)
        {
            selectedOutline.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
    }

    public void checkIfActive()
    {
        for (int i = 0; i < _equipmentBases.Count; i++)
        {
         
            if (_equipmentBases[i].equipped )
            {
                _equipmentBases[i].CheckIfActive();
            }
        }
    }
    
    public int returnTorchLocation()
    {
        for (int i = 0; i < _equipmentBases.Count; i++)
        {
            if (_equipmentBases[i].name.Contains("Torch"))
            {
//                print(i);
                return i;
            }
        }
        return -1;
    }
}
