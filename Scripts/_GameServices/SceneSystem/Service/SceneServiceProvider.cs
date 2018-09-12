using GameServices.Interface;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameServices.Interface
{
    public interface ISceneServiceProvider
    {
        string CurrentSceneName { get; }

        void LoadScene(int sceneIndex, Action onLoadComplete = null);
        void LoadScene(string sceneName, Action onLoadComplete = null);
        void LoadSceneAsync(int sceneIndex, Action onLoadComplete = null);
        void LoadSceneAsync(string sceneName, Action onLoadComplete = null);
    }
}

namespace GameServices.SceneService
{
    public class SceneServiceProvider : ISceneServiceProvider
    {
        private GameObject m_dummyGameObject;
        public GameObject DummyGameObject
        {
            get
            {
                if (m_dummyGameObject == null)
                    m_dummyGameObject = new GameObject("DummyGameObject for SceneServiceProvider");
                return m_dummyGameObject;
            }
        }
        
        public int CurrentSceneIndex
        {
            get { return SceneManager.GetActiveScene().buildIndex; }
        }

        public string CurrentSceneName
        {
            get { return SceneManager.GetActiveScene().name; }
        }

        public void LoadScene(int sceneIndex, Action onLoadComplete = null)
        {
            // since this is blocking, no need to set up the loading flag
            SceneManager.LoadScene(sceneIndex);
            if (onLoadComplete != null)
                onLoadComplete.Invoke();
        }

        public void LoadScene(string sceneName, Action onLoadComplete = null)
        {
            // since this is blocking, no need to set up the loading flag
            SceneManager.LoadScene(sceneName);
            if (onLoadComplete != null)
                onLoadComplete.Invoke();
        }

        public void LoadSceneAsync(int sceneIndex, Action onLoadComplete = null)
        {
            CoroutineRunner coroutineRunner = DummyGameObject.AddComponent<CoroutineRunner>();

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
            coroutineRunner.StartCoroutine(LoadSceneCoroutine(asyncLoad, onLoadComplete));
        }

        public void LoadSceneAsync(string sceneName, Action onLoadComplete = null)
        {
            CoroutineRunner coroutineRunner = DummyGameObject.AddComponent<CoroutineRunner>();

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            coroutineRunner.StartCoroutine(LoadSceneCoroutine(asyncLoad, onLoadComplete));
        }

        private IEnumerator LoadSceneCoroutine(AsyncOperation asyncLoad, Action onLoadComplete)
        {
            // when the scene completes loading itself to memory
            // it still needs to call Awake and Start on all objects in that scene
            // this process cannot be cut and must be done in one cycle
            // so LoadAsync still yields a stall, but shorter.
            
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            if (onLoadComplete != null)
                onLoadComplete.Invoke();
        }
    }
}