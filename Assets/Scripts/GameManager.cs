using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private GameObject[] Boards;
    private List<GameObject> codeSpawnPoints;
    private List<int> RandomNum;
    [SerializeField]private float Timer;
    [SerializeField] TextMeshProUGUI TimerDisplay;
    [SerializeField] Enemy monsterAI;
    public List<int> comboNums;
    Player player;  
    void Start()
    {
        Boards = GameObject.FindGameObjectsWithTag("Boards");
        
        codeSpawnPoints = new List<GameObject>();
        codeSpawnPoints.AddRange(GameObject.FindGameObjectsWithTag("CodeSpawnPoints"));
        RandomNum = new List<int>();
        for (int i = 0; i < codeSpawnPoints.Count; i++)
        {
            RandomNum.Add(i);
        }
        
        for (int k = 0; k < Boards.Length; k++)
        {        
            int index = Random.Range(0,RandomNum.Count );
            int sortednum = RandomNum[index];
                Boards[k].transform.position = codeSpawnPoints[sortednum].transform.position;
                Boards[k].transform.Rotate( codeSpawnPoints[sortednum].transform.rotation.eulerAngles);
                RandomNum.Remove(sortednum);
        }

        player = FindObjectOfType<Player>();
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
        Timer = 240;
    }


    void Update()
    {
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
        
    }

   
  

}
