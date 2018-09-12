using GameServices.Interface;
using UnityEngine;

namespace GameServices.Interface
{
    public interface IAudioServiceProvider
    {
        void SetVolume(float volume);
        float GetCurrentVolume();
    }
}

namespace GameServices.AudioService
{
    public class AudioServiceProvider : IAudioServiceProvider
    {
        private static readonly string PlayerPrefsVolumeKey = "PlayerPrefsVolumeKey";
        private float m_currentVolume;

        public AudioServiceProvider()
        {
            // Initialize Audio setting with PlayerPrefs, set default to 1
            SetVolume(PlayerPrefs.GetFloat(PlayerPrefsVolumeKey, 1f));
        }

        public float GetCurrentVolume()
        {
            return m_currentVolume;
        }

        public void SetVolume(float volume)
        {
            m_currentVolume = Mathf.Clamp01(volume);
            // save change to PlayerPrefs
            PlayerPrefs.SetFloat(PlayerPrefsVolumeKey, m_currentVolume);
        }
    }
}