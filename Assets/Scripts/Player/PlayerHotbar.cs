using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHotbar : MonoBehaviour
{    
    [SerializeField] internal InventoryObj Hotbar;
    [SerializeField] internal List<EquipmentBase>  _equipmentBases;
    [SerializeField] private ItemObj[] EquipmentObjs;
    [SerializeField] internal TextMeshProUGUI FuelCountText, BatteryCountText;

    internal int batteryCount;
    internal int FuelCount;
    internal int CurrentEquipped;
    public void ManageHotbar ()
    {
        isOpen = !isOpen;
    }

    internal bool isOpen { get; set; }
    private void Start()
    {
        var pain =  (int)_equipmentBases[returnTorchLocation()].CurrentUses;
        FuelCount = pain ;
        CurrentEquipped = -1;
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
        
        print($"Forced selection: {_equipmentBases[1].gameObject.name}");

    }

    private void Update()
    {
        BatteryCountText.text = $"Battery count {batteryCount} / 3";
        FuelCountText.text = $"Fuel count \n {FuelCount} / 3";

    }

    private void OnApplicationQuit()
    {
        Hotbar.Container.Clear();
    }

    public void ChangeItem(float slot)
    {
        var inputtedSlot = (int)slot - 1;

        for (var i = 1; i < Hotbar.Container.Slots.Length; i++)
        {
            var outline = Hotbar.Container.Slots[i].slotDisplay.transform.GetChild(0).GetComponentInChildren<Outline>();
            if (Hotbar.Container.Slots[i] != Hotbar.Container.Slots[inputtedSlot])
            {
                outline.enabled = false;
            }
            else
            {
                outline.enabled = true;
            }
        }

        inputtedSlot = (int)slot;

        print(inputtedSlot);
        for (var i = 0; i < _equipmentBases.Count; i++)
        {
            _equipmentBases[i].gameObject.SetActive(false);
            var lightEquipment = _equipmentBases[i].GetComponent<LightEquipment>();
            if (lightEquipment != null)
            {
                lightEquipment.equipped = false;
         //       lightEquipment.lightObj.gameObject.SetActive(false);
            }
        }
        if (inputtedSlot < _equipmentBases.Count)
        {
            _equipmentBases[inputtedSlot].gameObject.SetActive(true);
            var curWeapon = _equipmentBases[inputtedSlot];
            var lightEquipment = curWeapon.GetComponent<LightEquipment>();
    
            if (lightEquipment != null)
            {
                lightEquipment.equipped = true;
       //         lightEquipment.lightObj.gameObject.SetActive(false); 
            }
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
                _equipmentBases[i].CheckIfUsable();
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
