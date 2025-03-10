using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stim : ConsumableEquipment
{
 
    // Update is called once per frame

    internal override void HeldInteract()
    {
        print("here");
        if (!(_playerHotbar._equipmentBases[_playerHotbar.inputtedSlot].CurrentUses > 0) ||
            !(_playerHotbar.gameObject.GetComponent<PlayerInteraction>().holdDuration <= 5.1f)) return;

        print("in 1 ");

        if (!_playerHotbar._equipmentBases[_playerHotbar.inputtedSlot].equipped) return;
        print("in 2 ");

        if (_playerHotbar.gameObject.GetComponent<PlayerInteraction>().holdDuration >= 4.9f)
        {
            print("Held Interact Self");
            _playerHotbar.Hotbar.RemoveItem(_playerHotbar.Hotbar.Container.Slots[_playerHotbar.inputtedSlot - 1].item, 1);
            if (_playerHotbar.Hotbar.Container.Slots[_playerHotbar.inputtedSlot - 1].item.Id <= 0)
            {
                ActivateEffect();
                _playerHotbar.curEquipmentBase.gameObject.SetActive(false);
                _playerHotbar._equipmentBases.Remove(_playerHotbar.curEquipmentBase);
                _playerHotbar.ChangeItem(_playerHotbar.inputtedSlot);
                
            }   
            _playerHotbar.gameObject.GetComponent<PlayerInteraction>().Reset();
        }

        else if (_playerHotbar.gameObject.GetComponent<PlayerInteraction>().holdDuration <= 5.1)
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

    protected override void ActivateEffect()
    {
        _playerMovement.StartCoroutine(_playerMovement.SetStimSpeed());
    }
}
