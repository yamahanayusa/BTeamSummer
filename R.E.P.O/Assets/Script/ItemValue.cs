using UnityEngine;

public class ItemValue : MonoBehaviour
{
    [HideInInspector]
    public int value;

    void Start()
    {
        value = Random.Range(300, 601);

        Debug.Log(gameObject.name + " ‚Ì‰¿’l‚Í " + value + "‰~");
    }
}
