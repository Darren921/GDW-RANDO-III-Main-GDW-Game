using System.Collections;
using System.Collections.Generic;

using UnityEngine;

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

        controls.InGame.Movement.performed += _ =>
        {
            player.SetMoveDirection(_.ReadValue<Vector3>());
          
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
            player.startSprint();
        };
        controls.InGame.Sprint.canceled += _ =>
        {
            player.cancelSprint();   
        };
        controls.InGame.MousePos.performed += _ =>
        {
            mousePos = _.ReadValue<Vector2>();
        };
        controls.InGame.Hide.performed += _ =>
        {
            player.Hide();
        };
        controls.InGame.ItemSwap.performed += _ =>
        {
            player.ChangeItem(_.ReadValue<float>());
        };
        controls.InGame.OffOn.performed += _ =>
        {
            print(true);
            player.CheckIfActive();
        };

    }
   
    public static void EnableInGame()
    {
        controls.InGame.Enable();
    }
    public static void DisableInGame()
    {
        controls.InGame.Disable();
    }

}