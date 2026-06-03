using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] itemPrefabs;
    public Transform[] spawnPoints;

    public int itemCount = 20;

    private List<Transform> availableSpawns;

    void Start()
    {
        availableSpawns = new List<Transform>(spawnPoints);

        for (int i = 0; i < itemCount && availableSpawns.Count > 0; i++)
        {
            SpawnRandomItem();
        }
    }

    void SpawnRandomItem()
    {
        int spawnIndex = Random.Range(0, availableSpawns.Count);
        Transform spawn = availableSpawns[spawnIndex];

        availableSpawns.RemoveAt(spawnIndex);

        int itemIndex = Random.Range(0, itemPrefabs.Length);

        Instantiate(
            itemPrefabs[itemIndex],
            spawn.position,
            spawn.rotation
        );
    }
}