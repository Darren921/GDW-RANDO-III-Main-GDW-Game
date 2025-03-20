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
    private bool QTEAble;
    [SerializeField] internal TextMeshProUGUI InteractText;
    PlayerHotbar _playerHotbar;
    [SerializeField] private Slider InteractionBar;
    [SerializeField] internal InputActionReference HeldInteractionAction;
    internal float holdDuration;
    [SerializeField] internal IceMelting iceMelting;
    private bool _isResetting;
    QuickTimeEvents quickTimeEvents;
    internal FrostSystem _frostSystem;
    internal bool isCurrentlyInteracting;
    internal bool textLocked;

    private void Start()
    {
        quickTimeEvents = FindFirstObjectByType<QuickTimeEvents>();
        _playerHotbar = GetComponent<PlayerHotbar>();
        InteractionBar.gameObject.SetActive(false);
        _frostSystem = FindObjectOfType<FrostSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (!InteractText.enabled)
        {
            InteractText.enabled = true;
        }
        if(other.GetComponent<IceMelting>())  ResetPlayerInteraction();
        if (isCurrentlyInteracting && textLocked ) return;
        
       
            var interactable = other.GetComponent<GameManager.IInteractable>();
            if (interactable == null) return;
            currentInteractable = interactable;
            ResetPlayerInteraction();
        
        
        //  print(currentInteractable);
        if (other.GetComponent<SelfInteractionManager>() != null)
        {
            isHeldInteraction = other.GetComponent<SelfInteractionManager>().isHeld;
            QTEAble = other.GetComponent<SelfInteractionManager>().QTEAble;
        }
        else if (other.GetComponent<backGroundInteractable>() != null)
        {
            InteractText.text = $"Press E to read {other.GetComponent<backGroundInteractable>().name.ToLower()}";
            isHeldInteraction = other.GetComponent<backGroundInteractable>().isHeld;
            QTEAble = other.GetComponent<backGroundInteractable>().QTEAble;

        }
        else if (other.GetComponent<IceMelting>() != null)
        {
            isHeldInteraction = other.GetComponent<IceMelting>().isHeld;
            QTEAble = other.GetComponent<IceMelting>().QTEAble;

        }
    }

    public void InteractWithQTE()
    {
        quickTimeEvents.InteractQTE();
    }

    private void Update()
    {
//        print(isHeldInteraction);
//33        print(  _playerHotbar._equipmentBases?[_playerHotbar.inputtedSlot].CurrentUses > 0 );
        if (HeldInteractionAction.action.IsPressed() && currentInteractable != null  && isHeldInteraction &&
            _playerHotbar._equipmentBases?[_playerHotbar.inputtedSlot].CurrentUses > 0 &&
            _playerHotbar.curEquipmentBase.ID != 1)
        {
          //  print("Holding");
            if (_playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].GetComponent<Torch>().equipped)
            {
                _playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].GetComponent<Torch>().torchActive =
                    true;
                _frostSystem.isFreezing = false;
            }
            isCurrentlyInteracting = true;
            textLocked = true;
            InteractionBar.gameObject.SetActive(true);
            holdDuration += Time.deltaTime;
            InteractionBar.value = holdDuration;
         
            if (quickTimeEvents.state == QuickTimeEvents.State.NotStarted && holdDuration >= quickTimeEvents.qteMin && QTEAble)
            {
                quickTimeEvents.StartQTE();
            }

            if (!iceMelting.AtMeltingPoint &&
                _playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].equipped && !isHeldInteraction)
            {
                _playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].gameObject.GetComponent<Torch>()
                    .torchActive = false;
            }
        }
    }

    public void ResetPlayerInteraction()
    {
        holdDuration = 0;
        if ( _playerHotbar.GetComponent<Player>().dead) return;
        _frostSystem.isFreezing = true;
        _playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].GetComponent<Torch>().torchActive = false;
        HeldInteractionAction.action.Reset();
        InteractionBar?.gameObject.SetActive(false);
        isCurrentlyInteracting = false;
        QTEAble = false;
        textLocked = false;

    }

    private void OnTriggerStay(Collider other)
    {
        if (!isCurrentlyInteracting)
        {
            var interactable = other.GetComponent<GameManager.IInteractable>();
            if (interactable == null) return;
            currentInteractable = interactable;
        }

        // print(currentInteractable);
        if (other.GetComponent<GroundObj>() != null)
        {
            var groundObj = other.GetComponent<GroundObj>();
            if (groundObj != null)
            {
                if(textLocked)return;
                switch (other.GetComponent<GroundObj>().item.data.Name)
                {
                    case "Battery":
                        InteractText.text = _playerHotbar.batteryCount < groundObj.item.data.Limit
                            ? $"Press E to Pickup {groundObj.item.data.Name.ToLower()} "
                            : $"Carry limit for {groundObj.item.data.Name.ToLower()} has been reached";
                        break;
                    case "Fuel":
                        InteractText.text = _playerHotbar.FuelCount < groundObj.item.data.Limit
                            ? $"Press E to Pickup {groundObj.item.data.Name.ToLower()}"
                            : $"Carry limit for {groundObj.item.data.Name.ToLower()} has been reached";
                        break;
                    default:
                        if (_playerHotbar.inputtedSlot != -1)
                        {
                            if (_playerHotbar.inputtedSlot <= 0)
                            {
//                                print("in check backup");

                                InteractText.text = $"Press E to Pickup {groundObj.item.data.Name.ToLower()} ";
                                return;
                            }

                            var item = Array.Find(_playerHotbar.Hotbar.Container.Slots,
                                slot => slot.item.Id == groundObj.item.data.Id);
                            if (item != null)
                            {
                                //        print("in check");
                                InteractText.text =
                                    _playerHotbar.Hotbar.Container.Slots[_playerHotbar.inputtedSlot].amount <
                                    _playerHotbar.Hotbar.Container.Slots[_playerHotbar.inputtedSlot].item.Limit
                                        ? $"Press E to Pickup {groundObj.item.data.Name.ToLower()}"
                                        : $"Carry limit for {groundObj.item.data.Name.ToLower()} has been reached";
                            }
                            else
                            {
                                //          print("in check backup");
                                InteractText.text = $"Press E to Pickup {groundObj.item.data.Name.ToLower()} ";
                            }
                        }

                        break;
                }
            }
        }

        if (other.GetComponent<SelfInteractionManager>() != null)
        {
            isHeldInteraction = other.GetComponent<SelfInteractionManager>().isHeld;
//           print(_frostSystem._frost > 50);
//           print(!_playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].GetComponent<Torch>().torchActive);
            if (_playerHotbar.curEquipmentBase != null)
            {
                switch (_playerHotbar.Hotbar.Container.Slots[_playerHotbar.inputtedSlot - 1].item.Id)
                {
                    case 1:
                        InteractText.text = _frostSystem._frost > 50 ? "Hold E to warm up with the Torch " : "";
                        break;

                    case 2:
                        if (_playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].GetComponent<Torch>().torchActive && !iceMelting.AtMeltingPoint && isCurrentlyInteracting)
                        {
                            InteractText.text = "Defrosting Self";
                        }
                        else if (_playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].GetComponent<Torch>().torchActive && !iceMelting.AtMeltingPoint)
                        {
                            InteractText.text = "Defrosting Self";
                        }
                        else
                        {
                            //    print("in display");
                            InteractText.text = _frostSystem._frost > 50 ? "Hold E to warm up with the Torch " : "";
                        }

                        break;
                    default:
                        if (_playerHotbar.Hotbar.Container.Slots[_playerHotbar.inputtedSlot - 1].item.Id != -1)
                        {
                            InteractText.text = holdDuration <= 0
                                ? $"Hold E to use {_playerHotbar.Hotbar.Container.Slots?[_playerHotbar.inputtedSlot - 1].item.Name}"
                                : $"Using {_playerHotbar.Hotbar.Container.Slots?[_playerHotbar.inputtedSlot - 1].item.Name} ";
                        }

                        break;
                }
            }


            InteractionBar.maxValue = 5;
        }

        if (other.GetComponent<IceMelting>() != null &&
            _playerHotbar._equipmentBases[_playerHotbar.returnTorchLocation()].CurrentUses > 0)
        {
            QTEAble = iceMelting.QTEAble;
            InteractText.text = iceMelting.isMelting
                ? ""
                : $"Hold E to Melt {iceMelting.MeltingStage} times to fully melt left";
                  iceMelting.IcemeltingText.text = iceMelting.isMelting && holdDuration > 0  ? $"Melting Ice {iceMelting.MeltingStage} / 5 " : "";
        }

        if (other.GetComponent<backGroundInteractable>() != null)
        {
            if(textLocked)return;
            InteractText.text = _playerHotbar.isOpen
                ? ""
                : $"Press E to read {other.GetComponent<backGroundInteractable>().name.ToLower()}";
            if (!_playerHotbar.isOpen)
            {
                InteractText.enabled = true;
            }
        }
      
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<GameManager.IInteractable>() != currentInteractable) return;
        currentInteractable = null;
        InteractText.text = "";
        InteractionBar.gameObject.SetActive(false);
        iceMelting.IcemeltingText.text = "";
        holdDuration = 0;
        isHeldInteraction = false;
        isCurrentlyInteracting = false;
        ResetPlayerInteraction();
        textLocked = false;
    }

    public void TryInteract()
    {
        currentInteractable?.Interact();
        print("Standard interaction");
    }

    public void TryHeldInteract()
    {
        print("Held interaction");
        InteractionBar.gameObject.SetActive(false);
        currentInteractable?.HeldInteract();
    }
}