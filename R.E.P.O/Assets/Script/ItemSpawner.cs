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
        // ランダムなスポーン位置を選ぶ
        int spawnIndex = Random.Range(0, availableSpawns.Count);
        Transform spawn = availableSpawns[spawnIndex];
        availableSpawns.RemoveAt(spawnIndex);

        // ランダムなアイテムを選ぶ
        int itemIndex = Random.Range(0, itemPrefabs.Length);
        GameObject prefab = itemPrefabs[itemIndex];

        // いったん生成
        GameObject item = Instantiate(
            prefab,
            spawn.position,
            spawn.rotation
        );

        // Rendererの高さを取得して地面の上に乗せる
        Renderer renderer = item.GetComponentInChildren<Renderer>();

        //if (renderer != null)
        //{
        //    Bounds bounds = renderer.bounds;

        //    Vector3 pos = item.transform.position;

        //    // モデルの一番下がスポーン地点に来るよう補正
        //    pos.y += bounds.extents.y - (bounds.center.y - item.transform.position.y);

        //    item.transform.position = pos;
        //}
    }
}