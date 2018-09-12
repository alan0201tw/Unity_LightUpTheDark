using GameServices.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameServices.TextDisplayService
{
    // TextDisplayServiceProvider is MonoBehaviour, so TextDisplay in each scene can be easily changed
    // Just define multiple classes that inherits the ITextDisplayServiceProvider interface, and
    // register a instance to GameServicesLocator

    // Also, this implementation of ITextDisplayServiceProvider is for displaying Darksoul's
    // dialogue style texts
    public class TextDisplayServiceProvider : MonoBehaviour, ITextDisplayServiceProvider
    {
        // TODO : change this approach to an event-queue system
        // external code register string to queue, or call DisplayImmediate

        [SerializeField]
        private Canvas targetCanvas;
        [SerializeField]
        private GameObject textDisplayPrefab;

        private class QueueUnit
        {
            public string content;
            public float durationTime;
            public Action onDisplayEnded;

            public QueueUnit(string content, float durationTime, Action onDisplayEnded)
            {
                this.content = content;
                this.durationTime = durationTime;
                this.onDisplayEnded = onDisplayEnded;
            }
        }

        private Queue<QueueUnit> m_displayRequestQueue = new Queue<QueueUnit>();

        private GameObject m_currentDisplayingText;

        private void Awake()
        {
            ProvideService();
        }

        private void ProvideService()
        {
            GameServicesLocator.Instance.TextDisplayServiceProvider = this;
            StartCoroutine(ConstantWorkingCoroutine());
        }

        public void DisplayText(string content, float durationTime, Action onDisplayEnded)
        {
            // for a regular displaying text, just put it in the queue
            m_displayRequestQueue.Enqueue(new QueueUnit(content, durationTime, onDisplayEnded));
        }

        public void DisplayTextImmediate(string content, float durationTime, Action onDisplayEnded)
        {
            // if currently is displaying some text, just override it
            if (m_currentDisplayingText != null)
            {
                m_currentDisplayingText.GetComponentInChildren<Text>().text = content;
            }
            else // otherwise just regularly display it
            {
                DisplayText(content, durationTime, onDisplayEnded);
            }
        }

        // This coroutine will be running as long as the provider instance exists
        private IEnumerator ConstantWorkingCoroutine()
        {
            while (true)
            {
                // if no request, just wait until it does
                while (m_displayRequestQueue.Count == 0)
                {
                    yield return new WaitForEndOfFrame();
                }

                bool isDisplayNewlyCreated = false;
                // if a displaying prefab is not there for us th edit the text, create one
                if (m_currentDisplayingText == null)
                {
                    m_currentDisplayingText = Instantiate(textDisplayPrefab, targetCanvas.transform);
                    isDisplayNewlyCreated = true;
                }

                // retrieve resolving unit and set data to text accordingly
                QueueUnit currentUnit = m_displayRequestQueue.Dequeue();
                m_currentDisplayingText.GetComponentInChildren<Text>().text = currentUnit.content;

                // if we just created the displaying prefab, we need to fade it in
                if (isDisplayNewlyCreated)
                {
                    yield return StartCoroutine(FadingCoroutine(true));
                }

                // just let the text hang in there
                // might add a InterruptAction so player can click to skip a text
                yield return new WaitForSeconds(currentUnit.durationTime);

                //// Also, if you need to read the input from a coroutine, make sure to skip a frame
                //// to refresh the input states
                //yield return new WaitUntil(() =>
                //{
                //    return Input.GetMouseButtonDown(0);
                //});
                //// refresh input states
                //yield return new WaitForEndOfFrame();

                if (currentUnit.onDisplayEnded != null)
                    currentUnit.onDisplayEnded.Invoke();

                if (m_displayRequestQueue.Count == 0)
                {
                    yield return StartCoroutine(FadingCoroutine(false));

                    // !! IMPORTANT !! : Unity only destroy GameObejcts at the end of the frame, 
                    // so we need to wait an additional frame to actually destroy the 
                    // m_currentDisplayingText. Otherwise it might lead to judgemental bug when
                    // deciding whether m_currentDisplayingText is null in next loop.
                    Destroy(m_currentDisplayingText);
                    yield return new WaitForEndOfFrame();
                }
            }
        }

        private IEnumerator FadingCoroutine(bool isFadingIn)
        {
            float passTime = 0f;
            float totalTime = 0.5f;

            float startingAlpha = isFadingIn ? 0 : 1;
            float endingAlpha = 1 - startingAlpha;

            while (passTime < totalTime)
            {
                passTime += Time.deltaTime;
                // change alpha value in graphic's color field
                foreach (Graphic graphic in m_currentDisplayingText.GetComponentsInChildren(typeof(Graphic)))
                {
                    graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, Mathf.Lerp(startingAlpha, endingAlpha, passTime / totalTime));
                }

                yield return new WaitForEndOfFrame();
            }
        }
    }
}