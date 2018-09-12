using GameServices;
using UnityEngine;
using UnityEngine.UI;

public class IntroSceneManager : MonoBehaviour
{
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private Button settingButton;

    [SerializeField]
    private GameObject settingMenuPrefab;

    // TODO
    [SerializeField]
    private string startLevelName;

    private void Start()
    {
        RegisterButtonListener();
    }

    private void RegisterButtonListener()
    {
        startButton.onClick.AddListener(StartGame);
        settingButton.onClick.AddListener(OpenSettingMenu);
    }

    private void StartGame()
    {
        GameServicesLocator.Instance.SceneServiceProvider.LoadScene(startLevelName);
    }

    private void OpenSettingMenu()
    {
        Canvas canvas = FindObjectOfType<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("No canvas found!");
            return;
        }

        Instantiate(settingMenuPrefab, canvas.transform);
    }
}