using UnityEngine;
using UnityEditor;

public class MenuTest : EditorWindow
{
    [MenuItem("Tools/Do Something")]
    static void DoSomething()
    {
        Debug.Log("I did something");
    }
}