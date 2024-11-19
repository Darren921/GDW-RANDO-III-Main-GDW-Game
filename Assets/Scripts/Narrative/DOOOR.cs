using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DOOOR : MonoBehaviour
{
     private QuestManager questManager;


     private void Start()
     {
         questManager = FindObjectOfType<QuestManager>();
     }

     private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            questManager.convoLock();
            SceneManager.LoadScene("MainMenu");
        }
    }
    public void mains()
    {
        SceneManager.LoadScene("NarrativeTest");
    }
}
