using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class inGameMenu : MonoBehaviour
{
    GameObject menu;
    [SerializeField]Slider Sens;
    [SerializeField] CinemachineVirtualCamera _camera;
    [SerializeField]  PlayerMovement player;
    private bool isOpen;
    private void Awake()
    {
        menu = gameObject;
        Sens.minValue = 0.09f;
        Sens.maxValue = 0.1f;
//        print(Sens.value);
        Sens.value = _camera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed;
        Sens.value = _camera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed;
    }

    void Start()
    {
        menu.SetActive(false);
    }

    
    public void Modify()
    {
        _camera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = Sens.value;
        _camera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = Sens.value;
    }
    
    public void OpenMenu()
    {
        if (!isOpen)
        {
            isOpen = true;
            player.DisableInput();
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0;
            menu.SetActive(true);
        }
        else
        {
            player.EnableInput();
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            menu.SetActive(false);
            isOpen = false;
        }
      
    }

 
    // Update is called once per frame
    void Update()
    {
        
    }

  
}