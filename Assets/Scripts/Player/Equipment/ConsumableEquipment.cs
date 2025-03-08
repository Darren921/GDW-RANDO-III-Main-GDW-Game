using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class  ConsumableEquipment : EquipmentBase
{
    [SerializeField] Consumables consumable;
    [SerializeField] EquipmentBase matchingEquipmentBase;
    
    // Start is called before the first frame update
    protected void Awake()
    {
        base.Awake();
        ID = consumable.data.Id;
        MaxUses = consumable.data.Limit;
    }

   
    // Update is called once per frame
    protected virtual void Update()
    {
        if (equipped)
        {
            for (int i = 0; i < _playerHotbar.Hotbar.Container.Slots.Length; i++)
            {
            
                if (_playerHotbar.Hotbar.Container.Slots[i].item.Id != matchingEquipmentBase.ID) continue;
                print(_playerHotbar.Hotbar.Container.Slots[i].amount);
                print(_playerHotbar.Hotbar.Container.Slots[i].item.Id);
                print(_playerHotbar.Hotbar.Container.Slots[i].item.Name);
                CurrentUses = _playerHotbar.Hotbar.Container.Slots[i].amount;
          
                return;
            }

        }
    }
    

    
}
