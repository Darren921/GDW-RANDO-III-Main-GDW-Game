using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    private GameManager.IInteractable currentInteractable;
    private bool isHeldInteraction;
    [SerializeField] internal TextMeshProUGUI InteractText;
    private Player _player;
    PlayerHotbar hotbar;
    [SerializeField] private Slider InteractionBar;
   [SerializeField] internal InputActionReference HeldInteractionAction;
    internal float holdDuration;
    [SerializeField]internal IceMelting iceMelting;
    private bool _isResetting;
    FrostSystem _frostSystem;
    private void Start()
    {
        hotbar = GetComponent<PlayerHotbar>();
        InteractionBar.gameObject.SetActive(false);
        _frostSystem = FindObjectOfType<FrostSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
      
        if (!InteractText.enabled)
        {
            InteractText.enabled = true;
        }
        var interactable= other.GetComponent<GameManager.IInteractable>();

        if (interactable == null) return;
        currentInteractable = interactable; 
      //  print(currentInteractable);
       
        if (other.GetComponent<FrostSystem>() != null)
        {
            if (_frostSystem._frost > 100)
            {
                InteractText.text = $"Hold E to warm up ";
            }
            isHeldInteraction = other.GetComponent<FrostSystem>().isHeld;
         
        }
        else if(other.GetComponent<backGroundInteractable>() != null)
        {
            InteractText.text = $"Press E to read {other.GetComponent<backGroundInteractable>().name.ToLower()}";
            isHeldInteraction = other.GetComponent<backGroundInteractable>().isHeld;
        }
        else if(other.GetComponent<IceMelting>() != null)
        {
            isHeldInteraction = other.GetComponent<IceMelting>().isHeld;
        }
     
    }

    private void Update()
    {
        if (HeldInteractionAction.action.IsPressed() && currentInteractable != null && isHeldInteraction &&
            hotbar._equipmentBases[hotbar.returnTorchLocation()].CurrentUses > 0 &&  hotbar._equipmentBases[hotbar.returnTorchLocation()].equipped )
        {
            hotbar._equipmentBases[hotbar.returnTorchLocation()].GetComponent<Torch>().torchActive = true;
            InteractionBar.gameObject.SetActive(true);
            holdDuration += Time.deltaTime;
            InteractionBar.value = holdDuration;
        }
        if(HeldInteractionAction.action.WasReleasedThisFrame()  || isHeldInteraction && !hotbar._equipmentBases[hotbar.returnTorchLocation()].equipped && !iceMelting.isMelting && !_isResetting   )
        {
            hotbar._equipmentBases[hotbar.returnTorchLocation()].gameObject.GetComponent<Torch>().torchActive = false;
            Reset();
            HeldInteractionAction.action.Reset();

        }

        if (!iceMelting.AtMeltingPoint && hotbar._equipmentBases[hotbar.returnTorchLocation()].equipped && !isHeldInteraction)
        {
            hotbar._equipmentBases[hotbar.returnTorchLocation()].gameObject.GetComponent<Torch>().torchActive = false;
        }
    }

    public void Reset()
    {
        
        if (_isResetting || this is null) return;
        _isResetting = true;
        hotbar._equipmentBases[hotbar.returnTorchLocation()].GetComponent<Torch>().torchActive = false;
        holdDuration = 0;
        if(InteractionBar)
         InteractionBar?.gameObject.SetActive(false);
        _isResetting = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other is null)
        {
            InteractText.text = "";
            Reset();
            HeldInteractionAction.action.Reset();
            return;
        } 
        var interactable = other.GetComponent<GameManager.IInteractable>();
        if (interactable == null) return;
        currentInteractable = interactable; 
        print(currentInteractable);
        if (other.GetComponent<GroundObj>() != null )
        {
            var groundObj = other.GetComponent<GroundObj>();
            if (groundObj != null)
            {
                switch (other.GetComponent<GroundObj>().item.data.Name)
                {
                    case "Battery":
                        InteractText.text = hotbar.batteryCount < groundObj.item.data.Limit ? $"Press E to Pickup {groundObj.item.data.Name.ToLower()} " : $"Carry limit for {groundObj.item.data.Name.ToLower()} has been reached";
                        break;
                    case "Fuel" :
                        InteractText.text = hotbar.FuelCount < groundObj.item.data.Limit ? $"Press E to Pickup {groundObj.item.data.Name.ToLower()}" : $"Carry limit for {groundObj.item.data.Name.ToLower()} has been reached";
                        break;
                }
                   
            }
              
        }
        if (other.GetComponent<IceMelting>() != null && hotbar._equipmentBases[hotbar.returnTorchLocation()].CurrentUses > 0)
        {
            InteractText.text = iceMelting.isMelting ? "" : $"Hold E to Melt,  {iceMelting.MeltingStage} times to fully melt ";
            iceMelting.IcemeltingText.text = iceMelting.isMelting ? $"Melting Ice {iceMelting.MeltingStage} / 5 " : ""   ;
        }
        if (other.GetComponent<backGroundInteractable>() == null) return;
        InteractText.text = hotbar.isOpen ? "" : $"Press E to read {other.GetComponent<backGroundInteractable>().name.ToLower()}";
        if (!hotbar.isOpen)
        {
            InteractText.enabled = true;
        } 
       
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<GameManager.IInteractable>() != currentInteractable) return;
        currentInteractable = null; 
        InteractText.text = "";
        InteractionBar.gameObject.SetActive(false);
        holdDuration = 0;
        isHeldInteraction = false;
        HeldInteractionAction.action.Reset();
    }

    public void TryInteract()
    {
        InteractText.enabled = false;
        currentInteractable?.Interact();
        print("Standard interaction");
    }

    public void TryHeldInteract()
    {
        print("Held interaction");
        InteractText.enabled = false;
        InteractionBar.gameObject.SetActive(false);
        currentInteractable?.HeldInteract();

    }

 

  
    
}
