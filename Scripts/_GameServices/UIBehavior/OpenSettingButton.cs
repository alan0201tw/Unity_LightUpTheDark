using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class OpenSettingButton : MonoBehaviour
{
    [SerializeField]
    private GameObject settingMenuPrefab;

    // Use this for initialization
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OpenSettingMenu);
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