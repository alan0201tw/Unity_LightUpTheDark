using FatshihLib.Event;
using GameServices.Interface;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameServices.Interface
{
    public interface IMobileInputServiceProvider
    {
        /// <summary>
        /// This event is triggered whenever a Touch is in Ended or Canceled phase.
        /// The first bool is whether the ended touch moved or not. If you want a touch "click" event,
        /// do an if statement on that.
        /// The second Vector2 is the position of the ended touch.
        /// </summary>
        event UnityEventHandler<EndedTouchEventArgs> OnTouchEnded;

        /// <summary>
        /// This event is triggered whenever a Touch is moving.
        /// Please notice that touches that starts by touching a UI element is excluded, otherwise you
        /// might receive this event when player is tuning volume.
        /// The vector2 is the motion
        /// </summary>
        event UnityEventHandler<MotionEventArgs> OnTouchMoving;
    }
}

namespace GameServices.MobileInputService
{
    public class MobileInputServiceProvider : MonoBehaviour, IMobileInputServiceProvider
    {
        public event UnityEventHandler<EndedTouchEventArgs> OnTouchEnded;
        public event UnityEventHandler<MotionEventArgs> OnTouchMoving;

        // use this to determine if the touch start on an UI element
        private Dictionary<int, TouchState> touchStateDictionary = new Dictionary<int, TouchState>();

        private void Awake()
        {
            ProvideService();
        }

        private void ProvideService()
        {
            GameServicesLocator.Instance.MobileInputServiceProvider = this;
        }

        private void SetTouchState(int touchID, TouchState touchState)
        {
            if (!touchStateDictionary.ContainsKey(touchID))
                touchStateDictionary.Add(touchID, touchState);
            else
                touchStateDictionary[touchID] = touchState;
        }

        private void Update()
        {
            // if there are no touch to be processed, just ignore the following instructions
            if (Input.touchCount <= 0)
                return;

            // loop through each touch and process it according to its phase
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    // If this touch is on UI elements or PhysicsRaycaster targets, mark as invalid
                    if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    {
                        SetTouchState(touch.fingerId, TouchState.Invalid);
                    }
                    else // if not, we can mark it as a valid touch
                    {
                        SetTouchState(touch.fingerId, TouchState.Valid);
                    }
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    if (IsTouchInDictionaryInvalid(touch.fingerId))
                        continue;

                    bool isTouchMoved = (touchStateDictionary[touch.fingerId] == TouchState.Moved);

                    // this touch leaves screen, so set it to invalid
                    // need to do this after assigning isTouchMoved, otherwise it'll be corrupted
                    touchStateDictionary[touch.fingerId] = TouchState.Invalid;

                    if (OnTouchEnded != null)
                    {
                        EndedTouchEventArgs eventArgs = new EndedTouchEventArgs(isTouchMoved, touch.position);
                        OnTouchEnded.Invoke(this, eventArgs);
                    }
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    // if this touch is invalid, don't reset it to moved
                    if (IsTouchInDictionaryInvalid(touch.fingerId))
                        continue;

                    touchStateDictionary[touch.fingerId] = TouchState.Moved;

                    if (OnTouchMoving != null)
                    {
                        MotionEventArgs eventArgs = new MotionEventArgs(touch.deltaPosition);
                        OnTouchMoving.Invoke(this, eventArgs);
                    }
                }
            }
        }

        private bool IsTouchInDictionaryInvalid(int fingerID)
        {
            // if the touch didn't exist in the dictionary
            if (!touchStateDictionary.ContainsKey(fingerID)) return true;
            // or if the touch exist, but is invalid
            if (touchStateDictionary[fingerID] == TouchState.Invalid) return true;

            // if none above, it's valid or moved
            return false;
        }

    }

    public enum TouchState : byte
    {
        Valid,
        Moved,
        Invalid
    }
}