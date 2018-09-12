using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("Since the scene with map(main) is not used, this manager is obsolete.")]
public class MainSceneManager : MonoBehaviour
{
    [SerializeField]
    private Camera m_camera;

    [SerializeField]
    [Range(0.01f, 0.1f)]
    private float cameraSpeed;

    [SerializeField]
    private Transform topRightPivot;
    [SerializeField]
    private Transform bottomLeftPivot;

    private void Start()
    {
        if (m_camera == null)
        {
            Debug.LogWarning("Camera not assigned, use main camera!");
            m_camera = Camera.main;
        }
    }

    private void Update()
    {
        if (Input.touchCount < 1)
            return;

        Touch touch = Input.GetTouch(0);

        // calculating time interval
        if (touch.phase == TouchPhase.Moved)
        {
            Vector2 movement = touch.deltaPosition * -1;

            m_camera.transform.Translate(movement);

            // check out of bound
            var viewingTopRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
            var viewingBottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));

            float xMin = bottomLeftPivot.position.x;
            float xMax = topRightPivot.position.x;
            float yMin = bottomLeftPivot.position.y;
            float yMax = topRightPivot.position.y;
            // if is out of bound, do reverse translate
            if (viewingTopRight.x > xMax || viewingBottomLeft.x < xMin || viewingTopRight.y > yMax || viewingBottomLeft.y < yMin)
            {
                m_camera.transform.Translate(-movement);
            }
        }
    }
}