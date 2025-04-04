using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static Controls controls;
    private static Vector3 mousePos;

    public static Vector3 GetMousePos()
    {
        return mousePos;
    }

    //Activates player controls;
    public static void Init(Player player)
    {
        controls = new Controls();

        var PlayerMovement = player.GetComponent<PlayerMovement>();
        var PlayerHotbar = player.GetComponent<PlayerHotbar>();
        var PlayerInteraction = player.GetComponent<PlayerInteraction>();
        controls.InGame.Movement.performed += _ =>
        {
            PlayerMovement.SetMoveDirection(_.ReadValue<Vector3>());
          
            if (_.ReadValue<Vector3>() != Vector3.zero)
            {
                player.walkingSound();
            }
            else
            {
                player.stopWalkingSound();
            }
        };
        controls.InGame.Sprint.performed += _ =>
        {
            PlayerMovement.startSprint();
        };
        controls.InGame.Sprint.canceled += _ =>
        {
            PlayerMovement.cancelSprint();   
        };
        controls.InGame.MousePos.performed += _ =>
        {
            mousePos = _.ReadValue<Vector2>();
        };
        controls.InGame.Hide.performed += _ =>
        {
      //      player.Hide();
        };
        controls.InGame.ItemSwap.performed += _ =>
        {
            PlayerHotbar.ChangeItem(_.ReadValue<Single>());
        };
        controls.InGame.OffOn.performed += _ =>
        {
            PlayerHotbar?.checkIfActive();
        };
        controls.InGame.QTE.performed += _ =>
        {
            PlayerInteraction?.InteractWithQTE();
        };
         controls.InGame.OpenAndCloseInv.performed += _ =>
         { 
             PlayerHotbar?.ManageHotbar();
         };
        controls.InGame.Interact.performed += _ =>
        {
            PlayerInteraction?.TryInteract();
        };
        controls.InGame.Interact.canceled += _ =>
        {
            PlayerInteraction?.ResetPlayerInteraction();
        };
       

        controls.InGame.HeldInteract.performed += _ =>
        {
            PlayerInteraction?.TryHeldInteract();
        };
        controls.UI.MainMenu.performed += _ =>
        {
            player.OpenMenu();
        };

    }


    public static void DisableItemSwap()
    {
        controls.InGame.ItemSwap.Disable();
    }
    public static void EnableItemSwap()
    {
        controls.InGame.ItemSwap.Enable();
    }

    public static void EnableInGame()
    {
        controls.InGame.Enable();
        controls.UI.Enable();
    }
    public static void DisableInGame()
    {
        controls.InGame.Disable();
    }

    public static void DisableUI()
    {
        controls.UI.Disable();
    }

}
