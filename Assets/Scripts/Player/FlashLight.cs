using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : EquipmentBase
{
    
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
}

