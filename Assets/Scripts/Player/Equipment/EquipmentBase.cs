using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public abstract class EquipmentBase : MonoBehaviour
{
    protected internal int ID;
    internal bool equipped;
    internal float  MaxUses;
    internal float  CurrentUses;
    protected int RefillAmount;
    protected GameObject baseObj;
    protected PlayerHotbar _playerHotbar;
    protected PlayerInteraction _playerInteraction;

    protected virtual void Awake()
    {
        _playerInteraction = FindFirstObjectByType<PlayerInteraction>();
        _playerHotbar =FindFirstObjectByType<PlayerHotbar>();
        baseObj = gameObject;
    }
  

    internal abstract void HeldInteract();
   

    
    internal abstract void CheckIfUsable();
}

