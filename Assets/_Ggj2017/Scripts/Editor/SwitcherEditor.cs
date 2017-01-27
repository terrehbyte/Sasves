using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Switcher))]
public class SwitcherEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Switcher switcher = (Switcher)target;

        DrawDefaultInspector();

        if (Application.isPlaying)
        {
            if (GUILayout.Button("Switch to Next"))
            {
                switcher.SwitchNext();
            }
        }
    }
}