using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameServices.MobileInputService
{
    // use this to strict applying gameObject to UI elements since Graphic class is the base class
    // for all visual UI Components.
    [RequireComponent(typeof(Graphic))]
    public sealed class VirtualJoyStick : JoyStick
    {
        [SerializeField]
        private int m_movementRadius = 50;
        public int MovementRadius { get { return m_movementRadius; } }
        
        private Vector3 m_initialPosition;
        public Vector3 InitialPosition { get { return m_initialPosition; } }

        private bool m_isUsingWorldPosition;
        private Camera m_canvasCamera;

        private void Start()
        {
            m_initialPosition = transform.position;

            var renderMode = GetComponentInParent<Canvas>().renderMode;
            if (renderMode == RenderMode.ScreenSpaceOverlay)
            {
                m_isUsingWorldPosition = false;
            }
            else
            {
                m_isUsingWorldPosition = true;
                m_canvasCamera = GetComponentInParent<Canvas>().worldCamera;
            }
        }

        public override void OnDrag(PointerEventData eventData)
        {
            Vector3 touchPosition;
            if (!m_isUsingWorldPosition)
            {
                touchPosition = eventData.position;
            }
            else
            {
                touchPosition = m_canvasCamera.ScreenToWorldPoint(eventData.position);
            }

            Vector2 delta = new Vector2(touchPosition.x - InitialPosition.x, touchPosition.y - InitialPosition.y);

            if (delta.magnitude > m_movementRadius)
            {
                delta = delta.normalized * m_movementRadius;
            }

            transform.position = new Vector3(InitialPosition.x + delta.x, InitialPosition.y + delta.y, InitialPosition.z);

            // assign motion and state variables for external usage
            // normalizing motion will be necessary
            Motion = delta / m_movementRadius;
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            transform.position = InitialPosition;
            Motion = Vector2.zero;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            // Do nothing, but without this OnPointerUp won't work!!!
        }
    }
}