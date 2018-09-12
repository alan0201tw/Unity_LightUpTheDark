using GameServices.Interface;
using UnityEngine;

namespace GameServices
{
    public class GameServicesLocator
    {
        private static readonly GameServicesLocator s_instance = new GameServicesLocator();
        public static GameServicesLocator Instance
        {
            get { return s_instance; }
        }
        // set constructor to private to avoid external instancing
        private GameServicesLocator()
        {
            Initialize();
        }

        private void Initialize()
        {
            // Do something here.
        }

        #region Constant Existed Service, instantiate right here

        public readonly IAudioServiceProvider AudioServiceProvider = new AudioService.AudioServiceProvider();

        public readonly ISceneServiceProvider SceneServiceProvider = new SceneService.SceneServiceProvider();

        public readonly ILanguageServiceProvider LanguageServiceProvider = new LanguageService.LanguageServiceProvider();

        #endregion

        #region Services that should be provided by each scene
        // It this service need to vary from scene to scene, put it here and provide it externally
        // so changing service provider can be easy

        // You can optionally have multiple service provider with same interface
        // For example, one ITextDisplayServiceProvider for gaming hints, and another for story telling

        private ITextDisplayServiceProvider m_textDisplayServiceProvider;
        public ITextDisplayServiceProvider TextDisplayServiceProvider
        {
            get { return m_textDisplayServiceProvider; }
            set
            {
                if (value == null)
                    Debug.LogError("GameServices : TextDisplayServiceProvider : " +
                        "Error trying to provide null service.");
                else
                    m_textDisplayServiceProvider = value;
            }
        }

        private IMobileInputServiceProvider m_mobileInputServiceProvider;
        public IMobileInputServiceProvider MobileInputServiceProvider
        {
            get { return m_mobileInputServiceProvider; }
            set
            {
                if (value == null)
                    Debug.LogError("GameServices : MobileInputServiceProvider : " +
                        "Error trying to provide null service.");
                else
                    m_mobileInputServiceProvider = value;
            }
        }

        private IScanningServiceProvider m_scanningServiceProvider;
        public IScanningServiceProvider ScanningServiceProvider
        {
            get { return m_scanningServiceProvider; }
            set
            {
                if (value == null)
                    Debug.LogError("GameServices : ScanningServiceProvider : " +
                        "Error trying to provide null service.");
                else
                    m_scanningServiceProvider = value;
            }
        }

        #endregion
    }
}