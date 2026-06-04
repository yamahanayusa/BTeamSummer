using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleGhost : MonoBehaviour
{
    [Header("動きの激しさ（数値が大きいほど素早く動く）")]
    public float moveSpeed = 0.3f;

    [Header("ランダムに動き回る範囲")]
    public float moveRange = 500.0f;

    private Vector3 startPosition;
    private float randomOffset;

    void Start()
    {
        // 始まった瞬間の、元の位置を記憶
        startPosition = transform.position;

        // 毎回違う動きにするためのランダムな初期値
        randomOffset = Random.Range(0.0f, 100.0f);
    }

    void Update()
    {
        // PerlinNoiseを使って、なめらかな不規則な値を計算する
        // timeX と timeY をずらすことで、縦と横で違う動きにさせる
        float timeX = Time.time * moveSpeed + randomOffset;
        float timeY = Time.time * moveSpeed + randomOffset + 50.0f;

        // Mathf.PerlinNoiseは 0?1 の値を返すので、-0.5?0.5 に調整して範囲をかける
        float xOffset = (Mathf.PerlinNoise(timeX, 0.0f) - 0.5f) * moveRange * 2.0f;
        float yOffset = (Mathf.PerlinNoise(0.0f, timeY) - 0.5f) * moveRange * 2.0f;

        // 計算したランダムな位置をゴーストに適用
        transform.position = new Vector3(startPosition.x + xOffset, startPosition.y + yOffset, startPosition.z);
    }
}