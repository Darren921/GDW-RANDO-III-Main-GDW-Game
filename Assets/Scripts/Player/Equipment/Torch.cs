using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Torch : LightEquipment
{
    internal bool torchActive;
    [SerializeField] private TextMeshProUGUI interactText;


    protected override void Update()
    {
        slider.value = CurrentUses;
        slider.maxValue = MaxUses;
        if (CurrentUses < 0)
        {
            CurrentUses = 0;
            light.gameObject.SetActive(false);
        }

        if (torchActive)
        {
            light.gameObject.SetActive(true);
        }
        else
        {
            light.gameObject.SetActive(false);
        }
    }


    public override void CheckIfUsable()
    {
    }

    public void ReduceCount()
    {
        CurrentUses--;
        _playerHotbar.FuelCount--;
    }
}