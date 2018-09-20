using UnityEngine;
using UnityEngine.UI;
using GameServices;

public class SettingMenu : MonoBehaviour
{
    [SerializeField]
    private Button resumeButton;
    [SerializeField]
    private Button exitButton;

    [SerializeField]
    private Toggle chineseToggle;
    [SerializeField]
    private Toggle englishToggle;

    [SerializeField]
    private Slider mainVolumeControlSlider;

    private string mainSceneName = "Main";

    private void OnEnable()
    {
        InitializeToCurrentSettings();
        RegisterButtonListener();
    }

    private void InitializeToCurrentSettings()
    {
        var AudioManager = GameServicesLocator.Instance.AudioServiceProvider;
        LanguageType languageType = GameServicesLocator.Instance.LanguageServiceProvider.GetCurrentLanguageType();

        // initialize UI to match current setting
        mainVolumeControlSlider.value = AudioManager.GetCurrentVolume();
        chineseToggle.isOn = (languageType == LanguageType.Chinese);
        englishToggle.isOn = (languageType == LanguageType.English);

        // if already in "Main" scene, disable this button
        if(GameServicesLocator.Instance.SceneServiceProvider.CurrentSceneName == mainSceneName)
        {
            exitButton.interactable = false;
        }
    }

    private void RegisterButtonListener()
    {
        resumeButton.onClick.AddListener(() =>
        {
            Destroy(gameObject);
        });

        chineseToggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                GameServicesLocator.Instance.LanguageServiceProvider.SetLanguageType(LanguageType.Chinese);
            }
        });

        englishToggle.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                GameServicesLocator.Instance.LanguageServiceProvider.SetLanguageType(LanguageType.English);
            }
        });

        exitButton.onClick.AddListener(() =>
        {
            // TODO : should pop an alert box, asking are u sure u want to exit
            GameServicesLocator.Instance.SceneServiceProvider.LoadSceneAsync(mainSceneName);
        });

        mainVolumeControlSlider.onValueChanged.AddListener((value01) =>
        {
            GameServicesLocator.Instance.AudioServiceProvider.SetVolume(value01);
        });
    }
}