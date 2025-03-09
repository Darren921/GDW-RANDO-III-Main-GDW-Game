using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class LightEquipment : EquipmentBase
{
    [SerializeField] EquipmentObj equipmentObj;
    protected GameObject lightSource;
    protected Slider slider;
    internal bool active;

    protected override void Awake()
    {
        base.Awake();
        ID = equipmentObj.data.Id;
        MaxUses = equipmentObj.data.Limit;
        CurrentUses = 0;
        RefillAmount = equipmentObj.refuel;
        //Values can be changed in equipmentObj in items (inv system)
      
        lightSource = gameObject.transform.Find("LightSource").gameObject;
        lightSource.SetActive(false);
        slider = baseObj.GetComponentInChildren<Slider>();
    }

    protected virtual void Update()
    {
        print(CurrentUses);
        slider.value = CurrentUses;
        slider.maxValue = MaxUses;
        if (CurrentUses <= 0 && active )
        {
            if (equipmentObj.data.Id == 1)
            {
                if (_playerHotbar.batteryCount > 0)
                { 
                    _playerHotbar.batteryCount--;
                    CurrentUses = RefillAmount;
                }
                else
                {
                    CurrentUses = 0;
                    lightSource.SetActive(false);
                }
            }
        
                    
        }
    }
}