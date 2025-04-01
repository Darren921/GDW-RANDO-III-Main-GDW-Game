using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static bool TutorialActive;
    public static bool firstLoad;
    public static bool loaded;
    private void Start()
    {
        TutorialActive = true;
    }
    private void Update()
    {
       print(TutorialActive);
    }

    public static void TutStop()
    {
        TutorialActive = false;
    }
    public interface IInteractable
    {
        public bool QTEAble{get;set;}
        public bool isHeld{get;set;}
        public void Interact();
        public void HeldInteract();
    }
   
}