using GameServices.Interface;
using System;
using UnityEngine;

namespace GameServices
{
    public enum LanguageType : byte
    {
        Chinese,
        English
    }
}

namespace GameServices.Interface
{
    public interface ILanguageServiceProvider
    {
        event Action<LanguageType> OnLanguageTypeChange;

        LanguageType GetCurrentLanguageType();
        void SetLanguageType(LanguageType languageType);
    }
}

namespace GameServices.LanguageService
{
    public class LanguageServiceProvider : ILanguageServiceProvider
    {
        private static readonly string PlayerPrefsLanguageKey = "PlayerPrefsLanguageKey";

        private LanguageType m_currentLanguageType;

        public event Action<LanguageType> OnLanguageTypeChange;

        public LanguageServiceProvider()
        {
            // Initialize Language setting with PlayerPrefs, set default to Chinese
            LanguageType defaultType = 
                (LanguageType)PlayerPrefs.GetInt(PlayerPrefsLanguageKey, (int)LanguageType.Chinese);

            SetLanguageType(defaultType);
        }

        public LanguageType GetCurrentLanguageType()
        {
            return m_currentLanguageType;
        }

        public void SetLanguageType(LanguageType languageType)
        {
            if (m_currentLanguageType == languageType)
                return;

            m_currentLanguageType = languageType;
            // save change to PlayerPrefs
            PlayerPrefs.SetInt(PlayerPrefsLanguageKey, (int)m_currentLanguageType);

            if (OnLanguageTypeChange != null)
            {
                OnLanguageTypeChange.Invoke(m_currentLanguageType);
            }
        }
    }
}