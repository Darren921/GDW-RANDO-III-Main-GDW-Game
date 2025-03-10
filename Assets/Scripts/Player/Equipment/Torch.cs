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
            lightSource.gameObject.SetActive(true);
        }
        else
        {
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
            
            frostSystem.reduceFrost();
            _playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].GetComponent<Torch>().ReduceCount();
            _playerHotbar.gameObject.GetComponent<PlayerInteraction>().Reset();
        }
        
        else if(_playerHotbar.gameObject.GetComponent<PlayerInteraction>().holdDuration <= 5.1)
        {
            print("in oh no ");
            print(_playerHotbar.gameObject.GetComponent<PlayerInteraction>().holdDuration);
            _playerHotbar.gameObject.GetComponent<PlayerInteraction>().Reset();
        }
        _playerInteraction.InteractText.text = "";
    }

    internal override void CheckIfUsable()
    {
    }

    internal override void CheckHeldInteract()
    {
    }

    public void ReduceCount()
    {
        CurrentUses--;
        _playerHotbar.FuelCount--;
    }
}