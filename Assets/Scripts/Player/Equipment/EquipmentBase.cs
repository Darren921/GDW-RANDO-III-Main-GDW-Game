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
  
    protected void RemoveEquipmentBase()
    {
        if (_playerHotbar._equipmentBases[_playerHotbar.inputtedSlot -1] != null)
        {
            if (_playerHotbar._equipmentBases[_playerHotbar.inputtedSlot ] != _playerHotbar.emptyEquipmentBase)
            {
                _playerHotbar._equipmentBases.Remove(_playerHotbar.curEquipmentBase);
                _playerHotbar._equipmentBases.Insert(_playerHotbar.inputtedSlot , _playerHotbar.emptyEquipmentBase);
            }
            else
            {
                _playerHotbar._equipmentBases.RemoveAt(_playerHotbar.inputtedSlot - 1);
            }
                       
        }

        _playerHotbar.ChangeItem(_playerHotbar.inputtedSlot );
    }

    internal abstract void HeldInteract();
   

    
    internal abstract void CheckIfUsable();
}

