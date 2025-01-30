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
    public GameManager.IInteractable currentInteractable;

    [SerializeField] internal TextMeshProUGUI InteractText;
    private Player _player;
    PlayerHotbar hotbar;
    [SerializeField] private Slider InteractionBar;
   [SerializeField] internal InputActionReference HeldInteractionAction;
    internal float holdDuration;
    [SerializeField]private IceMelting iceMelting;

    private void Start()
    {
        _player = GetComponent<Player>();
        hotbar = GetComponent<PlayerHotbar>();
        InteractionBar.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
      
        if (!InteractText.enabled)
        {
            InteractText.enabled = true;
        }
        GameManager.IInteractable interactable = other.GetComponent<GameManager.IInteractable>();

        if (interactable != null)
        {
            currentInteractable = interactable; // Store the interactable
            if (other.GetComponent<GroundObj>() != null)
            {
                InteractText.text = $"Press E to pickup {other.GetComponent<GroundObj>().item.data.Name.ToLower()}";
            }
            else if(other.GetComponent<backGroundInteractable>() != null)
            {
                InteractText.text = $"Press E to read {other.GetComponent<backGroundInteractable>().name.ToLower()}";
            }
        }
        
    }

    private void Update()
    {
           if (HeldInteractionAction.action.IsPressed() && currentInteractable != null)
           {
               hotbar._equipmentBases[hotbar.returnTorchLocation()].GetComponent<Torch>().torchActive = true;
               InteractionBar.gameObject.SetActive(true);
               holdDuration += Time.deltaTime;
               InteractionBar.value = holdDuration;
           }
           else if (HeldInteractionAction.action.WasReleasedThisFrame() || HeldInteractionAction.action.WasPerformedThisFrame())
           {
               holdDuration = 0;
               hotbar._equipmentBases[hotbar.returnTorchLocation()].GetComponent<Torch>().torchActive = false;
               InteractText.text = iceMelting.isMelting ? "" : " Hold E to Melt";
               InteractionBar.gameObject.SetActive(false);

           }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other is null)
        {
            InteractText.text = "";
            return;
        }
        GameManager.IInteractable interactable = other.GetComponent<GameManager.IInteractable>();
        if (interactable == null) return;
        if (other.CompareTag("IceWall") && hotbar._equipmentBases[hotbar.returnTorchLocation()].gameObject.GetComponent<Torch>().equipped)
        {
            print(other.CompareTag("IceWall"));
           print(hotbar._equipmentBases[hotbar.returnTorchLocation()].gameObject.GetComponent<Torch>().equipped);
            iceMelting.AtMeltingPoint = true;
           if(hotbar._equipmentBases[hotbar.returnTorchLocation()].gameObject.GetComponent<Torch>().equipped == false) iceMelting.AtMeltingPoint = false;
           InteractText.text = iceMelting.isMelting ? "" : " Hold E to Melt";
     
        }
        if (other.GetComponent<backGroundInteractable>() == null) return;
        InteractText.text = _player.isOpen ? "" : $"Press E to read {other.GetComponent<backGroundInteractable>().name.ToLower()}";
        if (!_player.isOpen)
        {
            InteractText.enabled = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<GameManager.IInteractable>() != currentInteractable) return;
        currentInteractable = null; 
        InteractText.text = "";
        iceMelting.AtMeltingPoint = false;
        InteractionBar.gameObject.SetActive(false);
    }

    public void TryInteract()
    {
        InteractText.enabled = false;
        currentInteractable?.Interact();
    }

    public void TryHeldInteract()
    {
        InteractText.enabled = false;
        InteractionBar.gameObject.SetActive(true);
        currentInteractable?.HeldInteract();

    }
}
