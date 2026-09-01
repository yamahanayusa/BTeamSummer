using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("遷移先のゲームオーバーシーン名")]
    public string gameOverSceneName = "GameOverScene";

    private bool isGameOver = false;

    // PlayerHealthのDie()から呼び出されるメソッド
    public void TriggerGameOver()
    {
        // 既にゲームオーバー処理が走っていたら何もしない
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("ゲームオーバー！シーンを切り替えます");

        SceneManager.LoadScene(gameOverSceneName);
    }
}