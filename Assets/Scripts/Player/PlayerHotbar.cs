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
    [SerializeField] internal TextMeshProUGUI  CountText;
    [SerializeField] internal Image DisplayImage;
    internal int batteryCount;
    internal int FuelCount;
    private EquipmentBase curEquipmentBase;

    public void ManageHotbar ()
    {
        isOpen = !isOpen;
    }

    private void Awake()
    {
        if (!GameManager.firstLoad)
        {
            GameManager.firstLoad = true;
            Hotbar.AddItem(new Item(EquipmentObjs[1]) , 1);
            Hotbar.AddItem(new Item(EquipmentObjs[2]) , 1); 
        }
           
        
         
        

    }

    public void SwapEquimpment(List<EquipmentBase> equipmentList, int equipmentIndex1, int equipmentIndex2)
    {
        // ReSharper disable once SwapViaDeconstruction
        var temp = equipmentList[equipmentIndex2];
        equipmentList[equipmentIndex2] = equipmentList[equipmentIndex1];
        equipmentList[equipmentIndex1] = temp;
        
    }
    
    internal bool isOpen { get; set; }
    private void Start()
    {
        var currentUsesInInt =  (int)_equipmentBases[returnTorchLocation()].CurrentUses; //Stores Current Uses in Torch as int 
        FuelCount = currentUsesInInt ; 
        foreach (var equipment in _equipmentBases)
        {
            equipment.gameObject.SetActive(false);
        }
        
//        print($"Forced selection: {_equipmentBases[1].gameObject.name}");

    }

    private void Update()
    {
        if (curEquipmentBase is null)
        {
            DisplayImage.gameObject.SetActive(false);
            CountText.gameObject.SetActive(false);
            return;
        }
        DisplayImage.gameObject.SetActive(true);
        CountText.gameObject.SetActive(true);
//        print(DisplayImage.sprite);
//        print(curEquipmentBase.equipmentObj.data.UiDisplay);
        DisplayImage.sprite = curEquipmentBase.allClasses.equipmentObj.data.UiDisplay;
        if (curEquipmentBase.ID == _equipmentBases[returnTorchLocation()].ID)
        {
          
            CountText.text = $"X {FuelCount.ToString()}";
        }
        else if (curEquipmentBase.ID == _equipmentBases[1].ID)
        {
            CountText.text = $"X {batteryCount.ToString()}";
        }


    }

    private void OnApplicationQuit()
    {
        Hotbar.Container.Clear();
    }

    public void ChangeItem(float slot)
    {
        var inputtedSlot = (int)slot - 1;

        for (var i = 0; i < Hotbar.Container.Slots.Length; i++)
        {
            var outline = Hotbar.Container.Slots[i].slotDisplay.transform.GetChild(0).GetComponentInChildren<Outline>();
            outline.enabled = Hotbar.Container.Slots[i] == Hotbar.Container.Slots[inputtedSlot];
        }

        inputtedSlot = (int)slot;

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
        if (inputtedSlot < _equipmentBases.Count)
        {
            print(Hotbar.Container.Slots[inputtedSlot - 1].item.Id);
            print(_equipmentBases[inputtedSlot].ID);
            if (Hotbar.Container.Slots[inputtedSlot - 1].item.Id == _equipmentBases[inputtedSlot].ID)
            {
                //print("Equipped Normal");
                _equipmentBases[inputtedSlot].gameObject.SetActive(true);
                curEquipmentBase = _equipmentBases[inputtedSlot];
                var lightEquipment = curEquipmentBase.GetComponent<LightEquipment>();
    
                if (lightEquipment != null)
                {
                    lightEquipment.equipped = true;
                } 
            }
            else
            {
                var Target = 0;
                print(Target);
                _equipmentBases[Target].gameObject.SetActive(true);
                var lightEquipment = _equipmentBases[Target].GetComponent<LightEquipment>();
    
                if (lightEquipment != null)
                {
                    lightEquipment.equipped = true;
                } 


             
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
