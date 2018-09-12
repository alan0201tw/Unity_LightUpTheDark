using GameServices.Interface;
using System.Collections.Generic;
using UnityEngine;

namespace GameServices.ScanningService
{
    public class ScanningServiceProvider : MonoBehaviour, IScanningServiceProvider
    {
        [SerializeField]
        [Tooltip("Assign an ScanningEffect component on the camera and assign it to here")]
        private ScanningEffect m_scanningEffect;

        private List<Scannable> m_registeredScannables = new List<Scannable>();

        private Color scannerColor;
        public Color ScannerColor
        {
            get
            {
                return scannerColor;
            }
            set
            {
                scannerColor = value;
                // apply change to scanning effect
                UpdateEffectMaterialColor();
            }
        }

        private void Awake()
        {
            ProvideService();
        }

        private void ProvideService()
        {
            GameServicesLocator.Instance.ScanningServiceProvider = this;
        }

        public void RegisterScannable(Scannable scannable)
        {
            if (!m_registeredScannables.Contains(scannable))
            {
                m_registeredScannables.Add(scannable);
            }
            else
            {
                Debug.LogWarning("Scannble : " + scannable.name + " is already registered, but you're trying to register again");
            }
        }

        public void CancelRegisterScannble(Scannable scannable)
        {
            if (m_registeredScannables.Contains(scannable))
            {
                m_registeredScannables.Remove(scannable);
            }
            else
            {
                Debug.LogWarning("Scannble : " + scannable.name + " is not registered, but you're trying to cancel its register");
            }
        }

        private void Start()
        {
            // TODO : Find another way to set initial color
            ScannerColor = Color.white;

            GameServicesLocator.Instance.MobileInputServiceProvider.OnTouchEnded += OnTouchEnded;

            //PlayerInputHandler.Instance.OnTouchEnded += OnTouchEnded;
        }

        private void OnDestroy()
        {
            GameServicesLocator.Instance.MobileInputServiceProvider.OnTouchEnded -= OnTouchEnded;
        }

        public void UpdateEffectMaterialColor()
        {
            // if it's scanning , don't change its color
            if (m_scanningEffect.IsScanning) return;

            m_scanningEffect.MaterialColor = ScannerColor;
        }

        private void OnTouchEnded(Object sender, EndedTouchEventArgs eventArgs)
        {
            if (eventArgs.isTouchMoved)
                return;

            // fire a ray and restart scanner
            m_scanningEffect.ResetOriginByCameraRay(eventArgs.position);
        }

        private void Update()
        {
            if (m_scanningEffect.IsScanning)
            {
                // pass the color and intensity to let object determine interaction
                ScannerData scannerData = new ScannerData(m_scanningEffect.MaterialColor, m_scanningEffect.CurrentIntensity, m_scanningEffect.CurrentIntensityRatio);

                // scan all scannable every frame
                foreach (Scannable scannable in m_registeredScannables)
                {
                    if (Vector3.Distance(m_scanningEffect.ScannerOriginPosition, scannable.transform.position) <= m_scanningEffect.CurrentScanningDistance)
                    {
                        scannable.OnBeingScanned(scannerData);
                    }
                }
            }
        }
    }
}