using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DEBUGGING
{
    public class FPSChecker : MonoBehaviour
    {
        private float frameRate;
        private float averageFPS;

        private int passedFrameCount;

        [SerializeField]
        private float FPSUpdateRate = 0.1f;

        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 200, 20), "FPS : " + frameRate);
            GUI.Label(new Rect(10, 40, 200, 20), "averageFPS : " + averageFPS);
        }

        private void Start()
        {
            StartCoroutine(UpdateFPS());

            passedFrameCount = 0;
        }

        private void Update()
        {
            passedFrameCount++;

            averageFPS = passedFrameCount / Time.time;
        }

        private IEnumerator UpdateFPS()
        {
            while (true)
            {
                frameRate = 1 / Time.deltaTime;

                yield return new WaitForSeconds(FPSUpdateRate);
            }
        }
    }
}