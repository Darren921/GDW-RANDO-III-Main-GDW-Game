using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class backGroundInteractable : MonoBehaviour ,GameManager.IInteractable
{
    [SerializeField] private GameObject[] display;
   [SerializeField] Sprite _sprite;

   private PlayerHotbar _playerHotbar;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var gameObjects in display)
        {
            gameObjects.SetActive(false);

        }
        _playerHotbar = FindFirstObjectByType<PlayerHotbar>();
        isHeld = false;
    }


    public bool isHeld { get; set; }

    public void Interact()
    {
        _playerHotbar.isOpen = !_playerHotbar.isOpen;
        if (_playerHotbar.isOpen)
        {
            if (GetComponent<GameManager.IInteractable>() == null) return;
            foreach (var gameObjects in display)
            {
                gameObjects.SetActive(true);

            }
            display[1].GetComponent<Image>().sprite = _sprite;
                
        }
        else
        {
            foreach (var gameObjects in display)
            {
                gameObjects.SetActive(false);
                gameObjects.GetComponent<Image>().sprite = null;

            }
        }

    }

    public void HeldInteract()
    {
        
    }
}
