using GameServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SceneSystem
{
    public class LevelButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private string levelName;

        private void LoadScene()
        {
            GameServicesLocator.Instance.SceneServiceProvider.LoadSceneAsync(levelName);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // TODO : add some animation? 
            LoadScene();
        }
    }
}