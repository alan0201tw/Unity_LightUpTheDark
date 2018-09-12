using UnityEngine;
using AngleMathUtility;
using GameServices;
using GameServices.Interface;

namespace PlayerSystem.Behavior
{
    public class DragToLookAround : MonoBehaviour
    {
        [SerializeField]
        [Range(0.1f, 1.5f)]
        private float horizontalRotatingSpeed = 0.5f;

        [SerializeField]
        [Range(0.05f, 0.5f)]
        private float verticalRotatingSpeed = 0.1f;
        [SerializeField]
        [Range(-30f, 0f)]
        private float minVerticalRotation = -30f;
        [SerializeField]
        [Range(0f, 45f)]
        private float maxVerticalRotation = 45f;

        private float m_currentVerticalRotation;

        private void Start()
        {
            GameServicesLocator.Instance.MobileInputServiceProvider.OnTouchMoving += OnTouchMoving;

            m_currentVerticalRotation = transform.localEulerAngles.x;
        }

        private void OnDestroy()
        {
            GameServicesLocator.Instance.MobileInputServiceProvider.OnTouchMoving -= OnTouchMoving;
        }

        private void OnTouchMoving(Object sender, MotionEventArgs eventArgs)
        {
            float horizontalRotation = horizontalRotatingSpeed * eventArgs.motion.x;
            transform.Rotate(0, horizontalRotation, 0, Space.World);

            float verticalRotation = verticalRotatingSpeed * -eventArgs.motion.y;
            m_currentVerticalRotation += verticalRotation;

            Vector3 t_newEuler = transform.localEulerAngles;
            t_newEuler.x =
                AngleMath.ClampAngle(m_currentVerticalRotation, minVerticalRotation, maxVerticalRotation);

            transform.localEulerAngles = t_newEuler;
        }
    }
}