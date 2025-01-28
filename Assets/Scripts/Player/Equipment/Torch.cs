using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Torch : LightEquipment
{
    public bool torchActive { get; private set; }

    protected internal override void CheckIfActive()
    {
        if (lightObj == null)
        {
            lightObj = gameObject.transform.Find("Light")?.gameObject;
        }
        print("working");
        if (equipped)
        {
            active = !active;
            if (CurrentUses > 0)
            {
                //if item is active, activate relevant systems 
                if (active && !checkActive)
                {
                    checkActive = true;
                    StartCoroutine(CheckCharge());
                    lightObj.SetActive(true);
                    torchActive = true;
                }
                else
                {
                    active = false;
                    lightObj.SetActive(false);
                    torchActive = false;

                }
            }
            else
            {
                active = false;
                lightObj.SetActive(false);
                torchActive = false;

            }
        }
    }
    protected internal override IEnumerator CheckCharge()
    {
        //
        yield return new WaitUntil(() => active == false );
        checkActive = false;
        torchActive = false;
    }


    private void OnDisable()
    {
        active = false;
        torchActive = false;
        checkActive = false;
    }
}