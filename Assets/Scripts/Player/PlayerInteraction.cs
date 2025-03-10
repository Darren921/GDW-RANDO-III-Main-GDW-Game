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
    PlayerHotbar hotbar;
    [SerializeField] private Slider InteractionBar;
   [SerializeField] internal InputActionReference HeldInteractionAction;
    internal float holdDuration;
    [SerializeField]internal IceMelting iceMelting;
    private bool _isResetting;
  internal FrostSystem _frostSystem;
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
        if (other.GetComponent<SelfInteractionManager>() != null)
        {
            isHeldInteraction = other.GetComponent<SelfInteractionManager>().isHeld;
            InputManager.HoldChange(true);

        }
        else if(other.GetComponent<backGroundInteractable>() != null)
        {
            InteractText.text = $"Press E to read {other.GetComponent<backGroundInteractable>().name.ToLower()}";
            isHeldInteraction = other.GetComponent<backGroundInteractable>().isHeld;
        }
        else if(other.GetComponent<IceMelting>() != null)
        {
            InputManager.HoldChange(false);
            isHeldInteraction = other.GetComponent<IceMelting>().isHeld;
        }
     
    }

    private void Update()
    {
//        print(isHeldInteraction);
//33        print(  hotbar._equipmentBases?[hotbar.inputtedSlot].CurrentUses > 0 );
        if (HeldInteractionAction.action.IsPressed() && currentInteractable != null && isHeldInteraction &&
            hotbar._equipmentBases?[hotbar.inputtedSlot].CurrentUses > 0 && hotbar.curEquipmentBase.ID != 1)
        {
            print("Holding");
            if (hotbar._equipmentBases[hotbar.returnTorchLocation()].GetComponent<Torch>().equipped)
            {
                hotbar._equipmentBases[hotbar.returnTorchLocation()].GetComponent<Torch>().torchActive = true;
            }
            InteractionBar.gameObject.SetActive(true);
            holdDuration += Time.deltaTime;
            InteractionBar.value = holdDuration;
        }
        if (!iceMelting.AtMeltingPoint && hotbar._equipmentBases[hotbar.returnTorchLocation()].equipped && !isHeldInteraction)
        {
            hotbar._equipmentBases[hotbar.returnTorchLocation()].gameObject.GetComponent<Torch>().torchActive = false;
        }
    }

    public void Reset()
    {
        if (_isResetting) return;
        _isResetting = true;
        hotbar._equipmentBases[hotbar.returnTorchLocation()].GetComponent<Torch>().torchActive = false;
        HeldInteractionAction.action.Reset();
        holdDuration = 0;
        InteractionBar?.gameObject.SetActive(false);
        _isResetting = false;

    }

    private void OnTriggerStay(Collider other)
    {
        if (other is null)
        {
            Reset();
            HeldInteractionAction.action.Reset();
            return;
        } 
        var interactable = other.GetComponent<GameManager.IInteractable>();
        if (interactable == null) return;
        currentInteractable = interactable; 
       // print(currentInteractable);
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
                   default:
                        if (hotbar.inputtedSlot != -1 )
                        {
                            if (hotbar.inputtedSlot <= 0)
                            {
//                                print("in check backup");

                                InteractText.text =   $"Press E to Pickup {groundObj.item.data.Name.ToLower()} ";
                                return;
                            }
                            var item = Array.Find(hotbar.Hotbar.Container.Slots, slot => slot.item.Id == groundObj.item.data.Id);
                            if (item != null)
                            {
                        //        print("in check");
                                InteractText.text =  hotbar.Hotbar.Container.Slots[hotbar.inputtedSlot].amount <  hotbar.Hotbar.Container.Slots[hotbar.inputtedSlot].item.Limit ? $"Press E to Pickup {groundObj.item.data.Name.ToLower()}" : $"Carry limit for {groundObj.item.data.Name.ToLower()} has been reached";
                                
                            }
                            else
                            {
                      //          print("in check backup");

                                InteractText.text =   $"Press E to Pickup {groundObj.item.data.Name.ToLower()} ";
                            }                          

                        }
                        break;
                      
                      
                }
                   
            }
              
        }

        if (other.GetComponent<SelfInteractionManager>() != null)
        {
//           print(_frostSystem._frost > 50);
//           print(!hotbar._equipmentBases[hotbar.returnTorchLocation()].GetComponent<Torch>().torchActive);
            if (hotbar.curEquipmentBase != null)
            {
                switch (hotbar.Hotbar.Container.Slots[hotbar.inputtedSlot - 1].item.Id)
                {
                    case 1:
                        InteractText.text = " ";
                        break;
                    
                    case 2:
                        if (hotbar._equipmentBases[hotbar.returnTorchLocation()].GetComponent<Torch>().torchActive &&
                            !iceMelting.AtMeltingPoint)
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

                        if (hotbar.Hotbar.Container.Slots[hotbar.inputtedSlot - 1].item.Id != -1 )
                        {
                            InteractText.text = holdDuration <= 0
                                ? $"Hold E to use {hotbar.Hotbar.Container.Slots?[hotbar.inputtedSlot - 1].item.Name}"
                                : $"Using {hotbar.Hotbar.Container.Slots?[hotbar.inputtedSlot - 1].item.Name} ";
                        }

                        break;
                }
            }





            InteractionBar.maxValue = 5;

        }
        if (other.GetComponent<IceMelting>() != null && hotbar._equipmentBases[hotbar.returnTorchLocation()].CurrentUses > 0)
        {
            InteractText.text = iceMelting.isMelting ? "" : $"Hold E to Melt {iceMelting.MeltingStage} times to fully melt left";
            iceMelting.IcemeltingText.text = iceMelting.isMelting ? $"Melting Ice {iceMelting.MeltingStage} / 5 " : ""   ;
            InteractionBar.maxValue = 10;
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
