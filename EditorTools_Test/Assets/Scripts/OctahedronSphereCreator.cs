using UnityEngine;

public class OctahedronSphereCreator : MonoBehaviour
{
    public static Mesh Create(int subdivisions, float radius)
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

        int resolution = 1 << subdivisions;
        Vector3[] vertices = new Vector3[(resolution + 1) * (resolution + 1) * 4 -
            (resolution * 2 - 1) * 3];
        int[] triangles = new int[(1 << (subdivisions * 2 + 3)) * 3];
        CreateOctahedron(vertices, triangles, resolution);  

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

    private static void CreateOctahedron(Vector3[] vertices, int[] triangles, int resolution)
    {
        int v = 0;

        for (int i = 0; i < 4; i++)
        {
            vertices[v++] = Vector3.down;
        }

        for (int i = 1; i <= resolution; i++)
        {
            float progress = (float)i / resolution;
            Vector3 from, to;
            vertices[v++] = from = Vector3.Lerp(Vector3.down, Vector3.forward, progress);
            to = Vector3.Lerp(Vector3.down, Vector3.left, progress);
            v = CreateVertexLine(from, to, i, v, vertices);
        }
    }

    private static int CreateVertexLine(Vector3 from, Vector3 to, int steps, int v, Vector3[] vertices)
    {
        for (int i = 1; i <= steps; i++)
        {
            vertices[v++] = Vector3.Lerp(from, to, (float)i / steps);
        }
        return v;
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