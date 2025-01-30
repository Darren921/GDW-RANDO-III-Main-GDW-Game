using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Torch : LightEquipment
{
    internal bool torchActive;
    [SerializeField] private TextMeshProUGUI interactText;

    public new virtual void Update()
    {
        slider.value = CurrentUses;
        slider.maxValue = MaxUses;

        if (CurrentUses < 0)
        {
            CurrentUses = 0;
            active = false;
            lightObj.SetActive(false);
        }

        if (torchActive)
        {
            lightObj.SetActive(true);

        }
        else
        {
            lightObj.SetActive(false);
        }

}
    protected internal override void CheckIfActive()
    {
       
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
        equipped = false;
        torchActive = false;
        checkActive = false;
        interactText.text = "";
    }

    public void reduceCount()
    {
        CurrentUses -= CurrentUses;
    }
}