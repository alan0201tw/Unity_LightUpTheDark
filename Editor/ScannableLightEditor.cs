using GameServices.ScanningService;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScannableLight))]
[CanEditMultipleObjects]
public class ScannableLightEditor : Editor
{
    private static ScannerData scannerData = new ScannerData();

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.BeginVertical();

        EditorGUILayout.LabelField("Below is for testing in editor only : ");

        scannerData.color = EditorGUILayout.ColorField(new GUIContent("Scanner Color"), scannerData.color);
        scannerData.absoluteIntensity = EditorGUILayout.FloatField(new GUIContent("Absolute Intensity"), scannerData.absoluteIntensity);
        scannerData.normalizedIntensity =
            EditorGUILayout.Slider(new GUIContent("Normalized Intensity"), scannerData.normalizedIntensity, 0f, 1f);

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (!Application.isPlaying)
        {
            EditorGUI.BeginDisabledGroup(true);
        }

        if (GUILayout.Button(new GUIContent("Trigger OnBeingScanned"), GUILayout.Width(150)))
        {
            foreach (ScannableLight scannableLight in targets)
            {
                scannableLight.OnBeingScanned(scannerData);
            }
        }

        if (!Application.isPlaying)
        {
            EditorGUI.EndDisabledGroup();
        }

        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("You can only trigger in play mode.", MessageType.None);
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }
}