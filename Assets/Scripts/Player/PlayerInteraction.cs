using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private GameManager.IInteractable currentInteractable;

    [SerializeField] internal TextMeshProUGUI InteractText;
    private Player _player;

    private void Start()
    {
        _player = GetComponent<Player>();
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
                InteractText.text = $"Press F to pickup {other.GetComponent<GroundObj>().item.data.Name.ToLower()}";
            }
            else if(other.GetComponent<backGroundInteractable>() != null)
            {
                InteractText.text = $"Press F to read {other.GetComponent<backGroundInteractable>().name.ToLower()}";
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        GameManager.IInteractable interactable = other.GetComponent<GameManager.IInteractable>();

        if (interactable == null) return;
        if (other.GetComponent<backGroundInteractable>() == null) return;
        InteractText.text = _player.isOpen ? "" : $"Press F to read {other.GetComponent<backGroundInteractable>().name.ToLower()}";
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
    }

    public void TryInteract()
    {
        InteractText.enabled = false;
        currentInteractable?.Interact();
    }
}
