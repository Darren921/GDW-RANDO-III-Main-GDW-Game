using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : LightEquipment
{
    protected virtual void Update()
    {
        base.Update();
        if (CurrentUses > 0 && active)
        {
            CurrentUses -= Time.deltaTime;
        }
    }

    public override void CheckIfUsable()
    {
        if (CurrentUses > 0 && equipped)
        {
            active = !active;
            if (active)
            {
                light.SetActive(true);
            }
            else
            {
                active = false;
                light.SetActive(false);
            }
        }
        else
        {
            active = false;
            light.SetActive(false);
        }
    }


    private void OnDisable()
    {
        active = false;
        light.SetActive(false);

    }
}