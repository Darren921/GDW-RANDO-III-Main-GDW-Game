using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public abstract class EquipmentBase : MonoBehaviour
{
    [SerializeField]EquipmentObj equipmentObj;
    public float MaxLimit { get; private set; }
    internal GameObject baseObj;
    internal GameObject lightObj;
    protected float LimitLeft { get; set; }
    internal bool equipped;
    internal bool active;
    internal bool torchActive;
    internal bool checkActive;
    protected int refillAmount;
    private Slider slider;
    protected SpawnManager _spawnManager;
    internal int ID;
  
    // Start is called before the first frame update
    void Start()
    {
        ID =  equipmentObj.data.Id ;
        _spawnManager = FindFirstObjectByType<SpawnManager>();
        if (equipmentObj is not null)
        {
            //Values can be changed in equipmentObj in items (inv system)
            MaxLimit = equipmentObj.Limit;
            LimitLeft = equipmentObj.Limit;
            refillAmount = equipmentObj.refuel; 
        }
   
        
        baseObj = gameObject;
        lightObj = FindChildWithNameContaining(baseObj.transform, "Light");
        slider = gameObject.GetComponentInChildren<Slider>();
        baseObj.SetActive(false);
        lightObj.SetActive(false);  
    }
    
    // Update is called once per frame
    void Update()
    {
        slider.value = LimitLeft;
        slider.maxValue = MaxLimit;
        //if limit is greater than min value, limit left -- 
        if (LimitLeft > 0 && active)
        {
            LimitLeft -= Time.deltaTime;
        }
        
        if(LimitLeft < 0)
        {
            LimitLeft = 0;
            active = false;
            lightObj.SetActive(false);
        }
        
    }
    
    protected internal  abstract void  CheckIfActive();
    
    protected IEnumerator CheckCharge()
    {
            //
            yield return new WaitUntil(() => active == false );
            checkActive = false;
            torchActive = false;
    }


    public abstract void LimitCheck(GameObject other);
    
    
    //Helper class to find Child game objects using name (move to more global class?)
    public GameObject FindChildWithNameContaining(Transform parent, string substring)
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
