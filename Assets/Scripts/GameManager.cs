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
    internal float batteriesInScene,fuelInScene;
    private float minItems;
    private List<GameObject> ItemSpawnPoints;
    private List<int> RandomNum;
    internal List<int> SpawnedList;
    internal List<int> trackedIndexs;
    bool active;

    void Start()
    {
        minItems = 2;
      //  Boards = GameObject.FindGameObjectsWithTag("Boards");
      trackedIndexs = new List<int>();
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
                for (int k = 0; k < ItemSpawnPoints.Count; k++)
                {
                    int index = Random.Range(0, RandomNum.Count);
                    int sortednum = RandomNum[index];
                    if (fuelInScene < minItems)
                    {
                        if (!SpawnedList.Contains(index))
                        {
                          var fuelPickup =  Instantiate(Fuel, ItemSpawnPoints[sortednum].transform.position, Quaternion.identity);
                          fuelPickup.GetComponent<Tracker>().tracker = sortednum;
                            SpawnedList.Add(sortednum);
                            fuelInScene = GameObject.FindGameObjectsWithTag("Fuel").Length;
                            trackedIndexs.Add(sortednum);
                        }

                        if (SpawnedList.Contains(index) && fuelInScene < minItems)
                        {
                            for (int l = 0; l <= ItemSpawnPoints.Count; l++)
                            {
                                if (!SpawnedList.Contains(index) && fuelInScene < minItems)
                                {
                                    fuelInScene = GameObject.FindGameObjectsWithTag("Fuel").Length;
                                    var fuelPickup = Instantiate(Fuel, ItemSpawnPoints[k].transform.position, Quaternion.identity);
                                    fuelPickup.GetComponent<Tracker>().tracker = sortednum;
                                    trackedIndexs.Add(k);
                                    
                                }
                            }
                           
                        }
                       
                    }
                   
                }

               
                 
                
            }
            //WIP
            else if (batteriesInScene < minItems)
            {
                for (int k = 0; k < ItemSpawnPoints.Count; k++)
                {
                    int index = Random.Range(0, RandomNum.Count);
                    int sortednum = RandomNum[index];
                    if (batteriesInScene < minItems)
                    {
                        if (!SpawnedList.Contains(index))
                        {
                            var batteryPickup  =  Instantiate(Fuel, ItemSpawnPoints[sortednum].transform.position, Quaternion.identity);
                            battries.GetComponent<Tracker>().tracker = sortednum;
                            SpawnedList.Add(sortednum);
                            fuelInScene = GameObject.FindGameObjectsWithTag("Fuel").Length;
                            trackedIndexs.Add(sortednum);
                        }

                        if (SpawnedList.Contains(index) && fuelInScene < minItems)
                        {
                            for (int l = 0; l <= ItemSpawnPoints.Count; l++)
                            {
                                if (!SpawnedList.Contains(index) && fuelInScene < minItems)
                                {
                                    fuelInScene = GameObject.FindGameObjectsWithTag("Fuel").Length;
                                    var fuelPickup = Instantiate(Fuel, ItemSpawnPoints[k].transform.position, Quaternion.identity);
                                    fuelPickup.GetComponent<Tracker>().tracker = sortednum;
                                    trackedIndexs.Add(k);
                                    
                                }
                            }
                           
                        }
                       
                    }
                   
                }
            }
            
            batteriesInScene = GameObject.FindGameObjectsWithTag("Batteries").Length;
            yield return new WaitForSeconds(10);
            active = false;

        }
        
      
    }


    public void DeleteIndexes()
    {
        
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
