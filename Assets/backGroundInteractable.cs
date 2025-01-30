using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class backGroundInteractable : MonoBehaviour ,GameManager.IInteractable
{
    [SerializeField] private GameObject[] display;
   [SerializeField] Sprite _sprite;

   private Player _player;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var gameObjects in display)
        {
            gameObjects.SetActive(false);

        }
        _player = FindFirstObjectByType<Player>();
    }
    
    
    public void Interact()
    {
        _player.isOpen = !_player.isOpen;
        if (_player.isOpen)
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
