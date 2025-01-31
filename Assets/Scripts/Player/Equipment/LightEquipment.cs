using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightEquipment : EquipmentBase
{
    internal GameObject lightObj;
    internal bool active;
    internal bool checkActive;
    protected Slider slider;
    internal PlayerHotbar _playerHotbar;

    public override void Start()
    {
        base.Start(); // Calls EquipmentBase.Start()
        lightObj = FindChildWithNameContaining(baseObject.transform, "Light");
        lightObj.SetActive(false);
        slider = baseObject.GetComponentInChildren<Slider>();
        _playerHotbar = FindFirstObjectByType<PlayerHotbar>();
    }

    public virtual void Update()
    {
        slider.value = CurrentUses;
        slider.maxValue = MaxUses;
        //if limit is greater than min value, limit left -- 
        if (CurrentUses > 0 && active)
        {
            CurrentUses -= Time.deltaTime;
        }
        else if(CurrentUses <= 0 && active)
        {
            print(matchingItem.data.Name);
            switch (matchingItem.data.Name)
            {
                case "Fuel":
                    if (_playerHotbar.FuelCount > 0)
                    {
                        _playerHotbar.FuelCount--;
                        CurrentUses = RefillAmount;
                    }
                    else
                    {
                        CurrentUses = 0;
                        active = false;
                        lightObj.SetActive(false);
                    }
                    break;
                case "Battery":
                    if (_playerHotbar.batteryCount > 0)
                    {
                        _playerHotbar.batteryCount--;
                        CurrentUses = RefillAmount;
                    }
                    else
                    {
                        CurrentUses = 0;
                        active = false;
                        lightObj.SetActive(false);
                    }
                    break;
               
                 
            }
          
            
        }

     

    }


    protected internal override void CheckIfActive()
    {
        if (lightObj == null)
        {
            lightObj = gameObject.transform.Find("Light").gameObject;
        }
        print("working");
        if (equipped )
        {
            active = !active;
            if (CurrentUses > 0)
            {
                //if item is active, activate relevant systems 
                if (active && !checkActive)
                {
                    
                    active = true;
                    StartCoroutine(CheckCharge()) ;
                    lightObj.SetActive(true);
                    if (GetComponentInChildren<Collider>() != null)
                    {
                        GetComponentInChildren<Collider>().enabled = true;
                    }
                }
                else
                {
                  
                    active = false;
                    lightObj.SetActive(false);
                    if (GetComponentInChildren<Collider>() != null)
                    {
                        GetComponentInChildren<Collider>().enabled = true;
                    }
                    
                }
            }
            else
            {
                
                active = false;
                lightObj.SetActive(false);
                if (GetComponentInChildren<Collider>() != null)
                {
                    GetComponentInChildren<Collider>().enabled = true;
                };

            }
        }
    }
    
    
    protected internal override IEnumerator CheckCharge()
    {
        //
        yield return new WaitUntil(() => active == false );
        checkActive = false;
    }
    

    public override void LimitCheck(GameObject other)
    {
     
         
        
     
    }
    private void OnDisable()
    {
        active = false;
        checkActive = false;
    }
}
