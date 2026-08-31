using UnityEngine;
using System.Collections;

namespace AstronautPlayer
{
    public class AstronautPlayer : MonoBehaviour
    {
        private Animator anim;
        private CharacterController controller;

        public float speed = 6.0f;
        private Vector3 moveDirection = Vector3.zero;
        public float gravity = 20.0f;

        void Start()
        {
            controller = GetComponent<CharacterController>();
            anim = gameObject.GetComponentInChildren<Animator>();
        }

        void Update()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // WASD移動
            Vector3 move = transform.forward * v +
                           transform.right * h;

            move.Normalize();

            if (controller.isGrounded)
            {
                moveDirection = move * speed;
            }

            // 重力
            moveDirection.y -= gravity * Time.deltaTime;

            // 移動
            controller.Move(moveDirection * Time.deltaTime);

            // アニメーション
            if (move.magnitude > 0.1f)
            {
                anim.SetInteger("AnimationPar", 1);
            }
            else
            {
                anim.SetInteger("AnimationPar", 0);
            }
        }
    }
}