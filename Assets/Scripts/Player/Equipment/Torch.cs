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
    [SerializeField] internal BoxCollider torchCollider;
    [SerializeField] internal FrostSystem frostSystem;

    private bool torchCue = false;

    protected override void Update()
    {
        slider.value = CurrentUses;
        slider.maxValue = MaxUses;
        if (CurrentUses < 0)
        {
            CurrentUses = 0;
            lightSource.gameObject.SetActive(false);
        }

        if (torchActive)
        {
            if (torchCue)
            {
                torchCue = false;
                AudioManager.Instance.PlayTorchSFX("Torch Gun Sound");
            }

            lightSource.gameObject.SetActive(true);
        }
        else
        {
            if (!torchCue)
            {
                torchCue = true;
                AudioManager.Instance.StopTorchSFX("Torch Gun Sound");
            }

            lightSource.gameObject.SetActive(false);
        }
    }


    internal override void HeldInteract()
    {
        print("here");
        if (!(_playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].CurrentUses > 0) ||
            !(_playerHotbar.gameObject.GetComponent<PlayerInteraction>().holdDuration <= 5.1f)) return;
        
        print("in 1 ");
        
        if (!_playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].equipped) return;
        print("in 2 ");
        
        if (_playerHotbar.gameObject.GetComponent<PlayerInteraction>().holdDuration >= 4.9f)
        {
            print("Held Interact Self");
            
            StartCoroutine(frostSystem.reduceFrost());
            _playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].GetComponent<Torch>().ReduceCount(0.5f);
            _playerHotbar.gameObject.GetComponent<PlayerInteraction>().ResetPlayerInteraction();
        }
        
        else if(_playerHotbar.gameObject.GetComponent<PlayerInteraction>().holdDuration <= 5.1)
        {
            print("in oh no ");
            print(_playerHotbar.gameObject.GetComponent<PlayerInteraction>().holdDuration);
            _playerHotbar.gameObject.GetComponent<PlayerInteraction>().ResetPlayerInteraction();
        }
        _playerInteraction.InteractText.text = "";
    }

    internal override void CheckIfUsable()
    {
    }
    

    public void ReduceCount(float amount)
    {
        CurrentUses -= amount;
        _playerHotbar.FuelCount -= amount;

        if (CurrentUses < 0)
        {
            CurrentUses = 0;
        }
        if(  _playerHotbar.FuelCount < 0)
        {
            _playerHotbar.FuelCount = 0;
        }
    }
}