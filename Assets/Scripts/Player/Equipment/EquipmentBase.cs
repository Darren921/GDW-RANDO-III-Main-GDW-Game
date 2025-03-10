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
    protected PlayerMovement _playerMovement;


    protected virtual void Awake()
    {
        _playerInteraction = FindFirstObjectByType<PlayerInteraction>();
        _playerMovement = FindFirstObjectByType<PlayerMovement>();
        _playerHotbar =FindFirstObjectByType<PlayerHotbar>();
        baseObj = gameObject;
    }
  

    internal abstract void HeldInteract();
   

    
    internal abstract void CheckIfUsable();
    internal abstract void CheckHeldInteract();
}

