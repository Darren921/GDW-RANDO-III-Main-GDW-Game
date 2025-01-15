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
    
    
    
    [Header("SpawnManager")]
    [SerializeField] private GameObject Battery, Fuel;
    internal float batteriesInScene, fuelInScene,targetF,targetB;
    [SerializeField] private float minItems;
    private List<GameObject> ItemSpawnPoints;
    private List<int> RandomNum;
    internal List<int> SpawnedList;
    internal List<int> trackedIndexs;
    private bool active;
    private bool firstSpawn;

    [Header("Frost")]
    bool isFreezing;
    private float _frost;
    [SerializeField] private float maxFrost;
    [SerializeField] private Material frostTexture;
    private float _curOpacity;
    
    
    [Header("References")]
    [SerializeField] private Player _player;

    private void Start()
    {
        _curOpacity = -1;
            minItems = 5;
        //  Boards = GameObject.FindGameObjectsWithTag("Boards");
        trackedIndexs = new List<int>();
        SpawnedList = new List<int>();
        ItemSpawnPoints = new List<GameObject>();
        ItemSpawnPoints.AddRange(GameObject.FindGameObjectsWithTag("ItemSpawnPoint"));
        RandomNum = new List<int>();
        for (var i = 0; i < ItemSpawnPoints.Count; i++) RandomNum.Add(i);
        //use this to toggle off and on freezing in areas, maybe tut area?
        isFreezing = true;
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
    

    private IEnumerator itemSpawn(int amount,int cap)
    {
        if (!active)
        {
            active = true;
            if(fuelInScene < minItems && fuelInScene <= cap) 
            {
                for (var k = 0; k <= ItemSpawnPoints.Count; k++)
                    if (fuelInScene < minItems)
                    {
                        var index = Random.Range(0, RandomNum.Count);
                        var sortednum = RandomNum[index];


                        if (!SpawnedList.Contains(index) && fuelInScene < cap)
                        {
                            var fuelPickup = Instantiate(Fuel, ItemSpawnPoints[sortednum].transform.position,
                                ItemSpawnPoints[sortednum].transform.rotation);
                            fuelPickup.GetComponent<Tracker>().tracker = sortednum;
                            SpawnedList.Add(sortednum);
                            fuelInScene = GameObject.FindGameObjectsWithTag("Fuel").Length;
                            trackedIndexs.Add(sortednum);
                        }


                        if (SpawnedList.Contains(index) && fuelInScene < minItems)
                            for (var l = 0; l < ItemSpawnPoints.Count; l++)
                                if (!SpawnedList.Contains(index) && fuelInScene < minItems && fuelInScene < targetF)
                                {
                                    var fuelPickup = Instantiate(Fuel, ItemSpawnPoints[l].transform.position,
                                        ItemSpawnPoints[l].transform.rotation);
                                    fuelPickup.GetComponent<Tracker>().tracker = sortednum;
                                    fuelInScene = GameObject.FindGameObjectsWithTag("Fuel").Length;
                                    trackedIndexs.Add(k);
                                }
                    }
            }
            
            if (batteriesInScene <= targetB && batteriesInScene < minItems)
            {
                for (var k = 0; k <= ItemSpawnPoints.Count; k++)
                    if (batteriesInScene < minItems)
                    {
                        var index = Random.Range(0, RandomNum.Count);
                        var sortednum = RandomNum[index];
                        if (batteriesInScene < minItems)
                        {

                            if (!SpawnedList.Contains(index) && batteriesInScene < targetB)
                            {
                                var batteryPickup = Instantiate(Battery, ItemSpawnPoints[sortednum].transform.position,
                                    ItemSpawnPoints[sortednum].transform.rotation);
                                batteryPickup.GetComponent<Tracker>().tracker = sortednum;
                                SpawnedList.Add(sortednum);
                                batteriesInScene = GameObject.FindGameObjectsWithTag("Batteries").Length;
                                trackedIndexs.Add(sortednum);
                            }

                            if (SpawnedList.Contains(index) && batteriesInScene < minItems &&
                                batteriesInScene < targetB)
                                for (var l = 0; l < ItemSpawnPoints.Count; l++)
                                    if (!SpawnedList.Contains(index) && batteriesInScene < minItems)
                                    {
                                        var batteryPickup = Instantiate(Battery,
                                            ItemSpawnPoints[index].transform.position,
                                            ItemSpawnPoints[index].transform.rotation);
                                        batteryPickup.GetComponent<Tracker>().tracker = sortednum;
                                        batteriesInScene = GameObject.FindGameObjectsWithTag("Batteries").Length;
                                        trackedIndexs.Add(sortednum);
                                    }
                        }
                    }
            }

            Debug.Log($"Updated Fuel in Scene: {fuelInScene}, Updated Batteries in Scene: {batteriesInScene}");
            yield return new WaitForSeconds(10);
            active = false;
        }
    }


    private void Update()
    {
         if(!active )
         {
             StartCoroutine(itemSpawn(1,5));
         }
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