using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public abstract class EquipmentBase : MonoBehaviour
{
    [SerializeField] protected ItemObj matchingItem;
    protected internal int ID;
    internal bool equipped;
    internal float  MaxUses;
    internal float  CurrentUses;
    protected int RefillAmount;
    protected GameObject baseObj;
    protected PlayerHotbar _playerHotbar;

    protected void Awake()
    {
        _playerHotbar =FindFirstObjectByType<PlayerHotbar>();
        baseObj = gameObject;
    }
    public struct AllClasses
    {
        public EquipmentObj equipmentObj;
    }

  

    
    public abstract void CheckIfUsable();
}

