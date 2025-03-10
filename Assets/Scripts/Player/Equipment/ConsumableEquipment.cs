using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class  ConsumableEquipment : EquipmentBase
{
    [SerializeField] Consumables consumable;
    [SerializeField] EquipmentBase matchingEquipmentBase;
    
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        ID = consumable.data.Id;
        MaxUses = consumable.data.Limit;
    }
    protected abstract void ActivateEffect();
   
    // Update is called once per frame
    protected virtual void Update()
    {
        if (equipped)
        {
            for (int i = 0; i < _playerHotbar.Hotbar.Container.Slots.Length; i++)
            {
            
                if (_playerHotbar.Hotbar.Container.Slots[i].item.Id != matchingEquipmentBase.ID) continue;
                CurrentUses = _playerHotbar.Hotbar.Container.Slots[i].amount;
                return;
            }

        }
    }
    

    
}
