using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Sceneを切り替えるために必要

public class TitleMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject optionsCanvas;
    [SerializeField] private Slider audioSlider;
    [SerializeField] private Slider sensitivitySlider;

    // ゲーム全体で音量と感度を共有するための変数
    public static float MasterVolume = 0.7f;
    public static float MouseSensitivity = 2.0f;

    private void Start()
    {
        // 起動時には確実に設定画面を非表示にする
        if (optionsCanvas != null)
        {
            optionsCanvas.SetActive(false);
        }

        // 時間を確実に動かす
        Time.timeScale = 1.0f;

        // ゲーム起動時に、現在の数値をスライダーの見た目に反映させる
        if (audioSlider != null) audioSlider.value = MasterVolume;
        if (sensitivitySlider != null) sensitivitySlider.value = MouseSensitivity;
    }

    private void Update()
    {
        if (!Input.GetKeyUp(KeyCode.Escape)) return;
        if (optionsCanvas == null) return;

        // 「キーが押され、キャンバスもあるとき」だけが通る
        bool isActive = optionsCanvas.activeSelf;
        optionsCanvas.SetActive(!isActive);

        // 開閉に合わせて時間を止める / 動かす
        if (!isActive)
        {
            Time.timeScale = 0.0f;
            if (audioSlider != null) audioSlider.value = MasterVolume;
            if (sensitivitySlider != null) sensitivitySlider.value = MouseSensitivity;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    // --- Startボタンが押されたときの処理 ---
    public void OnStartButtonClick()
    {
        Debug.Log("ゲーム開始！");
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("GameScene");
    }

    // --- Optionsボタンが押されたときの処理 ---
    public void OnOptionsButtonClick()
    {
        Debug.Log("オプション画面を開きます！");
        if (optionsCanvas != null)
        {
            optionsCanvas.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }
    // --- Backボタンが押されたときの処理 ---
    public void OnBackButtonClick()
    {
        Debug.Log("オプション画面を閉じます！");
        if (optionsCanvas != null)
        {
            optionsCanvas.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }

    // --- 音量スライダーが動いたときに呼ばれる ---
    public void OnAudioValueChanged(float value)
    {
        MasterVolume = value;
        AudioListener.volume = MasterVolume;
    }

    // --- 感度スライダーが動いたときに呼ばれる ---
    public void OnSensitivityValueChanged(float value)
    {
        MouseSensitivity = value;
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
