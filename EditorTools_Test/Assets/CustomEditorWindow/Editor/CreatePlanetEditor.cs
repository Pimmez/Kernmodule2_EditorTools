using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreatePlanetEditor : EditorWindow
{
    [HideInInspector]
    private GameObject planet;
    
    private int planetID;
    private string planetName;

    [Range(0, 6)]
    private int planetSubdivisions = 0;
    private float planetRadius = 1f;
    private Material material;

    /// <summary>
    /// Create menu item and show an editorwindow.
    /// </summary>
    [MenuItem("Tools/Create Planet")]
    public static void ShowWindow()
    {
        GetWindow(typeof(CreatePlanetEditor));
    }

    private void OnGUI()
    {
        GUILayout.Label("Planatary Editor", EditorStyles.boldLabel);

        planetID = EditorGUILayout.IntField("PlanetID: ", planetID);
        planetName = EditorGUILayout.TextField("Planet Name: ", planetName);
        planetRadius = EditorGUILayout.FloatField("Planet Radius: ", planetRadius);
        planetSubdivisions = EditorGUILayout.IntField("Planet Subdivisions: ", planetSubdivisions);
        material = EditorGUILayout.ObjectField("Standard Material: ", material, typeof(Material), false) as Material;

        if (GUILayout.Button("Save Mesh"))
        {
            OnMeshSaved();
        }     

        if (GUILayout.Button("Create Planet"))
        {
            CreatePlanet();
        }

        if (GUILayout.Button("Save Planet"))
        {
            OnPlanetSaved(planet);
        }
    }

    private void CreatePlanet()
    {
        if (planetName == string.Empty)
        {
            Debug.LogError("Error: Please enter a base name for the planet");
            return;
        }

        if (material == null)
        {
            Debug.LogError("Error: Please submit material for the planet");
            return;
        }

        planet = new GameObject();

        planet.name = planetName + planetID;

        planet.AddComponent<MeshFilter>();
        planet.AddComponent<MeshRenderer>();
        planet.AddComponent<MeshCollider>();
        planet.GetComponent<MeshRenderer>().material = material;
        planet.GetComponent<MeshFilter>().mesh = OctahedronSphereCreator.Create(planetSubdivisions, planetRadius);

        planetID++;
    }

    private void OnMeshSaved()
    {
        string _path = EditorUtility.SaveFilePanelInProject(
            "Save Custom Mesh",
            "Custom Mesh",
            "asset",
            "Specify where to save the mesh.");
        if (_path.Length > 0)
        {
            Mesh _mesh = OctahedronSphereCreator.Create(planetSubdivisions, planetSubdivisions);
            MeshUtility.Optimize(_mesh);
            AssetDatabase.CreateAsset(_mesh, _path);
            AssetDatabase.SaveAssets();
        }
    }

    private void OnPlanetSaved(GameObject gameObject)
    {
        string _path = EditorUtility.SaveFilePanelInProject(
            "Save planetary gameobject",
            gameObject.name,
            "prefab",
            "Specify where to save the planet.");
        if (_path.Length > 0)
        {
            Mesh _mesh;
            _mesh = Resources.Load<Mesh>("Assets/Resources/");
            gameObject.GetComponent<MeshFilter>().mesh = _mesh;
            PrefabUtility.SaveAsPrefabAsset(gameObject, _path);
            Selection.activeObject = gameObject;
        }
    }
}