using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameServices.ScanningService
{
    public class ScanningEffect : MonoBehaviour
    {
        #region Visual Effect Parameters

        [SerializeField]
        private Material effectMaterial;

        private Color materialColor = Color.white;
        public Color MaterialColor
        {
            get
            {
                return materialColor;
            }
            set
            {
                materialColor = value;
                effectMaterial.SetColor("_Color", materialColor);
            }
        }

        [SerializeField]
        private float scanningSpeed = 10f;
        [SerializeField]
        [Tooltip("The intensity for scanner when initialized, will decrease as time pass")]
        private float maxScannerIntensity = 10f;

        [SerializeField]
        private LayerMask hitLayer;

        private Vector3 m_scannerOriginPosition;
        public Vector3 ScannerOriginPosition { get { return m_scannerOriginPosition; } }

        private float m_currentScanningDistance;
        public float CurrentScanningDistance { get { return m_currentScanningDistance; } }

        private Camera m_camera;

        private bool m_isScanning;
        public bool IsScanning { get { return m_isScanning; } }

        private float m_currentScannerIntensity;
        public float CurrentIntensity { get { return m_currentScannerIntensity; } }
        public float CurrentIntensityRatio { get { return m_currentScannerIntensity / maxScannerIntensity; } }

        #endregion

        private void OnEnable()
        {
            m_camera = GetComponent<Camera>();
            m_camera.depthTextureMode = DepthTextureMode.Depth;
        }

        public void ResetOriginByCameraRay(Vector2 touchPosition)
        {
            Ray ray = m_camera.ScreenPointToRay(touchPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, hitLayer))
            {
                m_isScanning = true;
                m_currentScannerIntensity = maxScannerIntensity;

                m_currentScanningDistance = 0;
                m_scannerOriginPosition = hit.point;
            }
        }

        private void Update()
        {
            if (m_isScanning)
            {
                m_currentScanningDistance += Time.deltaTime * scanningSpeed;
                m_currentScannerIntensity -= Time.deltaTime * scanningSpeed;

                effectMaterial.SetFloat("_ScanIntensityRatio", CurrentIntensityRatio);

                // stop the scanner when intensity is not enough
                if (m_currentScannerIntensity < 0)
                {
                    m_isScanning = false;
                }
            }
            else if (!m_isScanning)
            {
                // if isn't scanning, just pass 0 as intensity ratio
                effectMaterial.SetFloat("_ScanIntensityRatio", 0);
            }
        }

        [ImageEffectOpaque]
        void OnRenderImage(RenderTexture src, RenderTexture dst)
        {
            effectMaterial.SetVector("_WorldSpaceScannerPos", m_scannerOriginPosition);
            effectMaterial.SetFloat("_ScanDistance", m_currentScanningDistance);
            RaycastCornerBlit(src, dst, effectMaterial);
        }

        void RaycastCornerBlit(RenderTexture source, RenderTexture dest, Material mat)
        {
            // Compute Frustum Corners
            float camFar = m_camera.farClipPlane;
            float camFov = m_camera.fieldOfView; // unity fov is vertical fov
            float camAspect = m_camera.aspect;

            float fovWHalf = camFov * 0.5f;

            Vector3 toRight = m_camera.transform.right * Mathf.Tan(fovWHalf * Mathf.Deg2Rad) * camAspect;
            Vector3 toTop = m_camera.transform.up * Mathf.Tan(fovWHalf * Mathf.Deg2Rad);

            Vector3 topLeft = (m_camera.transform.forward - toRight + toTop);
            float camScale = topLeft.magnitude * camFar;

            topLeft.Normalize();
            topLeft *= camScale;

            Vector3 topRight = (m_camera.transform.forward + toRight + toTop);
            topRight.Normalize();
            topRight *= camScale;

            Vector3 bottomRight = (m_camera.transform.forward + toRight - toTop);
            bottomRight.Normalize();
            bottomRight *= camScale;

            Vector3 bottomLeft = (m_camera.transform.forward - toRight - toTop);
            bottomLeft.Normalize();
            bottomLeft *= camScale;

            // Custom Blit, encoding Frustum Corners as additional Texture Coordinates
            RenderTexture.active = dest;

            mat.SetTexture("_MainTex", source);

            GL.PushMatrix();
            GL.LoadOrtho();

            mat.SetPass(0);

            GL.Begin(GL.QUADS);

            GL.MultiTexCoord2(0, 0.0f, 0.0f);
            GL.MultiTexCoord(1, bottomLeft);
            GL.Vertex3(0.0f, 0.0f, 0.0f);

            GL.MultiTexCoord2(0, 1.0f, 0.0f);
            GL.MultiTexCoord(1, bottomRight);
            GL.Vertex3(1.0f, 0.0f, 0.0f);

            GL.MultiTexCoord2(0, 1.0f, 1.0f);
            GL.MultiTexCoord(1, topRight);
            GL.Vertex3(1.0f, 1.0f, 0.0f);

            GL.MultiTexCoord2(0, 0.0f, 1.0f);
            GL.MultiTexCoord(1, topLeft);
            GL.Vertex3(0.0f, 1.0f, 0.0f);

            GL.End();
            GL.PopMatrix();
        }
    }
}