using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreatePlanetEditor : EditorWindow
{
    private int planetID;
    private string planetName;
    private float planetRadius = 1f;
    private int planetSubdivisions = 0;
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
      
        if (GUILayout.Button("Create Planet"))
        {
            CreatePlanet();
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

        GameObject _planet = new GameObject();

        _planet.name = planetName + planetID;

        _planet.AddComponent<MeshFilter>();
        _planet.AddComponent<MeshRenderer>();
        _planet.GetComponent<MeshRenderer>().material = material;
        _planet.GetComponent<MeshFilter>().mesh = CreateMesh(planetSubdivisions, planetRadius);

        planetID++;
    }

    public static Mesh CreateMesh(int subdivisions, float radius)
    {
        if (subdivisions < 0)
        {
            subdivisions = 0;
            Debug.LogWarning("Octahedron Sphere subdivisions increased to minimum, which is 0.");
        }
        else if (subdivisions > 6)
        {
            subdivisions = 6;
            Debug.LogWarning("Octahedron Sphere subdivisions decreased to maximum, which is 6.");
        }

        Vector3[] vertices = {
            Vector3.down, Vector3.down, Vector3.down, Vector3.down,
            Vector3.forward,
            Vector3.left,
            Vector3.back,
            Vector3.right,
            Vector3.up, Vector3.up, Vector3.up, Vector3.up
        };

        int[] triangles = {
            0, 4, 5,
            1, 5, 6,
            2, 6, 7,
            3, 7, 4,

             8, 5, 4,
             9, 6, 5,
            10, 7, 6,
            11, 4, 7
        };

        Vector3[] normals = new Vector3[vertices.Length];
        Normalize(vertices, normals);

        Vector2[] uv = new Vector2[vertices.Length];
        CreateUV(vertices, uv);

        if (radius != 1f)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] *= radius;
            }
        }

        Mesh mesh = new Mesh();
        mesh.name = "Octahedron Sphere";
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uv;
        mesh.triangles = triangles;
        return mesh;
    }

    private static void CreateUV(Vector3[] vertices, Vector2[] uv)
    {
        float previousX = 1f;
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 v = vertices[i];
            if (v.x == previousX)
            {
                uv[i - 1].x = 1f;
            }
            previousX = v.x;
            Vector2 textureCoordinates;
            textureCoordinates.x = Mathf.Atan2(v.x, v.z) / (-2f * Mathf.PI);
            if (textureCoordinates.x < 0f)
            {
                textureCoordinates.x += 1f;
            }
            textureCoordinates.y = Mathf.Asin(v.y) / Mathf.PI + 0.5f;
            uv[i] = textureCoordinates;
        }
        uv[vertices.Length - 4].x = uv[0].x = 0.125f;
        uv[vertices.Length - 3].x = uv[1].x = 0.375f;
        uv[vertices.Length - 2].x = uv[2].x = 0.625f;
        uv[vertices.Length - 1].x = uv[3].x = 0.875f;
    }

    private static void Normalize(Vector3[] vertices, Vector3[] normals)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            normals[i] = vertices[i] = vertices[i].normalized;
        }
    }
}