using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [Header("ダメージ量の設定")]
    public int damageAmount = 20; // 1回の接触で与えるダメージ

    // 物理的な衝突
    private void OnCollisionEnter(Collision collision)
    {
        // ぶつかった相手が "Player" か確認
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damageAmount);
            }
        }
    }

    // すり抜ける判定
    private void OnTriggerEnter(Collider other)
    {
        // 進入した相手のが "Player" か確認
        if (other.CompareTag("Player"))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damageAmount);
            }
        }
    }
}