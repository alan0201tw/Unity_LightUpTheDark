using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LUTD.VisualEffects
{
    public class DissolveBehavior : MonoBehaviour
    {
        private Material m_dissolveMaterial;
        private readonly float fadeInTime = 3f;

        public void TriggerFadeIn()
        {
            gameObject.SetActive(true);
            StartCoroutine(FadeIn());
        }

        private IEnumerator FadeIn()
        {
            float timeCounter = 0f;

            while (timeCounter < fadeInTime)
            {
                foreach (Material mat in GetComponentInChildren<Renderer>().materials)
                {
                    mat.SetFloat("_Threshold", 1 - (timeCounter / fadeInTime));
                }

                timeCounter += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            foreach (Material mat in GetComponentInChildren<Renderer>().materials)
            {
                mat.SetFloat("_Threshold", 0);
            }
        }
    }
}