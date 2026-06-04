using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Sceneを切り替えるために必要

public class TitleMenuManager : MonoBehaviour
{
    // --- Startボタンが押されたときの処理 ---
    public void OnStartButtonClick()
    {
        Debug.Log("ゲーム開始！");
        SceneManager.LoadScene("GameScene");
    }

    // --- Optionsボタンが押されたときの処理 ---
    public void OnOptionsButtonClick()
    {
        Debug.Log("オプション画面を開きます！");
    }

    // --- Quitボタンが押されたときの処理 ---
    public void OnQuitButtonClick()
    {
        Debug.Log("ゲームを終了します！");

        #if UNITY_EDITOR
            // Unityのエディタ上で進行中の場合は、再生を停止する
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // ビルドしたゲームの場合は、ゲームを終了する
            Application.Quit();
        #endif
    }
}
