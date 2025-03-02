using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeFrost : MonoBehaviour,GameManager.IInteractable  
{
    internal FrostSystem frostSystem;
    PlayerHotbar _playerHotbar;
    PlayerInteraction _playerInteraction;
    public InputActionReference _reference;
    private void Start()
    {
        _playerHotbar = FindObjectOfType<PlayerHotbar>();
        frostSystem = FindObjectOfType<FrostSystem>();
        _playerInteraction = FindObjectOfType<PlayerInteraction>();

        isHeld = true;

    }
    
   
    public bool isHeld { get; set; }
    public void Interact()
    {
    }

    public void HeldInteract()
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
            _playerHotbar.GetComponent<PlayerInteraction>().HeldInteractionAction.action.Reset();
        }
        
        else if(_playerHotbar.gameObject.GetComponent<PlayerInteraction>().holdDuration <= 5.1)
        {
            print("in oh no ");
            print(_playerHotbar.gameObject.GetComponent<PlayerInteraction>().holdDuration);
            _playerHotbar.gameObject.GetComponent<PlayerInteraction>().Reset();
            _playerHotbar.GetComponent<PlayerInteraction>().HeldInteractionAction.action.Reset();

        }
        _playerInteraction.InteractText.text = "";

    }
}
