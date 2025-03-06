using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public abstract class EquipmentBase : MonoBehaviour
{
    internal AllClasses allClasses;
    [SerializeField] protected ItemObj matchingItem;
    protected internal int ID;
    internal bool equipped;
    internal float  MaxUses;
    internal float  CurrentUses;
    protected int RefillAmount;
    protected GameObject baseObj;
    protected PlayerHotbar _playerHotbar;

    public struct AllClasses
    {
        internal EquipmentObj equipmentObj;
    }

  

    
    public abstract void CheckIfUsable();
}

