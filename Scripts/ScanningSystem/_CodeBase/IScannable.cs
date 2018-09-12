using UnityEngine;

namespace GameServices.ScanningService
{
    public struct ScannerData
    {
        public Color color;
        public float absoluteIntensity;
        public float normalizedIntensity;

        public ScannerData(Color _color, float _absoluteIntensity, float _normalizedIntensity)
        {
            color = _color;
            absoluteIntensity = _absoluteIntensity;
            normalizedIntensity = _normalizedIntensity;
        }
    }

    public abstract class Scannable : MonoBehaviour
    {
        protected virtual void Start()
        {
            GameServicesLocator.Instance.ScanningServiceProvider.RegisterScannable(this);
        }

        public abstract bool IsScanned { get; }

        public abstract void OnBeingScanned(ScannerData scannerData);
    }
}