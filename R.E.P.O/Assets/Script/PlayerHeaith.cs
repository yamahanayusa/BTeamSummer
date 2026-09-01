using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    [Header("無敵時間の設定")]
    public float invincibilityTime = 1.0f; // ダメージを受けた後の無敵時間
    private bool isInvincible = false;     // 現在無敵状態かどうか

    private GameOverManager gameOverManager;
    private UIManager uiManager;

    void Start()
    {
        currentHealth = maxHealth;
        gameOverManager = FindObjectOfType<GameOverManager>();

        // シーンからUIManagerを見つける
        uiManager = FindObjectOfType<UIManager>();

        // ゲーム開始時のHPをUIに反映
        if (uiManager != null)
        {
            uiManager.UpdateHP(currentHealth);
        }
    }

    // ダメージを受ける処理
    public void TakeDamage(int damage)
    {
        // 無敵時間中ならダメージ処理をスキップ
        if (isInvincible) return;

        currentHealth -= damage;
        Debug.Log($"プレイヤーがダメージを受けた！ 残りHP: {currentHealth}");

        // ダメージを受けたらUIを更新
        if (uiManager != null)
        {
            uiManager.UpdateHP(currentHealth);
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        else
        {
            // 生きていたら無敵時間をスタート
            StartCoroutine(InvincibilityRoutine());
        }
    }

    // 無敵時間をカウントする
    private System.Collections.IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;
    }

    void Die()
    {
        Debug.Log("プレイヤーは倒れた...");
        if (gameOverManager != null)
        {
            gameOverManager.TriggerGameOver();
        }
    }
}