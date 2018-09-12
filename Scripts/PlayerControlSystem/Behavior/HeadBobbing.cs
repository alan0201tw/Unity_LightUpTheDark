using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSystem.Behavior
{
    public class HeadBobbing : MonoBehaviour
    {
        [SerializeField]
        private float bobbingSpeed = 0.18f;
        [SerializeField]
        private float bobbingAmount = 0.2f;

        private float midPoint = 0.8f;
        private float m_timer = 0.0f;

        [SerializeField]
        [Range(0.01f, 0.2f)]
        private float dampingTime = 0.05f;

        private Vector3 m_dampingVelocity = Vector3.zero;

        private void Start()
        {
            midPoint = transform.localPosition.y;
        }

        public void UpdateBobbing(Vector3 motion)
        {
            float waveSlice = 0f;
            float horizontal = motion.x;
            float vertical = motion.z;

            Vector3 targetPosition = transform.localPosition;

            // if both is zero
            if (horizontal * vertical == 0)
            {
                m_timer = 0.0f;
            }
            else
            {
                waveSlice = Mathf.Sin(m_timer);
                m_timer = m_timer + bobbingSpeed;
                if (m_timer > Mathf.PI * 2)
                {
                    m_timer = m_timer - (Mathf.PI * 2);
                }
            }

            if (waveSlice != 0)
            {
                float translateChange = waveSlice * bobbingAmount;
                float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
                totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
                translateChange = totalAxes * translateChange;
                targetPosition.y = midPoint + translateChange;
            }
            else
            {
                targetPosition.y = midPoint;
            }

            //transform.localPosition = originalPosition;
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetPosition, ref m_dampingVelocity, dampingTime);
        }
    }
}