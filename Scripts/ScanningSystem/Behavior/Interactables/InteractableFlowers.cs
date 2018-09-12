using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameServices.ScanningService.Interactables
{
    [RequireComponent(typeof(ParticleSystem))]
    [RequireComponent(typeof(Collider))]
    public class InteractableFlowers : Scannable, IInteractable
    {
        [SerializeField]
        private ColorMathUtility.StartEndColorPair colorPair;

        [SerializeField]
        private bool isOnWhenAwake;

        private ParticleSystem m_particleSystem;

        private bool m_isInteractable;

        private bool m_isScanned;
        public override bool IsScanned
        {
            get { return m_isScanned; }
        }

        protected override void Start()
        {
            base.Start();

            m_particleSystem = GetComponent<ParticleSystem>();

            // set up color over life time property based on editor values
            Gradient gradient = new Gradient();
            GradientColorKey[] gradientColorKeys = new GradientColorKey[2];
            GradientAlphaKey[] gradientAlphaKeys = new GradientAlphaKey[2];

            gradientColorKeys[0] = new GradientColorKey { color = colorPair.StartColor, time = 0 };
            gradientColorKeys[1] = new GradientColorKey { color = colorPair.EndColor, time = 1 };

            gradientAlphaKeys[0] = new GradientAlphaKey { alpha = 1, time = 0 };
            gradientAlphaKeys[1] = new GradientAlphaKey { alpha = 0, time = 1 };

            gradient.SetKeys(gradientColorKeys, gradientAlphaKeys);

            ParticleSystem.ColorOverLifetimeModule colorOverLifetimeModule = m_particleSystem.colorOverLifetime;
            colorOverLifetimeModule.color = new ParticleSystem.MinMaxGradient(gradient);
            colorOverLifetimeModule.enabled = true;

            // turn on or off by settings in unity editor
            if (isOnWhenAwake)
                m_particleSystem.Play();
            // if on when awake, become interactable.
            m_isScanned = isOnWhenAwake;
            m_isInteractable = isOnWhenAwake;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!m_isInteractable) return;
            // check if close enough
            float distance = eventData.pointerCurrentRaycast.distance;
            if (distance > 10f) return;

            StartCoroutine(BoostParticleSystem());
            // set user's scanner color base on start color
            GameServicesLocator.Instance.ScanningServiceProvider.ScannerColor = colorPair.StartColor;
        }

        private IEnumerator BoostParticleSystem()
        {
            m_isInteractable = false;

            float timeInterval = 0.2f;
            float passTime = 0;
            ParticleSystem.EmissionModule emission = m_particleSystem.emission;
            float initial = emission.rateOverTime.Evaluate(m_particleSystem.time);

            while (passTime < timeInterval)
            {
                passTime += Time.deltaTime;

                emission.rateOverTime = initial * Mathf.Lerp(15, 1, passTime / timeInterval);

                //emissionModule.rateOverTimeMultiplier = Mathf.Lerp(1, 5, passTime / timeInterval);

                yield return new WaitForEndOfFrame();
            }

            m_isInteractable = true;
            //emissionModule.rateOverTimeMultiplier = 1;
        }

        public override void OnBeingScanned(ScannerData scannerData)
        {
            if (m_isScanned) return;

            // being scanned -> become interactable for player to color their scanner
            m_isInteractable = true;
            m_particleSystem.Play();
        }
    }
}