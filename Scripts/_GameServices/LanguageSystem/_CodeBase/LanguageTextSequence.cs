using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameServices.LanguageService
{
    [CreateAssetMenu(menuName = "LightUpTheDark/LanguageTextSequence", fileName = "NewLanguageTextSequence")]
    public class LanguageTextSequence : ScriptableObject
    {
        [SerializeField]
        private List<LanguageToSequenceStringStruct> lines;

        public string[] GetContentsByLanguageType(LanguageType languageType)
        {
            return lines.Find(x => x.LanguageType == languageType).Contents;
        }
    }
}