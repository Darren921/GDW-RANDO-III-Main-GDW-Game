using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DialogueEditor;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    Player _player;
    [SerializeField]
    CinemachineInputProvider InputProvider;

  


    void Start()
    {
        _player = FindObjectOfType<Player>();
    }

   
    public void convoLock()
    {
        Cursor.lockState = CursorLockMode.None;
        InputProvider.enabled = false;
        InputManager.DisableInGame();  
    }
    public void convoUnlock()
    {
        
        ConversationManager.Instance.EndConversation();
        Cursor.lockState = CursorLockMode.Locked;
        InputProvider.enabled = true;
        InputManager.EnableInGame();  
        _player.GetComponent<Rigidbody>().velocity -= _player.GetComponent<Rigidbody>().velocity ;
     
    }

  

    
}
