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
    [SerializeField]private GameObject Battery,Fuel;
    private float batteriesInScene,fuelInScene;
    private float minItems;
    private List<GameObject> ItemSpawnPoints;
    private List<int> RandomNum,discaredNum,SpawnedList;
   
    bool active;

    void Start()
    {
        minItems = 2;
      //  Boards = GameObject.FindGameObjectsWithTag("Boards");
      discaredNum = new List<int>();
      SpawnedList = new List<int>();
      ItemSpawnPoints = new List<GameObject>();
      ItemSpawnPoints.AddRange(GameObject.FindGameObjectsWithTag("ItemSpawnPoint"));
        RandomNum = new List<int>();
        for (int i = 0; i < ItemSpawnPoints.Count; i++)
        {
            RandomNum.Add(i);
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

    private IEnumerator itemSpawn()
    {
        if (!active)
        {
            active = true;
           
            if (fuelInScene < minItems)
            {
                for (int k = 0; k <= ItemSpawnPoints.Count; k++)
                {
                    int index = Random.Range(0, 1);
                    int sortednum = RandomNum[index];
                    if (fuelInScene < minItems)
                    {
                        if (!SpawnedList.Contains(index))
                        {
                          var fuelPickup =  Instantiate(Fuel, ItemSpawnPoints[sortednum].transform.position, Quaternion.identity);
                            SpawnedList.Add(sortednum);
                            fuelInScene = GameObject.FindGameObjectsWithTag("Fuel").Length;

                        }

                        if (SpawnedList.Contains(sortednum) && fuelInScene < minItems)
                        {
                            for (int l = 0; l <= ItemSpawnPoints.Count; l++)
                            {
                                if (!SpawnedList.Contains(sortednum) && fuelInScene < minItems)
                                {
                                    fuelInScene = GameObject.FindGameObjectsWithTag("Fuel").Length;
                                    Instantiate(Fuel, ItemSpawnPoints[k].transform.position, Quaternion.identity);
                                }
                            }
                           
                        }
                       
                    }
                   
                }
                 
                
            }
            else if (batteriesInScene < minItems)
            {
            
            }
            
            batteriesInScene = GameObject.FindGameObjectsWithTag("Batteries").Length;
            yield return new WaitForSeconds(10);
            active = false;

        }
        
      
    }


    void Update()
    {
        if (!active)
        {
            print("Start");
            StartCoroutine(itemSpawn());
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
