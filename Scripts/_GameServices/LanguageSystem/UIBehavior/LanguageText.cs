using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameServices.LanguageService
{
    [RequireComponent(typeof(Text))]
    public class LanguageText : MonoBehaviour
    {
        [SerializeField]
        private Text m_text;
        [SerializeField]
        private LanguageTextSingle content;

        private void Reset()
        {
            m_text = GetComponent<Text>();
        }

        private void Start()
        {
            if (m_text == null)
                Reset();

            OnLanguageTypeChange(GameServicesLocator.Instance.LanguageServiceProvider.GetCurrentLanguageType());

            GameServicesLocator.Instance.LanguageServiceProvider.OnLanguageTypeChange += OnLanguageTypeChange;
        }

        private void OnDestroy()
        {
            GameServicesLocator.Instance.LanguageServiceProvider.OnLanguageTypeChange -= OnLanguageTypeChange;
        }

        private void OnLanguageTypeChange(LanguageType languageType)
        {
            m_text.text = content.GetContentByLanguageType(languageType);
        }
    }
}