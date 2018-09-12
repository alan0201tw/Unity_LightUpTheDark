using ColorMathUtility;
using UnityEngine;
using UnityEngine.Events;

namespace GameServices.ScanningService
{
    public class ScannableEventTrigger : Scannable
    {
        [SerializeField]
        private UnityEvent OnSuccessfulScannedEvents;

        [SerializeField]
        private Color targetColor = Color.white;

        private bool m_isScanned = false;
        public override bool IsScanned
        {
            get { return m_isScanned; }
        }

        public override void OnBeingScanned(ScannerData scannerData)
        {
            if (m_isScanned) return;

            if (ColorMath.ColorDistance01(scannerData.color, targetColor) <= 0.3f)
            {
                if (OnSuccessfulScannedEvents != null)
                {
                    OnSuccessfulScannedEvents.Invoke();
                }

                m_isScanned = true;
            }
        }
    }
}