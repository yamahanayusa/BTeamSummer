using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoneyBox : MonoBehaviour
{
    [Header("目標金額")]
    public int targetMoney = 5000;

    [Header("現在の合計金額")]
    public int currentMoney = 0;

    [Header("ゲームクリア時に遷移するシーン名")]
    public string clearSceneName = "GameClearScene";

    private void OnTriggerEnter(Collider other)
    {
        // 入ってきたオブジェクトから ItemValue を取得
        ItemValue item = other.GetComponent<ItemValue>();

        // 親や子にアタッチされている場合も考慮して取得
        if (item == null)
        {
            item = other.GetComponentInParent<ItemValue>();
        }

        if (item != null)
        {
            // ItemValue の valueを加算
            currentMoney += item.value;
            Debug.Log($"箱に納品されました！ +${item.value} (合計: ${currentMoney} / ${targetMoney})");

            // 納品したアイテムを消去
            Destroy(item.gameObject);

            // 目標金額に達したか確認
            if (currentMoney >= targetMoney)
            {
                Debug.Log("目標金額達成！ゲームクリア画面へ遷移します。");
                SceneManager.LoadScene(clearSceneName);
            }
        }
    }
}