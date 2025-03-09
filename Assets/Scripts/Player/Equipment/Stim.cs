using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stim : ConsumableEquipment
{
    // Start is called before the first frame update
    void Start()
    {

    }

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
            _playerHotbar.Hotbar.Container.Slots[_playerHotbar.inputtedSlot]
                .UpdateSlot(_playerHotbar.Hotbar.Container.Slots[_playerHotbar.inputtedSlot].item, 0);
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
}
