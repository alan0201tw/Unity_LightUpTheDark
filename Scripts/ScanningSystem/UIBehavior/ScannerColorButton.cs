using GameServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameServices.ScanningService
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Button))]
    public class ScannerColorButton : MonoBehaviour
    {
        private Color m_color;

        private void Start()
        {
            m_color = GetComponent<Image>().color;

            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            GameServicesLocator.Instance.ScanningServiceProvider.ScannerColor = m_color;
        }
    }
}