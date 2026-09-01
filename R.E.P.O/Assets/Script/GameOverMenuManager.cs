using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenuManager : MonoBehaviour
{
    [Header("遷移先シーン名")]
    [SerializeField] private string gameSceneName = "GameScene";
    [SerializeField] private string titleSceneName = "TitleScene";

    private void Start()
    {
        // マウスカーソルの表示
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // ゲームオーバー画面が開いた時は確実に時間を動かす
        Time.timeScale = 1.0f;
    }

    // --- Retryボタンが押されたとき ---
    public void OnRetryButtonClick()
    {
        Debug.Log("ゲームをリトライします！");
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(gameSceneName);
    }

    // --- Titleボタンが押されたとき ---
    public void OnTitleButtonClick()
    {
        Debug.Log("タイトル画面へ戻ります！");
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(titleSceneName);
    }

    // --- Quitボタンが押されたとき ---
    public void OnQuitButtonClick()
    {
        Debug.Log("ゲームを終了します！");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}