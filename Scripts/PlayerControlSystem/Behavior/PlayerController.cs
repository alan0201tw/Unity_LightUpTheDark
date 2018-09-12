using UnityEngine;
using PlayerSystem.Behavior;
using GameServices;
using GameServices.MobileInputService;

namespace PlayerSystem
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        private CharacterController m_characterController;

        [SerializeField]
        private HeadBobbing m_headBobbing;

        [SerializeField]
        private float speed = 6.0F;
        [SerializeField]
        private float jumpSpeed = 8.0F;
        [SerializeField]
        private float gravity = 20.0F;

        private Vector3 movementVector = Vector3.zero;

        private void Start()
        {
            m_characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if (JoyStick.Motion == Vector2.zero)
            {
                movementVector = new Vector3(0, movementVector.y, 0);
            }
            else if (m_characterController.isGrounded)
            {
                movementVector = new Vector3(JoyStick.Motion.x, 0, JoyStick.Motion.y);
                movementVector = transform.TransformDirection(movementVector);
                movementVector *= speed;
                if (Input.GetButton("Jump"))
                    movementVector.y = jumpSpeed;
            }

            if (!m_characterController.isGrounded)
                movementVector.y -= gravity * Time.deltaTime;

            Vector3 characterMotion = movementVector * Time.deltaTime;

            m_characterController.Move(characterMotion);
        }

        private void LateUpdate()
        {
            Vector3 characterMotion = movementVector * Time.deltaTime;

            m_headBobbing.UpdateBobbing(characterMotion);
        }
    }
}