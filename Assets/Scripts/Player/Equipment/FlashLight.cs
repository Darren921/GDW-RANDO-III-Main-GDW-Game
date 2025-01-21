using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : EquipmentBase
{
    [SerializeField] private ItemObj matchingItem;
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
                    checkActive = true;
                    StartCoroutine(CheckCharge()) ;
                    lightObj.SetActive(true);
                }
                else
                {
                    active = false;
                    lightObj.SetActive(false);
                    
                }
            }
            else
            {
                active = false;
                lightObj.SetActive(false);

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

