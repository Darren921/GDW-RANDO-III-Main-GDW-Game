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

   public delegate void InventoryChanged();

    private void Awake()
    {

        if (PlayerPrefs.GetInt("StartingItemsGiven") == 0)
        {
            PlayerPrefs.GetInt("StartingItemsGiven", 1);
            Hotbar.AddItem(new Item(EquipmentObjs[1]), 1);
            Hotbar.AddItem(new Item(EquipmentObjs[2]) , 1); 
            PlayerPrefs.Save();
     
        }

    }

 
    
    internal bool isOpen { get; set; }
    private void Start()
    {
        var currentUsesInInt =  (int)_equipmentBases[returnTorchLocation()].CurrentUses; //Stores Current Uses in Torch as int 
        FuelCount = currentUsesInInt ; 
        CurrentEquipped = -1;
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

   

        print(inputtedSlot);

        
        for (var i = 1; i < _equipmentBases.Count; i++)
        {
            _equipmentBases[i].gameObject.SetActive(false);
            var lightEquipment = _equipmentBases[i].GetComponent<LightEquipment>();
            if (lightEquipment != null)
            {
                lightEquipment.equipped = false;
            }
        }
        
        var Target = Hotbar.Container.Slots[inputtedSlot].item.Id;
        
        var TargetIndex = _equipmentBases.FindIndex(item => item.ID == Target);

        if (TargetIndex != -1)
        {
            print(TargetIndex);
            _equipmentBases[TargetIndex].gameObject.SetActive(true);
            var lightEquipment = _equipmentBases[TargetIndex].GetComponent<LightEquipment>();
    
            if (lightEquipment != null)
            {
                lightEquipment.equipped = true;
            } 

        }
        for (var i = 0; i < Hotbar.Container.Slots.Length; i++)
        {
            var outline = Hotbar.Container.Slots[i].slotDisplay.transform.GetChild(0).GetComponentInChildren<Outline>();
            if (Hotbar.Container.Slots[i] == Hotbar.Container.Slots[inputtedSlot])
            {
                outline.enabled = true;
            }
            outline.enabled = Hotbar.Container.Slots[i] == Hotbar.Container.Slots[inputtedSlot];
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
