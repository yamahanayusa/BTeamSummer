using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Text References")]
    [SerializeField] private TextMeshProUGUI hpText; // HP表示用
    [SerializeField] private TextMeshProUGUI currentMoneyText; // 現在の所持金表示用
    [SerializeField] private TextMeshProUGUI targetMoneyText; // 目標金額表示用

    // HPの表示を更新する
    public void UpdateHP(int currentHealth)
    {
        if (hpText != null)
        {
            hpText.text = "HP: " + currentHealth;
        }
    }

    // 所持金の表示を更新する
    public void UpdateCurrentMoney(int currentMoney)
    {
        if (currentMoneyText != null)
        {
            currentMoneyText.text = "$" + currentMoney;
        }
    }

    // 目標金額の表示をセットする
    public void SetTargetMoney(int targetMoney)
    {
        if (targetMoneyText != null)
        {
            targetMoneyText.text = "$" + targetMoney;
        }
    }
}