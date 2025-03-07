using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class LightEquipment : EquipmentBase
{
    [SerializeField] EquipmentObj equipmentObj;
    protected GameObject light;
    protected Slider slider;
    internal bool active;

    protected virtual void Awake()
    {
        base.Awake();
        ID = equipmentObj.data.Id;
        //Values can be changed in equipmentObj in items (inv system)
      
        light = gameObject.transform.Find("LightSource").gameObject;
        light.SetActive(false);
        slider = baseObj.GetComponentInChildren<Slider>();
    }

    protected virtual void Update()
    {
        print(CurrentUses);
        slider.value = CurrentUses;
        slider.maxValue = MaxUses;
        if (CurrentUses <= 0 && active)
        {
            switch (matchingItem.data.Name)
            {
                case "Battery":
                    if (_playerHotbar.batteryCount > 0)
                    {
                        _playerHotbar.batteryCount--;
                        CurrentUses = RefillAmount;
                    }
                    else
                    {
                        CurrentUses = 0;
                        light.SetActive(false);
                    }

                    break;
            }
        }
    }
}