using GameServices;
using UnityEngine;
using GameServices.LanguageService;

[RequireComponent(typeof(Collider))]
public class LightOffZone : MonoBehaviour
{
    [SerializeField]
    private GameObject lightObject;
    [SerializeField]
    private GameObject enableObject;

    [SerializeField]
    private LanguageTextSequence content;

    private void Start()
    {
        enableObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // turn off the lights with player walks in
            lightObject.SetActive(false);
            // turn on the firefly
            enableObject.SetActive(true);

            LanguageType languageType = GameServicesLocator.Instance.LanguageServiceProvider.GetCurrentLanguageType();
            string[] texts = content.GetContentsByLanguageType(languageType);

            foreach (string text in texts)
            {
                GameServicesLocator.Instance.TextDisplayServiceProvider.DisplayTextImmediate(text, 1.5f);
            }

            // destroy itself to avoid duplicated trigger and improve performance
            Destroy(gameObject);
        }
    }
}