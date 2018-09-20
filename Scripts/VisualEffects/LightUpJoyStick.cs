using ColorMathUtility;
using GameServices.MobileInputService;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LUTD.VisualEffects
{
    [RequireComponent(typeof(Image))]
    public class LightUpJoyStick : MonoBehaviour
    {
        private Image m_backgroundImage;
        
        [SerializeField]
        private Color idleColor = Color.clear;
        [SerializeField]
        private Color highlightColor = Color.white;

        private float ratio = 0f;

        private void Start()
        {
            m_backgroundImage = GetComponent<Image>();
            // initialization
            m_backgroundImage.color = idleColor;
        }

        private void Update()
        {
            if (JoyStick.Motion.magnitude > 0)
                ratio += Time.deltaTime;
            else
                ratio -= Time.deltaTime;

            ratio = Mathf.Clamp01(ratio);
            m_backgroundImage.color = ColorMath.ColorLerpSafe(idleColor, highlightColor, ratio);
        }
    }
}