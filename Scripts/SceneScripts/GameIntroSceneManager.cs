using GameServices;
using GameServices.LanguageService;
using System;
using System.Collections;
using UnityEngine;

public class GameIntroSceneManager : MonoBehaviour
{
    [SerializeField]
    private string firstLevelName;

    [SerializeField]
    private LanguageTextSequence sequence;

    private void Start()
    {
        StartCoroutine(IntroCoroutine(IntroCallback));
    }

    private void IntroCallback()
    {
        StartCoroutine(CallBackCoroutine());
    }

    private IEnumerator CallBackCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        GameServicesLocator.Instance.SceneServiceProvider.LoadSceneAsync(firstLevelName);
    }

    private IEnumerator IntroCoroutine(Action onEndCallback)
    {
        LanguageType languageType = GameServicesLocator.Instance.LanguageServiceProvider.GetCurrentLanguageType();

        var lines = sequence.GetContentsByLanguageType(languageType);

        for (int i = 0; i < lines.Length; i++)
        {
            if (i == lines.Length - 1)
                GameServicesLocator.Instance.TextDisplayServiceProvider.DisplayText(lines[i], 1.5f, onEndCallback);
            else
                GameServicesLocator.Instance.TextDisplayServiceProvider.DisplayText(lines[i], 1.5f);
        }

        yield return new WaitForEndOfFrame();
    }
}