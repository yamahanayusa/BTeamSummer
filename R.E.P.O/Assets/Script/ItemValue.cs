using UnityEngine;
using TMPro;

public class ItemValue : MonoBehaviour
{
    [HideInInspector]
    public int value;

    private TextMeshPro priceText;

    [Header("金額表示のワールドサイズ")]
    public float worldTextSize = 0.3f;

    void Start()
    {
        // 300～600円をランダムで設定
        value = Random.Range(300, 601);

        // 子オブジェクトからTextMeshProを探す
        priceText = GetComponentInChildren<TextMeshPro>();

        if (priceText != null)
        {
            priceText.text = "$" + value;

            // Font Sizeを統一
            priceText.fontSize = 36;
            priceText.enableAutoSizing = false;

            // 親のScaleの影響を受けないワールドサイズにする
            SetWorldScale(priceText.transform, worldTextSize);
        }

        Debug.Log(gameObject.name + " の価値は " + value + "円");
    }

    void SetWorldScale(Transform target, float size)
    {
        Vector3 parentScale = target.parent != null
            ? target.parent.lossyScale
            : Vector3.one;

        target.localScale = new Vector3(
            size / parentScale.x,
            size / parentScale.y,
            size / parentScale.z
        );
    }

    void LateUpdate()
    {
        if (priceText != null && Camera.main != null)
        {
            priceText.transform.LookAt(Camera.main.transform);
            priceText.transform.Rotate(0, 180, 0);
        }
    }
}