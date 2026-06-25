using UnityEngine;

namespace AstronautFirstPersonCamera
{
    public class AstronautFirstPersonCamera : MonoBehaviour
    {
        [Header("プレイヤー")]
        public Transform player;

        [Header("カメラ位置（頭の位置）")]
        public Transform cameraHolder;

        public float sensitivityX = 3.0f;
        public float sensitivityY = 3.0f;

        private float currentX;
        private float currentY;

        private const float Y_ANGLE_MIN = -80.0f;
        private const float Y_ANGLE_MAX = 80.0f;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            if (Time.timeScale == 0.0f)
            {
                // マウスカーソルを表示して自由に動かせるようにする
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                return;
            }

            // プレイ中はマウスカーソルを隠して固定する
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            currentX += Input.GetAxis("Mouse X") * sensitivityX;
            currentY -= Input.GetAxis("Mouse Y") * sensitivityY;

            currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);

            // プレイヤーはY軸だけ回転
            player.rotation = Quaternion.Euler(0, currentX, 0);

            // カメラは上下だけ回転
            cameraHolder.localRotation = Quaternion.Euler(currentY, 0, 0);
        }

        public Vector3 headOffset = new Vector3(0f, 1.6f, 0.2f);

        void LateUpdate()
        {
            transform.position = player.position + player.TransformDirection(headOffset);
            transform.rotation = cameraHolder.rotation;
        }
    }
}