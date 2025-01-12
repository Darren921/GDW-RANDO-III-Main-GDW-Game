using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    //  [SerializeField]private float Timer;
    // [SerializeField] TextMeshProUGUI TimerDisplay;
    //  [SerializeField] Enemy monsterAI;
    
    
    
   

    [Header("Frost")]
    bool isFreezing;
    [SerializeField]private float _frost;
    [SerializeField] private float maxFrost;
    [SerializeField] private Material frostTexture;
    private float _curOpacity;
    
    
    [Header("References")]
    [SerializeField] private Player _player;

    private void Start()
    {
        _curOpacity = -1;
     
        //use this to toggle off and on freezing in areas, maybe tut area?
       // isFreezing = true;
        _frost = 0;
        /*
        codePanel = FindObjectOfType<CodePanel>();
        for (int j = 0; j < 4; j++)
        {
            comboNums.Add(Random.Range(0,9));
        }

        foreach(int values in comboNums)
        {
            print(values);
        }

        foreach (var item in comboNums)
        {
            codePanel.comboNumstemp.Add(item);
        }
        */
        //  Timer = 240;
    }
    

   


    private void Update()
    {
       
         if(_player is null) return;
         switch (isFreezing)
         {
             case false:
                 return;
             case true when _player.returnTorchState():
             {
                 _frost -= Time.deltaTime;
                 _curOpacity -= 0.01f * Time.deltaTime;
                 frostTexture.SetFloat("_Opacity",_curOpacity ); 
                 if (_frost <= 0)
                 {
                     _frost = 0;
                 }

                 break;
             }
             case true when !_player.returnTorchState():
             {
                 _frost += Time.deltaTime;
                 _curOpacity += 0.01f * Time.deltaTime;
                 frostTexture.SetFloat("_Opacity",_curOpacity ); 
                 if (_frost >= maxFrost)
                 {
                     _frost = 0;
                     frostTexture.SetFloat("_Opacity",0 ); 

                     StartCoroutine(_player.LookatDeath());
                 }

                 break;
             }
         }


         /*
        float minutes = Mathf.FloorToInt(Timer / 60f);
        float seconds = Mathf.FloorToInt(Timer - minutes * 60);
        string displayTime = string.Format("{0:00}:{1:00}", minutes, seconds);
        TimerDisplay.text = "Time till the HUNT: " + displayTime;
        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
            Timer = 0;
            TimerDisplay.text = "YOU CAN'T HIDE!";
            monsterAI._IsHunting = true;
        }
        */
    }

   
}