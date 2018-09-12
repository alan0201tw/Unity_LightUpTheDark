using GameServices;
using GameServices.ScanningService.Interactables;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameServices.ScanningService
{
    public class InteractableMemoryShatter : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private string nextLevelName;

        public void OnPointerClick(PointerEventData eventData)
        {
            GameServicesLocator.Instance.SceneServiceProvider.LoadSceneAsync(nextLevelName);
        }

        void Update()
        {
            // Animation
            transform.Rotate(Time.deltaTime * Vector3.up * 5f);
        }
    }
}