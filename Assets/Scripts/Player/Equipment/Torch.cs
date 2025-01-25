using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Torch : EquipmentBase
{
    private EquipmentBase _equipmentBaseImplementation;
    [SerializeField] private ItemObj matchingItem;
    
    
    
    
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

        if (GetComponent<Collider>() is not null)
        {
            GetComponent<Collider>().enabled = false;
        }
        baseObj = gameObject;
        lightObj = FindChildWithNameContaining(baseObj.transform, "Light");
        slider = gameObject.GetComponentInChildren<Slider>();
        baseObj.SetActive(false);
        lightObj.SetActive(false);  
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
            if (LimitLeft > 0)
            {
                //if item is active, activate relevant systems 
                if (active && !checkActive)
                {
                    
                    torchActive = true;
                    active = true;
                    StartCoroutine(CheckCharge()) ;
                    lightObj.SetActive(true);
                    GetComponent<Collider>().enabled = true;
                }
                else
                {
                  
                    torchActive = false;
                    active = false;
                    lightObj.SetActive(false);
                    GetComponent<Collider>().enabled = false;
                    
                }
            }
            else
            {
                
                torchActive = false;
                active = false;
                lightObj.SetActive(false);
                GetComponent<Collider>().enabled = false;

            }
        }
    }

    public override void LimitCheck(GameObject other)
    {
        //add limit as necessary to cur amount (ground item pickup)
     
        if (LimitLeft < MaxLimit && other.CompareTag(matchingItem.name))
        {
            var tracker = other.GetComponent<GroundObj>().tracker;
            LimitLeft += refillAmount;
         
            if (_spawnManager.trackedIndexs.Contains((tracker)))
            {
                _spawnManager.SpawnedList.Remove(tracker);
                _spawnManager.trackedIndexs.Remove(tracker);
                Destroy(other.gameObject,0.1f);
            }
        }
        if (LimitLeft > MaxLimit)
        {
            Debug.Log($" entred with {LimitLeft}");
            LimitLeft = MaxLimit;
            
        }



    }
}
