using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(DonutSpriteGenerator))]
public class DonutSpriteGeneratorInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var myScript = (DonutSpriteGenerator)target;
        if (GUILayout.Button("Generate"))
        {
            myScript.Generate();
        }
    }
}
