using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("SpawnManager")] [SerializeField]
    private ItemObj[] Items;

    [SerializeField] private int MaxItems;
    private List<GameObject> ItemSpawnPoints;
    private List<int> RandomNum;
    internal List<int> SpawnedList;
    internal List<int> trackedIndexs;
    private bool active;

    private bool firstSpawn;

    // Start is called before the first frame update
    void Start()
    {
        firstSpawn = true;
        // minItems = 5;
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
            var tag = item[i].thisGameObject.tag;
            var curItemCount = GameObject.FindGameObjectsWithTag(tag).Length;
            int cap = item[i].itemLimit;

            if (curItemCount < cap && SpawnedList.Count < MaxItems)
            {
                // Get available spawn points
                var availableSpawns = RandomNum.Except(SpawnedList).OrderBy(x => Random.value).ToList();
                if (availableSpawns.Count == 0)
                {
                    Debug.LogWarning("No available spawn points!");
                    break;
                }

                // Primary spawn attempt based on rarity
                foreach (var sortednum in availableSpawns)
                {
                    if (curItemCount >= cap || spawnCount >= amount || SpawnedList.Count >= MaxItems) break;

                    float rarityRoll = Random.value;
                    if (rarityRoll <= item[i].itemRarity)
                    {
                        if (Physics.Raycast(ItemSpawnPoints[sortednum].transform.position, Vector3.down,
                                out RaycastHit hit,
                                Mathf.Infinity))
                        {
                            var pickUp = Instantiate(item[i].thisGameObject,
                                ItemSpawnPoints[sortednum].transform.position,
                                ItemSpawnPoints[sortednum].transform.rotation);

                            var Collider = pickUp.GetComponent<Collider>();
                            if (Collider is not null)
                            {
                                float objHeight = Collider.bounds.extents.y;
                                pickUp.transform.position = hit.point + Vector3.up * objHeight;
                            }

                            SpawnedList.Add(sortednum);
                            trackedIndexs.Add(sortednum);
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
        }

        // Fallback logic after cycling through all items
        if (spawnCount < amount && SpawnedList.Count < MaxItems)
        {
            Debug.Log(
                "Fallback: Attempting to fill remaining slots without rarity checks after cycling through all items.");

            foreach (var itemObj in item)
            {
                var tag = itemObj.thisGameObject.tag;
                var curItemCount = GameObject.FindGameObjectsWithTag(tag).Length;
                int cap = itemObj.itemLimit;

                if (curItemCount < cap && SpawnedList.Count < MaxItems)
                {
                    var availableSpawns = RandomNum.Except(SpawnedList).OrderBy(x => Random.value).ToList();

                    foreach (var sortednum in availableSpawns)
                    {
                        if (curItemCount >= cap || spawnCount >= amount || SpawnedList.Count >= MaxItems) break;

                        if (Physics.Raycast(ItemSpawnPoints[sortednum].transform.position, Vector3.down,
                                out RaycastHit hit,
                                Mathf.Infinity))
                        {
                            var pickUp = Instantiate(itemObj.thisGameObject,
                                ItemSpawnPoints[sortednum].transform.position,
                                ItemSpawnPoints[sortednum].transform.rotation);

                            var Collider = pickUp.GetComponent<Collider>();
                            if (Collider is not null)
                            {
                                float objHeight = Collider.bounds.extents.y;
                                pickUp.transform.position = hit.point + Vector3.up * objHeight;
                            }

                            SpawnedList.Add(sortednum);
                            trackedIndexs.Add(sortednum);
                            spawnCount++;
                            curItemCount = GameObject.FindGameObjectsWithTag(tag).Length;
                        }
                    }
                }
            }
        }

        Debug.Log($"Spawning complete. Total items spawned: {spawnCount}/{amount}.");

        yield return new WaitForSeconds(20);
        active = false;
        firstSpawn = false;
    }

// Update is called once per frame
    void Update()
    {
        if (!active)
        {
            if (firstSpawn)
            {
                StartCoroutine(itemSpawn(Items, 10));
            }

            StartCoroutine(itemSpawn(Items, 1));
        }
    }
}

  

