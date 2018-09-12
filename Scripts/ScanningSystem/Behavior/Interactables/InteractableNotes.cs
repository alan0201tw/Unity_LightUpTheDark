using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameServices.ScanningService.Interactables
{
    [RequireComponent(typeof(Collider))]
    public class InteractableNotes : Scannable, IInteractable
    {
        [SerializeField]
        private Color targetColor;
        [SerializeField]
        private NoteSystem.Note note;

        [SerializeField]
        private bool isOnWhenAwake;

        [Header("Animation Parameter")]
        [SerializeField]
        private GameObject noteGameObject;
        [SerializeField]
        private float rotationSpeed = 10f;

        private bool m_isInteractable;

        private readonly float fadeInTime = 3f;

        private bool m_isScanned;
        public override bool IsScanned
        {
            get { return m_isScanned; }
        }

        protected override void Start()
        {
            base.Start();

            UpdateState(isOnWhenAwake);
        }

        private void UpdateState(bool isScanned)
        {
            // set up behavior state base on isOnWhenAwake
            gameObject.SetActive(isScanned);
            m_isInteractable = isScanned;
            m_isScanned = isScanned;

            if (isScanned)
            {
                StartCoroutine(Maintainence());
                StartCoroutine(FadeIn());
            }
        }

        public override void OnBeingScanned(ScannerData scannerData)
        {
            if (m_isScanned)
                return;

            if (ColorMathUtility.ColorMath.ColorDistance01(scannerData.color, targetColor) < 0.2f)
            {
                UpdateState(true);
            }
        }

        private IEnumerator FadeIn()
        {
            float timeCounter = 0f;

            while (timeCounter < fadeInTime)
            {
                foreach (Material mat in noteGameObject.GetComponent<Renderer>().materials)
                {
                    mat.SetFloat("_Threshold", 1 - (timeCounter / fadeInTime));
                }

                timeCounter += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            foreach (Material mat in noteGameObject.GetComponent<Renderer>().materials)
            {
                mat.SetFloat("_Threshold", 0);
            }
        }

        private IEnumerator Maintainence()
        {
            while (true)
            {
                noteGameObject.transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed, Space.World);
                yield return new WaitForEndOfFrame();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!m_isInteractable) return;

            OnCollect();
        }

        private void OnCollect()
        {
            string text = string.Empty;
            LanguageType languageType = LanguageType.Chinese;
            languageType = GameServicesLocator.Instance.LanguageServiceProvider.GetCurrentLanguageType();

            if (languageType == LanguageType.English)
            {
                text = @"You collected note : " + note.GetNoteNameByLanguageType(languageType);
            }
            else if (languageType == LanguageType.Chinese)
            {
                text = @"你撿起了筆記 : " + note.GetNoteNameByLanguageType(languageType);
            }

            GameServicesLocator.Instance.TextDisplayServiceProvider.DisplayText(text, 2f);

            NoteSystem.NoteManager.Instance.AddNoteToList(note);
            Destroy(gameObject);
        }
    }
}