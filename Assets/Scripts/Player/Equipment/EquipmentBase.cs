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
    protected SpawnManager _spawnManager;
    protected GameObject baseObject;
    protected internal abstract void CheckIfActive();
    
    protected internal abstract IEnumerator CheckCharge();

    public abstract void LimitCheck(GameObject other);

    public virtual void Start()
    {
        ID =  equipmentObj.data.Id ;
        _spawnManager = FindFirstObjectByType<SpawnManager>();
        baseObject = gameObject;
        if (equipmentObj is not null)
        {
            //Values can be changed in equipmentObj in items (inv system)
            MaxUses = equipmentObj.data.Limit;
            //change this after 
          //  CurrentUses =  0;
          CurrentUses = 3;
            RefillAmount = equipmentObj.refuel;
        }
    }

    protected GameObject FindChildWithNameContaining(Transform parent, string substring)
    {
        foreach (Transform child in parent)
        {
            if (child.name.Contains(substring))
            {
                return child.gameObject; 
            }
        }

        return null; 
    }

    
}

