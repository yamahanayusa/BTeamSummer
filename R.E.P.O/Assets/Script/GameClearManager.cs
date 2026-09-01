using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearManager : MonoBehaviour
{
    private void Start()
    {
        // マウスカーソルの表示
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // TITLEボタン
    public void OnClickTitleButton()
    {
        SceneManager.LoadScene("TitleScene");
    }

    // QUITボタン
    public void OnClickQuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // ビルド後のアプリ用
#endif
    }
}