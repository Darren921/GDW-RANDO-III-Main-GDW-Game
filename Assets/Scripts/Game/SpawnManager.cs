using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class SpawnManager : MonoBehaviour
{
    [Header("SpawnManager")]
    [SerializeField] private ItemObj[] Items;
    [SerializeField] private int MaxItems;
    private List<GameObject> ItemSpawnPoints;
    private List<int> RandomNum;
    internal List<int> SpawnedList;
    internal List<int> trackedIndexs;
    private bool active;
    private bool firstSpawn;

    void Start()
    {
        firstSpawn = true;
        trackedIndexs = new List<int>();
        SpawnedList = new List<int>();
        ItemSpawnPoints = new List<GameObject>();
        ItemSpawnPoints.AddRange(GameObject.FindGameObjectsWithTag("ItemSpawnPoint"));
        RandomNum = new List<int>();
        for (var i = 0; i < ItemSpawnPoints.Count; i++) RandomNum.Add(i);
    }
    
    private IEnumerator itemSpawn(ItemObj[] item, int amount)
    {
        if (active) yield break;

        active = true;
      
        int spawnCount = 0; // Total spawned items during this cycle

        for (int i = 0; i < item.Length && SpawnedList.Count < MaxItems; i++)
        {
            var tag = item[i].prefab.tag;
            var curItemCount = GameObject.FindGameObjectsWithTag(tag).Length;
            var cap = item[i].itemLimit;

            if (curItemCount < cap && SpawnedList.Count < MaxItems)
            {
                // get available spawn points, and randomize them
                var availableSpawns = RandomNum.Except(SpawnedList).OrderBy(x => Random.value).ToList();
                if (availableSpawns.Count == 0)
                {
                    Debug.LogWarning("No available spawn points!");
                    break;
                }

                foreach (var spawn in availableSpawns)
                {
                    if (curItemCount >= cap || spawnCount >= amount || SpawnedList.Count >= MaxItems) break;

                    float SpawnChance = Random.value;
                    if (SpawnChance <= item[i].itemRarity)
                    {
                        if (Physics.Raycast(ItemSpawnPoints[spawn].transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity))
                        {
                            var pickup = Instantiate(
                                item[i].prefab,
                                ItemSpawnPoints[spawn].transform.position,
                                ItemSpawnPoints[spawn].transform.rotation
                            );
                            var collider = pickup.GetComponent<Collider>();
                            if (collider is not null)
                            {
                                float objHeight = collider.bounds.extents.y;
                                pickup.transform.position = hit.point + Vector3.up * objHeight;
                            }

                            pickup.GetComponent<GroundObj>().tracker = spawn;
                            SpawnedList.Add(spawn);
                            trackedIndexs.Add(spawn);
                            spawnCount++;
                            curItemCount = GameObject.FindGameObjectsWithTag(tag).Length;
                        }
                    }
                }
            }
            else
            {
                Debug.Log($"Skipping spawn for tag {tag} (Current count: {curItemCount}, Cap: {cap})");
            }
            if (curItemCount < cap && SpawnedList.Count < MaxItems)
            {
                // get available spawn points, and randomize them
                var availableSpawns = RandomNum.Except(SpawnedList).OrderBy(x => Random.value).ToList();
                if (availableSpawns.Count == 0)
                {
                    Debug.LogWarning("No available spawn points!");
                    break;
                }

                foreach (var spawn in availableSpawns)
                {
                //    Debug.Log("Fallback activated, filling in leftover items");
                    if (curItemCount >= cap || spawnCount >= amount || SpawnedList.Count >= MaxItems) break;


                    if (Physics.Raycast(ItemSpawnPoints[spawn].transform.position, Vector3.down, out RaycastHit hit,
                            Mathf.Infinity))
                    {
                        var pickup = Instantiate(
                            item[i].prefab,
                            ItemSpawnPoints[spawn].transform.position,
                            ItemSpawnPoints[spawn].transform.rotation
                        );
                        var collider = pickup.GetComponent<Collider>();
                        if (collider is not null)
                        {
                            float objHeight = collider.bounds.extents.y;
                            pickup.transform.position = hit.point + Vector3.up * objHeight;
                        }

                        pickup.GetComponent<GroundObj>().tracker = spawn;
                        SpawnedList.Add(spawn);
                        trackedIndexs.Add(spawn);
                        spawnCount++;
                        curItemCount = GameObject.FindGameObjectsWithTag(tag).Length;

                    }
                }
            }
        }
//        Debug.Log($"Spawning complete. Total items spawned: {spawnCount}/{amount}.");
        yield return new WaitForSecondsRealtime(15);
        active = false;
        firstSpawn = false;
    }

    void Update()
    {
        if (!active)
        {
            if (firstSpawn)
            {
                StartCoroutine(itemSpawn(Items,MaxItems));
            }
            StartCoroutine(itemSpawn(Items, 1));
        }
    }
}
