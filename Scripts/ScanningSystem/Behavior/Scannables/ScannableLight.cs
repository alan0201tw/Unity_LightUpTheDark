using ColorMathUtility;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace GameServices.ScanningService
{
    [RequireComponent(typeof(Light))]
    public class ScannableLight : Scannable
    {
        [Range(0f, 1f)]
        [SerializeField]
        private float interactThreshold = 0.3f;

        [SerializeField]
        private float timeToFullyLightUp = 3f;

        private Light m_light;

        // for coroutine usage and/or interaction usage
        private Color m_targetColor;
        private float m_targetRange;
        private float m_targetIntensity;

        // for rendering sphere
        private CommandBuffer m_commandBuffer;

        private static Mesh s_sphereMesh;

        private bool m_isScanned = false;
        public override bool IsScanned
        {
            get
            {
                return m_isScanned;
            }
        }

        private void Awake()
        {
            // initialize only once
            if(s_sphereMesh == null)
            {
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                s_sphereMesh = sphere.GetComponent<MeshFilter>().mesh;
                Destroy(sphere);
            }
        }

        protected override void Start()
        {
            // call start from base class to register itself
            base.Start();

            // set up light in awake so it won't have strange light flash in first frame
            m_light = GetComponent<Light>();
            m_light.enabled = false;
            m_targetColor = m_light.color;
            m_targetRange = m_light.range;
            m_targetIntensity = m_light.intensity;

            m_light.range = 0f;

            // set up command buffer
            m_commandBuffer = new CommandBuffer
            {
                name = string.Format("Sphere Rendering Command Buffer for {0}", name)
            };
        }

        public override void OnBeingScanned(ScannerData scannerData)
        {
            // already interacted , don't trigger again
            if (m_isScanned) return;

            if (ColorMath.ColorDistance01(scannerData.color, m_targetColor) <= interactThreshold)
            {
                m_light.enabled = true;
                m_isScanned = true;

                StartCoroutine(LightUpCoroutine());
                StartCoroutine(Maintainence());
            }

            // set up command buffer
            Camera.main.AddCommandBuffer(CameraEvent.AfterForwardAlpha, m_commandBuffer);
        }

        private IEnumerator Maintainence()
        {
            Mesh mesh = s_sphereMesh;
            Material material = Resources.Load<Material>("Materials/LightSphereMaterial");

            float timeCounter = 0;
            float lerpTime = 1.5f;

            Vector3 originSpherePosition = transform.position;

            Vector3 currentSpherePosition = originSpherePosition;
            Vector3 targetSpherePosition = originSpherePosition;

            while (true)
            {
                timeCounter += Time.deltaTime;
                // this is needed, or else the previous drawn sphere won't be cleared
                m_commandBuffer.Clear();

                if (timeCounter >= lerpTime)
                {
                    targetSpherePosition = originSpherePosition + UnityEngine.Random.onUnitSphere * 0.5f;
                    timeCounter = 0f;
                }

                currentSpherePosition = 
                    Vector3.Slerp(currentSpherePosition, targetSpherePosition, timeCounter / lerpTime);

                Matrix4x4 mvpMatrix = Matrix4x4.TRS(currentSpherePosition, transform.rotation, new Vector3(0.2f, 0.2f, 0.2f));

                material.SetColor("_MainColor", m_light.color);

                m_commandBuffer.DrawMesh(mesh, mvpMatrix, material);

                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator LightUpCoroutine()
        {
            float passTime = 0;

            while (passTime < timeToFullyLightUp)
            {
                passTime += Time.deltaTime;

                m_light.range = Mathf.Lerp(0, m_targetRange, passTime / timeToFullyLightUp);
                m_light.intensity = Mathf.Lerp(0, m_targetIntensity, passTime / timeToFullyLightUp);

                yield return new WaitForEndOfFrame();
            }

            m_light.range = m_targetRange;
            m_light.intensity = m_targetIntensity;
        }
    }
}