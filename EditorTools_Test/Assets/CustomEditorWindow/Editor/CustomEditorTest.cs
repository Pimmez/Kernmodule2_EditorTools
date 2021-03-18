using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CustomEditorTest : EditorWindow
{
    private string objectEditorName;
    private int objectID = 1;
    private GameObject objectToSpawn;
    private float objectScale;
    private Vector3 objectVector;
    private float radius = 5f;

    [MenuItem("Tools/Custom Editor")]
    public static void ShowWindow()
    {
        GetWindow(typeof(CustomEditorTest));
    }

    private void OnGUI()
    {
        GUILayout.Label("Test Unity Editor", EditorStyles.boldLabel);

        objectEditorName = EditorGUILayout.TextField("Object Editor Name", objectEditorName);
        objectID = EditorGUILayout.IntField("Object ID", objectID);
        objectScale = EditorGUILayout.Slider("Scale", objectScale, 0.5f, 3f);
        objectVector = EditorGUILayout.Vector3Field("Object XYZ", objectVector);
        radius = EditorGUILayout.FloatField("radius", radius);
        objectToSpawn = EditorGUILayout.ObjectField("Object", objectToSpawn, typeof(GameObject), false) as GameObject;
    }
}