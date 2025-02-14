using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static bool firstLoad;
    internal PlayerHotbar playerHotbar;
    private void Awake()
    {
        PlayerPrefs.SetString("StartingItemsGiven", "false");
        PlayerPrefs.Save();
        playerHotbar = FindObjectOfType<PlayerHotbar>();
        firstLoad = true;
    }

    private void Start()
    {
    }
    private void Update()
    {
       
    }
   
    public interface IInteractable
    {
        public bool isHeld{get;set;}
        public void Interact();
        public void HeldInteract();
    }
   
}