using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CircleTriggerColliderGenerator))]
public class CircleTriggerColliderGeneratorInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var myScript = (CircleTriggerColliderGenerator)target;
        if (GUILayout.Button("Generate"))
        {
            myScript.Generate();
        }
    }
}
