using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    
    [SerializeField] Image _image ;
    [SerializeField] Sprite deadScreen,escapedScreen;
    [SerializeField] Button restart,lastCheckpoint, mainMenu;


    private void Awake()
    {
        _image.sprite = Player.isDead ? deadScreen : escapedScreen;
        lastCheckpoint.gameObject.SetActive(Player.isDead);
    }

    private void Start()
    {
   
    }

    public void  goToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void restartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void GoToLastCheckpoint()
    {
       // SceneManager.LoadScene("MainScene");
    }
    
    
    
    
}
