using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpawnManager : MonoBehaviour
{
    [Header("SpawnManager")]
    [SerializeField] private ItemObj[] Items;
    [SerializeField] private float minItems;
    private List<GameObject> ItemSpawnPoints;
    private List<int> RandomNum;
    internal List<int> SpawnedList;
    internal List<int> trackedIndexs;
    private bool active;
    private bool firstSpawn;
    // Start is called before the first frame update
    void Start()
    {
        // minItems = 5;
        trackedIndexs = new List<int>();
        SpawnedList = new List<int>();
        ItemSpawnPoints = new List<GameObject>();
        ItemSpawnPoints.AddRange(GameObject.FindGameObjectsWithTag("ItemSpawnPoint"));
        RandomNum = new List<int>();
        for (var i = 0; i < ItemSpawnPoints.Count; i++) RandomNum.Add(i);
    }
    private IEnumerator itemSpawn(ItemObj[] item, int amount, int cap)
    {
        if (active) yield break;

        active = true;

        for (int i = 0; i < item.Length; i++)
        {
            var tag = item[i].thisGameObject.tag;
            var curItemCount = GameObject.FindGameObjectsWithTag(tag).Length;

            // Ensure item count does not exceed limits
            if (curItemCount < cap && curItemCount <= minItems)
            {
                int spawnCount = 0; // Track the number of items spawned in this cycle

                for (var k = 0; k < ItemSpawnPoints.Count && spawnCount < amount; k++)
                {
                    if (curItemCount >= cap) break;

                    var index = Random.Range(0, RandomNum.Count);
                    var sortednum = RandomNum[index];

                    if (!SpawnedList.Contains(index))
                    {
                        // Spawn the item
                        var pickup = Instantiate(
                            item[i].thisGameObject,
                            ItemSpawnPoints[sortednum].transform.position,
                            ItemSpawnPoints[sortednum].transform.rotation
                        );

                        pickup.GetComponent<Tracker>().tracker = sortednum;
                        SpawnedList.Add(sortednum);
                        trackedIndexs.Add(sortednum);
                        spawnCount++; // Increment spawn count
                        curItemCount = GameObject.FindGameObjectsWithTag(tag).Length;
                    }
                }
            }
        }

        yield return new WaitForSeconds(10);
        active = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
        {
            StartCoroutine(itemSpawn(Items,1,5));
        }
    }
}
