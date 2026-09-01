using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimplePickup : MonoBehaviour
{

    public Image crosshair;

    [Header("掴める距離")]
    public float range = 10f;

    [Header("持つ位置")]
    public Vector3 holdOffset = new Vector3(0f, -0.7f, 4f);

    [Header("投げる力")]
    public float throwForce = 15f;

    [Header("ドアを回す強さ")]
    public float doorScrollSpeed = 500f;

    private Rigidbody heldRb;
    private Transform holdPoint;
    private Vector3 grabOffset;
    private GameObject aimMarker;
    public Camera playerCamera;

    private float objectDistance;
    private bool isHoldingDoor = false;

    void Start()
    {
        // =====================
        // HoldPoint
        // =====================

        GameObject point = new GameObject("HoldPoint");
        holdPoint = point.transform;
        holdPoint.SetParent(playerCamera.transform);
        holdPoint.localPosition = holdOffset;

        // =====================
        // 照準マーカー
        // =====================

        aimMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        aimMarker.transform.localScale = Vector3.one * 0.15f;
        Destroy(aimMarker.GetComponent<Collider>());
        Renderer markerRenderer = aimMarker.GetComponent<Renderer>();
        markerRenderer.material.color = Color.red;

        aimMarker.SetActive(false);

        ////マウス表示
        //Cursor.lockState =
        //    CursorLockMode.Locked;

        //Cursor.visible = false;
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("クリック");
        }
        // ==================================
        // 画面中央からRay
        // ==================================

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Debug.DrawRay(ray.origin, ray.direction * range, Color.red);

        // ==================================
        // ドアを掴み中のホイール処理
        // ================================== 
        if (isHoldingDoor && heldRb != null)
        {
            float wheelInput = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(wheelInput) > 0.01f)
            {
                // ドアのRigidbodyに対して、ホイール入力に応じた回転の力を加える
                Vector3 torque = new Vector3(0, wheelInput * doorScrollSpeed, 0);
                heldRb.AddTorque(torque, ForceMode.Acceleration);
            }
        }

        // ==================================
        // 掴む
        // ==================================

        if (Input.GetMouseButtonDown(0))
        {
            if (heldRb == null)
            {
                if (Physics.Raycast(ray, out RaycastHit hit, range))
                {
                    Rigidbody rb = hit.rigidbody;

                    if (rb != null)
                    {
                        // 当たったオブジェクトに「Hinge Joint」が付いているかチェック
                        HingeJoint doorJoint = rb.GetComponent<HingeJoint>();

                        if (doorJoint != null)
                        {
                            // ドアだった場合の処理
                            Debug.Log("ドアを掴んだ！");
                            heldRb = rb;
                            isHoldingDoor = true;
                            heldRb.angularVelocity = Vector3.zero; // ブレ防止
                        }
                        else
                        {
                            // 通常のアイテムを掴む処理
                            Debug.Log("通常のアイテムを掴んだ！");
                            heldRb = rb;
                            isHoldingDoor = false;
                            heldRb.useGravity = false;
                            heldRb.freezeRotation = true;
                            heldRb.velocity = Vector3.zero;
                            heldRb.angularVelocity = Vector3.zero;
                            grabOffset = heldRb.position - hit.point;

                            Renderer renderer = heldRb.GetComponentInChildren<Renderer>();
                            if (renderer != null)
                            {
                                objectDistance = renderer.bounds.extents.magnitude;
                            }
                        }
                    }
                }
            }
        }

        // ==================================
        // 離す
        // ==================================

        if (Input.GetMouseButtonUp(0))
        {
            if (heldRb != null)
            {
                if (isHoldingDoor)
                {
                    // ドアを離すときの処理
                    Debug.Log("ドアを離した！");
                }
                else
                {
                    // 通常のアイテムを離す処理
                    heldRb.useGravity = true;
                    heldRb.freezeRotation = false;
                    heldRb.velocity = Vector3.zero;
                    heldRb.angularVelocity = Vector3.zero;
                }

                heldRb = null;
                isHoldingDoor = false;
            }
        }

        // ==================================
        // 投げる
        // ==================================

        if (Input.GetMouseButtonDown(1))
        {
            if (heldRb != null)
            {
                Rigidbody rb = heldRb;

                rb.useGravity = true;
                rb.freezeRotation = false;

                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                // 前方向へ投げる
                rb.AddForce(playerCamera.transform.forward * throwForce, ForceMode.Impulse);

                Debug.Log("投げた！");

                heldRb = null;
            }
        }

        // ==================================
        // 照準マーカー
        // ==================================

        if (heldRb == null)
        {
            if (Physics.Raycast(ray, out RaycastHit aimHit, range))
            {
                Rigidbody aimRb = aimHit.collider.GetComponentInParent<Rigidbody>();

                if (aimRb != null)
                {
                    aimMarker.SetActive(true);

                    aimMarker.transform.position = aimHit.point;
                }
                else
                {
                    aimMarker.SetActive(false);
                }
            }
            else
            {
                aimMarker.SetActive(false);
            }
        }
        else
        {
            aimMarker.SetActive(false);
        }

        // =====================
        // Crosshair色変更
        // =====================

        Ray crosshairRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        bool canGrab = false;

        if (Physics.Raycast(crosshairRay, out RaycastHit crosshairHit, range))
        {
            Rigidbody rb = crosshairHit.collider.GetComponentInParent<Rigidbody>();
            if (rb != null && heldRb == null)
            {
                canGrab = true;
            }
        }

        if (crosshair != null)
        {
            if (canGrab)
            {
                crosshair.color = Color.green;
            }
            else
            {
                crosshair.color = Color.white;
            }
        }
    }

    void FixedUpdate()
    {
        if (heldRb != null && !isHoldingDoor)
        {
            Vector3 targetPos = holdPoint.position;
            Vector3 direction = targetPos - heldRb.position;
            heldRb.velocity = direction * 15f;
            heldRb.angularVelocity = Vector3.zero;
        }
    }
}