using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameServices.LanguageService
{
    [System.Serializable]
    public struct LanguageToSequenceStringStruct
    {
        [SerializeField]
        private LanguageType languageType;
        [SerializeField]
        [TextArea]
        private string[] contents;

        public LanguageType LanguageType { get { return languageType; } }
        public string[] Contents { get { return contents; } }
    }

    [System.Serializable]
    public struct LanguageToSingleStringStruct
    {
        [SerializeField]
        private LanguageType languageType;
        [SerializeField]
        [TextArea]
        private string content;

        public LanguageType LanguageType { get { return languageType; } }
        public string Content { get { return content; } }
    }
}