using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameServices.LanguageService
{
    [CreateAssetMenu(menuName = "LightUpTheDark/LanguageTextSingle", fileName = "NewLanguageTextSingle")]
    public class LanguageTextSingle : ScriptableObject
    {
        [SerializeField]
        private List<LanguageToSingleStringStruct> line;

        public string GetContentByLanguageType(LanguageType languageType)
        {
            return line.Find(x => x.LanguageType == languageType).Content;
        }
    }
}