using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    int MaxspawnLimit = 10;
    [SerializeField] List<int> SpawnPointIndex = new List<int>();
    internal List<int> AvaiableSpawns = new List<int>();
    internal List<int> TrackedIndex = new List<int>();
    List<GameObject> SpawnPoints = new List<GameObject>();
    [SerializeField]  internal List<ItemObj>  RandomItems = new List<ItemObj>();
    private static ItemObj lastSelectedItem = null; 
    
    
    
    
    private bool active;
    private bool firstSpawn;
    int spawnedItems;

    private void Awake()
    {
        SpawnPoints = GameObject.FindGameObjectsWithTag("ItemSpawnPoint").ToList();
        for (int i = 0; i < SpawnPoints.Count; i++)
        {
            SpawnPointIndex.Add(i);
        }
        SpawnPointIndex =  SpawnPointIndex.OrderBy(_ => Random.value).ToList();
        AvaiableSpawns = SpawnPointIndex.ToList();
       
    }

    void Start()
    {
    }

    private IEnumerator SpawnItems( int SpawnCycleCap)
    {
        var RandomItem = ShuffleRandomItems();
        var tag = RandomItem[0].prefab.tag;
        var curerentItemSpawn = GameObject.FindGameObjectsWithTag(tag).Length;
        if (active) yield break;
        active = true;
        var spawnedCount = 0;
        for(int i = 0; i < SpawnPointIndex.Count; i++)
        {
            RandomItem = ShuffleRandomItems();
            tag = RandomItem[0].prefab.tag;            
            curerentItemSpawn = GameObject.FindGameObjectsWithTag(tag).Length;
            print($"{tag} spawned with {curerentItemSpawn} and {SpawnPointIndex[i]}");
            print($"RandomItem: {RandomItem[0]} + {curerentItemSpawn}");
            if (spawnedCount < SpawnCycleCap && curerentItemSpawn < RandomItem[0].data.ItemSpawnLimit)
            {
                if (!TrackedIndex.Contains(SpawnPointIndex[i]))
                {
                   
                    var SpawnedItem = Instantiate(RandomItem[0].prefab,SpawnPoints[SpawnPointIndex[i]].transform.position,SpawnPoints[SpawnPointIndex[i]].transform.rotation );
                    TrackedIndex.Add(SpawnPointIndex[i]);
                    if (AvaiableSpawns.Contains(SpawnPointIndex[i]))
                    {
                        AvaiableSpawns.Remove(SpawnPointIndex[i]);
                    }
                    SpawnedItem.GetComponent<GroundObj>().tracker = SpawnPointIndex[i];
                    spawnedCount++;
                }
            }
        }
        yield return new WaitForSecondsRealtime(20);
        active = false;
        print("spawning ended");
    }

    private ItemObj lastItem;
    private List<ItemObj> ShuffleRandomItems()  
    {  
        List<ItemObj> ShuffledItems  = new List<ItemObj>();
        for (int i = RandomItems.Count - 1; i > 0; i--)
        {
            var rnd = Random.Range(0,i);
            // ReSharper disable once SwapViaDeconstruction

            var tempStorage = RandomItems[i];
            lastItem = RandomItems[i];
            while (RandomItems[i] == lastItem && i > 1)
            {
                rnd = Random.Range(0,i);
            }
            RandomItems[i] = RandomItems[rnd];
            
            RandomItems[rnd] = tempStorage;
        }
        ShuffledItems.AddRange(RandomItems);
        return ShuffledItems;
    }

    void Update()
    {
        if (!active)
        {
            if (!firstSpawn)
            {
                firstSpawn = true;
                StartCoroutine(SpawnItems( MaxspawnLimit));
            }
            else
            {
                StartCoroutine(SpawnItems(RandomItems.Count));
            }
        }
    }
}
