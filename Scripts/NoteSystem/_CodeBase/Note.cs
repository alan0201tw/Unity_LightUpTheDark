using GameServices;
using GameServices.LanguageService;
using System.Collections.Generic;
using UnityEngine;

namespace NoteSystem
{
    [CreateAssetMenu(menuName = "LightUpTheDark/Note", fileName = "NewNote")]
    public class Note : ScriptableObject
    {
        [SerializeField]
        private int guid;
        public int GUID { get { return guid; } }

        [SerializeField]
        private List<LanguageToSingleStringStruct> noteName;

        [SerializeField]
        private List<LanguageToSingleStringStruct> noteContent;

        public string GetNoteNameByLanguageType(LanguageType languageType)
        {
            string target = noteName.Find(x => x.LanguageType == languageType).Content;

            return target != null ? target : string.Empty;
        }

        public string GetNoteContentByLanguageType(LanguageType languageType)
        {
            string target = noteContent.Find(x => x.LanguageType == languageType).Content;

            return target != null ? target : string.Empty;
        }
    }
}