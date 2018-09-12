using UnityEngine;
using GameServices;

namespace DEBUGGING
{
    public class Debugger : MonoBehaviour
    {
# if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                GameServicesLocator.Instance.LanguageServiceProvider.SetLanguageType(LanguageType.Chinese);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                GameServicesLocator.Instance.LanguageServiceProvider.SetLanguageType(LanguageType.English);
            }
        }
# endif
    }
}