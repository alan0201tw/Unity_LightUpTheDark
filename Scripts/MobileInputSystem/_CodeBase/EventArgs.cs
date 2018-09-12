using System;
using UnityEngine;

namespace GameServices.Interface
{
    public class EndedTouchEventArgs : EventArgs
    {
        public bool isTouchMoved;
        public Vector2 position;

        public EndedTouchEventArgs(bool isTouchMoved, Vector2 position)
        {
            this.isTouchMoved = isTouchMoved;
            this.position = position;
        }
    }

    public class MotionEventArgs : EventArgs
    {
        public Vector2 motion;

        public MotionEventArgs(Vector2 motion)
        {
            this.motion = motion;
        }
    }
}