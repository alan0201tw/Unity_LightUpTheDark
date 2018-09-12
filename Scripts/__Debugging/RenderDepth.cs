using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class RenderDepth : MonoBehaviour
{
    [SerializeField]
    private DepthTextureMode mode;

    void OnEnable()
    {
        GetComponent<Camera>().depthTextureMode = mode;
    }

    //private void Update()
    //{
    //    Debug.Log(GetComponent<Camera>().depthTextureMode);
    //}
}