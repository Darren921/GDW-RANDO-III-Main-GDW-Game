using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public abstract class EquipmentBase : MonoBehaviour
{
    [SerializeField] internal EquipmentObj equipmentObj;
    [SerializeField] protected ItemObj matchingItem;
    protected internal int ID;
    internal bool equipped;
    internal float  MaxUses;
    internal float  CurrentUses;
    protected int RefillAmount;
    protected GameObject baseObj;
    protected PlayerHotbar _playerHotbar;
    protected virtual void Awake()
    {
        _playerHotbar =FindFirstObjectByType<PlayerHotbar>();
        ID = equipmentObj.data.Id;
        baseObj = gameObject;
        if (equipmentObj is not null)
        {
            //Values can be changed in equipmentObj in items (inv system)
            MaxUses = equipmentObj.data.Limit;
            CurrentUses =  0;
            RefillAmount = equipmentObj.refuel;
        }
    }

    public abstract void CheckIfUsable();
}

