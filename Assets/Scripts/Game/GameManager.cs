using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static bool firstLoad;
    public static bool loaded;
    private void Start()
    {

    }
    private void Update()
    {
       
    }
   
    public interface IInteractable
    {
        public bool QTEAble{get;set;}
        public bool isHeld{get;set;}
        public void Interact();
        public void HeldInteract();
    }
   
}