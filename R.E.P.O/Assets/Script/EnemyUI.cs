using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyUI : MonoBehaviour
{
    [Header("頭上のテキストコンポーネント")]
    public TextMeshProUGUI markText;

    void Start()
    {
        // ゲーム起動時は表示しない
        ClearMark();
    }

    // --- 文字を「？」にする命令 ---
    public void ShowAlertMark()
    {
        if (markText != null) markText.text = "?";
    }

    // --- 文字を「！」にする命令 ---
    public void ShowChaseMark()
    {
        if(markText != null) markText.text = "!";
    }

    // --- 文字を消す命令 ---
    public void ClearMark()
    {
        if(markText != null) markText.text = "";
    }
}
