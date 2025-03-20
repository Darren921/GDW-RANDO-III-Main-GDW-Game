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
    [SerializeField] internal List<EquipmentBase> _equipmentBases;
    [SerializeField] private ItemObj[] EquipmentObjs;
    [SerializeField] internal TextMeshProUGUI CountText;
    [SerializeField] internal Image DisplayImage;
    internal int batteryCount;
    internal float FuelCount;
    internal EquipmentBase curEquipmentBase;
    [SerializeField]internal EquipmentBase emptyEquipmentBase;
    internal int inputtedSlot;

    public void ManageHotbar()
    {
        isOpen = !isOpen;
    }

    private void Awake()
    {
       





    }



    internal bool isOpen { get; set; }

    private void Start()
    {
        if (!GameManager.firstLoad)
        {
            GameManager.firstLoad = true;
            Hotbar.AddItem(new Item(EquipmentObjs[1]), 1);
            Hotbar.AddItem(new Item(EquipmentObjs[2]), 1);
        }
    
        FuelCount = _equipmentBases[returnTorchLocation()].CurrentUses;
       

//        print($"Forced selection: {_equipmentBases[1].gameObject.name}");
        var equipments = FindObjectsOfType<EquipmentBase>();
        foreach (var equipment in equipments)
        {
            equipment.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (curEquipmentBase is null)
        {
            DisplayImage.gameObject.SetActive(false);
            CountText.gameObject.SetActive(false);
            return;
        }

//        print(DisplayImage.sprite);
//        print(curEquipmentBase.equipmentObj.data.UiDisplay);


        if (curEquipmentBase.ID == _equipmentBases[returnTorchLocation()].ID)
        {

            CountText.text = $"X {FuelCount.ToString()}";
        }
        else if (curEquipmentBase.ID == _equipmentBases[1].ID)
        {
            CountText.text = $"X {batteryCount.ToString()}";
        }
        else
        {
            CountText.text = $"X {Hotbar.Container.Slots[inputtedSlot - 1].amount.ToString()}";
        }
    


}

    private void OnApplicationQuit()
    {
        Hotbar.Container.Clear();
    }

    public void ChangeItem(float slot)
    {
         inputtedSlot = (int)slot - 1;

         DisplayImage.gameObject.SetActive(true);
         CountText.gameObject.SetActive(true);
        for (var i = 0; i < Hotbar.Container.Slots.Length; i++)
        {
            var outline = Hotbar.Container.Slots[i].slotDisplay.transform.GetChild(0).GetComponentInChildren<Outline>();
            outline.enabled = Hotbar.Container.Slots[i] == Hotbar.Container.Slots[inputtedSlot];
        }

        inputtedSlot = (int)slot;

////        print(inputtedSlot);
        for (var i = 1; i < _equipmentBases.Count; i++)
        {
            _equipmentBases[i].gameObject.SetActive(false);
            var lightEquipment = _equipmentBases?[i].GetComponent<LightEquipment>();
            var UsableEquipment = _equipmentBases?[i].GetComponent<ConsumableEquipment>();

            if (lightEquipment != null)
            {
                lightEquipment.equipped = false;
            }
            else if (UsableEquipment != null)
            {
                UsableEquipment.equipped = false;
            }
        }
        if (inputtedSlot < _equipmentBases.Count)
        {
//            print("in log");
//            print(Hotbar.Container.Slots[inputtedSlot - 1].item.Id);
//            print(_equipmentBases[inputtedSlot].ID);
            if (Hotbar.Container.Slots[inputtedSlot - 1].item.Id == _equipmentBases[inputtedSlot].ID)
            {
//                print(Hotbar.Container.Slots[inputtedSlot - 1].item.Id);
                if (Hotbar.Container.Slots[inputtedSlot - 1].item == null)
                {
                    DisplayImage.gameObject.SetActive(false);
                    CountText.gameObject.SetActive(false);
                }
                if ( Hotbar.Container.Slots[inputtedSlot - 1].item.MatchingItemObj != null)
                {
                    DisplayImage.sprite = Hotbar.Container.Slots[inputtedSlot - 1].item.MatchingItemObj.data.UiDisplay;
                }
                else
                {
                    DisplayImage.sprite = Hotbar.Container.Slots[inputtedSlot - 1].item.UiDisplay;
                }
             
                
                   
              
                //print("Equipped Normal");
                _equipmentBases[inputtedSlot].gameObject.SetActive(true);
                curEquipmentBase = _equipmentBases[inputtedSlot];
                var lightEquipment = curEquipmentBase?.GetComponent<LightEquipment>();
                var UsableEquipment = curEquipmentBase?.GetComponent<ConsumableEquipment>();
                if (lightEquipment != null)
                {
                    lightEquipment.equipped = true;
                } 
                else if (UsableEquipment != null)
                {
                    UsableEquipment.equipped = true;
                }
            }
            else
            {
                var Target = 0;
              //  print(Target);
                _equipmentBases[Target].gameObject.SetActive(true);
                var lightEquipment = _equipmentBases?[Target].GetComponent<LightEquipment>();
                var UsableEquipment = _equipmentBases?[Target].GetComponent<ConsumableEquipment>();

                if (lightEquipment != null)
                {
                    lightEquipment.equipped = true;
                } 
                else if (UsableEquipment != null)
                {
                    UsableEquipment.equipped = true;
                }


             
            }
        }

        if (Hotbar.Container.Slots[inputtedSlot - 1].item.Id != -1) return;
        print("out log");
        DisplayImage.gameObject.SetActive(false);
        CountText.gameObject.SetActive(false);





    }

    private void OnDisable()
    {
        Hotbar.Container.Clear();
        GameManager.firstLoad = false;
    }

    private void OnTriggerEnter(Collider other)
    {
    }

    public void checkIfActive()
    {
        for (var i = 0; i < _equipmentBases.Count; i++)
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
