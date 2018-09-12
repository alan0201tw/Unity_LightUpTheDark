using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorMathUtility
{
    [CreateAssetMenu(menuName = "ColorUtility/StartEndColorPair", fileName = "NewColorPair")]
    public class StartEndColorPair : ScriptableObject
    {
        [SerializeField]
        private Color startColor;
        public Color StartColor { get { return startColor; } }

        [SerializeField]
        private Color endColor;
        public Color EndColor { get { return endColor; } }
    }
}