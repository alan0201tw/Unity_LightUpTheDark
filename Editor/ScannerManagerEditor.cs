using UnityEditor;
using UnityEngine;
using GameServices.ScanningService;

[CustomEditor(typeof(ScanningServiceProvider))]
[CanEditMultipleObjects]
public class ScannerManagerEditor : Editor
{
    private Color lastColor;

    public override void OnInspectorGUI()
    {
        ScanningServiceProvider manager = target as ScanningServiceProvider;

        GUILayout.BeginVertical();
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Current Scanning color :");

        Rect rect = GUILayoutUtility.GetLastRect();
        rect.y += 1;
        rect.width /= 2;
        rect.x += rect.width;

        Color color = manager.ScannerColor;
        color.a = 1;

        EditorGUI.DrawRect(rect, color);

        GUILayout.EndVertical();
        Repaint();
    }
}