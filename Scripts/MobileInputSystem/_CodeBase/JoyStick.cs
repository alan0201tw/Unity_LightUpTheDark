using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameServices.MobileInputService
{
    /// <summary>
    /// This joystick class is a base class for any JoyStick class, both virtual and real.
    /// For mobile, you can inherit from this class to create a virtual JoyStick.
    /// 
    /// Also, please notice that OnPointerDown method is required for OnPointerUp to work, implementing
    /// only IPointerUpHandler won't receive MonoBehavior message from Unity Engine.
    /// </summary>
    public abstract class JoyStick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        public static Vector2 Motion { get; protected set; }

        // Unity EventSystems API
        public abstract void OnDrag(PointerEventData eventData);
        public abstract void OnPointerDown(PointerEventData eventData);
        public abstract void OnPointerUp(PointerEventData eventData);
    }
}