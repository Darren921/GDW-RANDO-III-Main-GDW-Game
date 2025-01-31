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
    private bool isHeldInteraction;
    [SerializeField] internal TextMeshProUGUI InteractText;
    private Player _player;
    PlayerHotbar hotbar;
    [SerializeField] private Slider InteractionBar;
   [SerializeField] internal InputActionReference HeldInteractionAction;
    internal float holdDuration;
    [SerializeField]internal IceMelting iceMelting;
    private bool _isResetting;

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
                isHeldInteraction = other.GetComponent<GroundObj>().isHeld;
            }
            else if(other.GetComponent<backGroundInteractable>() != null)
            {
                InteractText.text = $"Press E to read {other.GetComponent<backGroundInteractable>().name.ToLower()}";
                isHeldInteraction = other.GetComponent<GroundObj>().isHeld;
            }
        }
    }

    private void Update()
    {
        if (HeldInteractionAction.action.IsPressed() && currentInteractable != null && isHeldInteraction &&
            hotbar._equipmentBases[hotbar.returnTorchLocation()].CurrentUses > 0 &&  hotbar._equipmentBases[hotbar.returnTorchLocation()].equipped)
        {
            hotbar._equipmentBases[hotbar.returnTorchLocation()].GetComponent<Torch>().torchActive = true;
            InteractText.text = iceMelting.AtMeltingPoint &&  hotbar._equipmentBases[hotbar.returnTorchLocation()].gameObject.GetComponent<Torch>().CurrentUses > 0 ? "Hold E to Melt" : "";            
            InteractionBar.gameObject.SetActive(true);
            holdDuration += Time.deltaTime;
            InteractionBar.value = holdDuration;
        }
        else if(HeldInteractionAction.action.WasReleasedThisFrame() || isHeldInteraction && !hotbar._equipmentBases[hotbar.returnTorchLocation()].equipped && !iceMelting.AtMeltingPoint && !_isResetting   )
        {
            Reset();
        }
    }

    public void Reset()
    {

        if (!_isResetting)
        {
            _isResetting = true;
            holdDuration = 0;
            InteractionBar.gameObject.SetActive(false);
            hotbar._equipmentBases[hotbar.returnTorchLocation()].GetComponent<Torch>().torchActive = false;
            InteractText.text = iceMelting.AtMeltingPoint && hotbar._equipmentBases[hotbar.returnTorchLocation()].gameObject.GetComponent<Torch>().CurrentUses > 0 ? "Hold E to Melt" : "";           
            HeldInteractionAction.action.Disable();
            _isResetting = false;
            HeldInteractionAction.action.Enable();

        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other is null)
        {
            InteractText.text = "";
            return;
        } 
        if(other.GetComponent<IceMelting>() != null)
        {
            iceMelting = other.GetComponent<IceMelting>();
            isHeldInteraction = other.GetComponent<IceMelting>().isHeld;
            if (iceMelting != null)
            {
                iceMelting.AtMeltingPoint = true;
            }
        }
        else
        {
            iceMelting.AtMeltingPoint = false;
        }
        GameManager.IInteractable interactable = other.GetComponent<GameManager.IInteractable>();
        if (interactable == null) return;
        if (other.CompareTag("IceWall") && hotbar._equipmentBases[hotbar.returnTorchLocation()].gameObject.GetComponent<Torch>().equipped)
        {
            print(other.CompareTag("IceWall"));
           print(hotbar._equipmentBases[hotbar.returnTorchLocation()].gameObject.GetComponent<Torch>().equipped);
            iceMelting.AtMeltingPoint = true;
           InteractText.text = iceMelting.AtMeltingPoint && hotbar._equipmentBases[hotbar.returnTorchLocation()].gameObject.GetComponent<Torch>().CurrentUses > 0 ? "Hold E to Melt" : "";
     
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
        holdDuration = 0;
        iceMelting.AtMeltingPoint = false;
        isHeldInteraction = false;

    }

    public void TryInteract()
    {
        InteractText.enabled = false;
        currentInteractable?.Interact();
    }

    public void TryHeldInteract()
    {
        print("tryed");
        InteractText.enabled = false;
        InteractionBar.gameObject.SetActive(false);
        currentInteractable?.HeldInteract();

    }
}
