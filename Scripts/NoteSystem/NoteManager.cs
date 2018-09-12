using GameServices;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace NoteSystem
{
    public class NoteManager : MonoBehaviour
    {
        public static NoteManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("NoteManager already exists! instance at : " + Instance.name);
            }
            else
            {
                Instance = this;
            }
        }

        [SerializeField]
        private GameObject notePrefab;
        [SerializeField]
        private Canvas targetCanvas;

        private List<Note> m_noteList = new List<Note>();
        public List<Note> NoteList { get { return m_noteList; } }

        private NoteBookInterface m_noteBookInterface;
        private int m_currentNoteIndex;

        public bool IsAtLastPage { get { return m_currentNoteIndex >= m_noteList.Count - 2; } }
        public bool IsAtFirstPage { get { return m_currentNoteIndex <= 0; } }
        // if the interface is not null, the notebook is being displayed
        public bool IsDisplayingNotebook { get { return m_noteBookInterface != null; } }

        public void LoadNoteFromGUIDList(List<int> GUID_List)
        {
            Note[] allNotes = Resources.LoadAll("Notes", typeof(Note)).Cast<Note>().ToArray();

            Note[] addingNotes = Array.FindAll(allNotes, (note) =>
            {
                return GUID_List.Contains(note.GUID);
            });

            AddNoteToListRange(addingNotes.ToList());
        }

        public void AddNoteToList(Note note)
        {
            if (note == null || m_noteList.Contains(note))
                return;
            else
                m_noteList.Add(note);

            RemoveDuplictedNote();
        }

        public void AddNoteToListRange(List<Note> notes)
        {
            if (notes == null) return;

            m_noteList.AddRange(notes);
            RemoveDuplictedNote();
        }

        private void RemoveDuplictedNote()
        {
            m_noteList = m_noteList.Distinct().ToList();
        }

        public void DisplayNoteBook()
        {
            if(IsDisplayingNotebook)
            {
                return;
            }

            m_currentNoteIndex = 0;
            GameObject noteGameObject = Instantiate(notePrefab, targetCanvas.transform);
            m_noteBookInterface = new NoteBookInterface(noteGameObject);

            // for outer access of a reference, make sure you're using a class rather than a struct
            m_noteBookInterface.previousPageButton.onClick.AddListener(ToPreviousPage);
            m_noteBookInterface.nextPageButton.onClick.AddListener(ToNextPage);
            m_noteBookInterface.closeButton.onClick.AddListener(() =>
            {
                Destroy(noteGameObject);
                m_noteBookInterface = null;
            });

            UpdateNoteBookInterface();
        }

        private void ToPreviousPage()
        {
            if (IsAtFirstPage)
                return;

            m_currentNoteIndex -= 2;
            UpdateNoteBookInterface();
        }

        private void ToNextPage()
        {
            if (IsAtLastPage)
                return;

            m_currentNoteIndex += 2;
            UpdateNoteBookInterface();
        }

        private void UpdateNoteBookInterface()
        {
            m_noteBookInterface.UpdateButtonStatus(IsAtFirstPage, IsAtLastPage);
            m_noteBookInterface.UpdateTextContent(m_noteList, m_currentNoteIndex);
        }

        private class NoteBookInterface
        {
            public Text leftTitle;
            public Text rightTitle;

            public Text leftText;
            public Text rightText;

            public Button previousPageButton;
            public Button nextPageButton;
            public Button closeButton;

            public NoteBookInterface(GameObject noteGameObject)
            {
                string leftTitleIdentifier = "Notebook/LeftTitle";
                string rightTitleIdentifier = "Notebook/RightTitle";
                string leftTextIdentifier = "Notebook/LeftText";
                string rightTextIdentifier = "Notebook/RightText";

                string previousPageButtonIdentifier = "Notebook/PreviousPageButton";
                string nextPageButtonIdentifier = "Notebook/NextPageButton";
                string closeButtonIdentifier = "Notebook/CloseButton";

                leftTitle = noteGameObject.transform.Find(leftTitleIdentifier).GetComponent<Text>();
                rightTitle = noteGameObject.transform.Find(rightTitleIdentifier).GetComponent<Text>();
                leftText = noteGameObject.transform.Find(leftTextIdentifier).GetComponent<Text>();
                rightText = noteGameObject.transform.Find(rightTextIdentifier).GetComponent<Text>();

                previousPageButton = noteGameObject.transform.Find(previousPageButtonIdentifier).GetComponent<Button>();
                nextPageButton = noteGameObject.transform.Find(nextPageButtonIdentifier).GetComponent<Button>();
                closeButton = noteGameObject.transform.Find(closeButtonIdentifier).GetComponent<Button>();
            }

            public void UpdateButtonStatus(bool isAtFirstPage, bool isAtLastPage)
            {
                previousPageButton.gameObject.SetActive(!isAtFirstPage);
                nextPageButton.gameObject.SetActive(!isAtLastPage);
            }

            public void UpdateTextContent(List<Note> noteList, int currentNoteIndex)
            {
                LanguageType languageType = LanguageType.Chinese;
                languageType = GameServicesLocator.Instance.LanguageServiceProvider.GetCurrentLanguageType();

                // deal with left text
                if (currentNoteIndex < noteList.Count)
                {
                    leftTitle.text = noteList[currentNoteIndex].GetNoteNameByLanguageType(languageType);
                    leftText.text = noteList[currentNoteIndex].GetNoteContentByLanguageType(languageType);
                }
                // deal with right text
                if (currentNoteIndex + 1 < noteList.Count)
                {
                    rightTitle.text = noteList[currentNoteIndex + 1].GetNoteNameByLanguageType(languageType);
                    rightText.text = noteList[currentNoteIndex + 1].GetNoteContentByLanguageType(languageType);
                }
                else
                {
                    rightTitle.text = string.Empty;
                    rightText.text = string.Empty;
                }
            }
        }

        // DEBUGGING
        //[SerializeField]
        //private List<Note> debugNotes;
        //[SerializeField]
        //private List<int> debugIntegerList;

        //private void Start()
        //{
        //    AddNoteToListRange(m_noteList);

        //    LoadNoteFromGUIDList(debugIntegerList);
        //}

        //private void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.A))
        //    {
        //        DisplayNoteBook();
        //    }
        //}
        /////////////
    }
}