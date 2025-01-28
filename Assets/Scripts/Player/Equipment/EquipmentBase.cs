using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public abstract class EquipmentBase : MonoBehaviour
{
    [SerializeField] protected EquipmentObj equipmentObj;
    [SerializeField] protected ItemObj matchingItem;
    protected internal int ID;
    internal bool equipped;
    protected float  MaxUses;
    protected float  CurrentUses;
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
            MaxUses = equipmentObj.Limit;
            CurrentUses = equipmentObj.Limit;
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

