using GameServices.MobileInputService;
using PlayerSystem;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VirtualJoyStick))]
public class JoystickEditor : Editor
{
    public void OnSceneGUI()
    {
        VirtualJoyStick joystick = target as VirtualJoyStick;
        Handles.color = Color.yellow;

        if (!EditorApplication.isPlaying)
            Handles.DrawWireDisc(joystick.transform.position, joystick.transform.forward, joystick.MovementRadius);
        else
            Handles.DrawWireDisc(joystick.InitialPosition, joystick.transform.forward, joystick.MovementRadius);
    }
}