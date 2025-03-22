using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : LightEquipment
{
    protected override void Update()
    {
        base.Update();
        if (CurrentUses > 0 && active)
        {
            CurrentUses -= Time.deltaTime;
        }
    }

    internal override void HeldInteract()
    {
        
    }

    internal override void CheckIfUsable()
    {
        if (CurrentUses > 0 && equipped)
        {
            active = !active;
            if (active)
            {
                AudioManager.Instance.PlayFlashlightSFX("Flashlight On Sound");
                lightSource.SetActive(true);
            }
            else
            {
                AudioManager.Instance.PlayFlashlightSFX("Flashlight Off Sound");
                active = false;
                lightSource.SetActive(false);
            }
        }
        else
        {
            active = false;
            lightSource.SetActive(false);
        }
    }

 


    private void OnDisable()
    {
        active = false;
        lightSource.SetActive(false);

    }
}