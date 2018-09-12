using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AngleMathUtility
{
    public static class AngleMath
    {
        public static float ClampAngle(float angle, float min, float max)
        {
            angle = angle % 360;
            if ((angle >= -360f) && (angle <= 360f))
            {
                if (angle < -360f)
                {
                    angle += 360f;
                }
                if (angle > 360f)
                {
                    angle -= 360f;
                }
            }
            return Mathf.Clamp(angle, min, max);
        }
    }
}