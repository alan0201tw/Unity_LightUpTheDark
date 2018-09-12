using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorMathUtility
{
    public static class ColorMath
    {
        public static Vector3 ColorToVector3(Color color)
        {
            return new Vector3(color.r, color.g, color.b);
        }

        public static Vector4 ColorToVector4(Color color)
        {
            return new Vector4(color.r, color.g, color.b, color.a);
        }

        public static Color Vector3ToColorRGB(Vector3 vector, float alpha = 1)
        {
            return new Color(vector.x, vector.y, vector.z, alpha);
        }

        public static Color Vector4ToColor(Vector4 vector)
        {
            return new Color(vector.x, vector.y, vector.z, vector.w);
        }

        public static float ColorDistance01(Color colorA, Color colorB)
        {
            Vector3 aVec = new Vector3(colorA.r, colorA.g, colorA.b);
            Vector3 bVec = new Vector3(colorB.r, colorB.g, colorB.b);

            return Vector3.Distance(aVec, bVec);
        }

        public static Color ColorLerpSafe(Color start, Color end, float t)
        {
            Vector4 startVec = ColorToVector4(start);
            Vector4 endVec = ColorToVector4(end);

            Vector4 lerpVec = Vector4.Lerp(startVec, endVec, t);

            Color lerpColor = Vector4ToColor(lerpVec);

            return lerpColor;
        }
    }
}