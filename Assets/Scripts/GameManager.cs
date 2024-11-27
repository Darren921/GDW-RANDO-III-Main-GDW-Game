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
    private GameObject Battery,Fuel;
    private float batteriesInScene,fuelInScene;
    private float minItems;
    private List<GameObject> ItemSpawnPoints;
    private List<int> RandomNum;
    private List<int> discaredNum;
    void Start()
    {
      //  Boards = GameObject.FindGameObjectsWithTag("Boards");
      ItemSpawnPoints = new List<GameObject>();
      ItemSpawnPoints.AddRange(GameObject.FindGameObjectsWithTag("ItemSpawnPoint"));
        RandomNum = new List<int>();
        for (int i = 0; i < ItemSpawnPoints.Count; i++)
        {
            RandomNum.Add(i);
        }
        
        for (int k = 0; k < ItemSpawnPoints.Count; k++)
        {        
            int index = Random.Range(0,RandomNum.Count );
            int sortednum = RandomNum[index];
               
                RandomNum.Remove(sortednum);
                discaredNum.Add(sortednum);
        }

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


    void Update()
    {
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
