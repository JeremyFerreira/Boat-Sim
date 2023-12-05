using UnityEngine;

public class PlaneGenerator : MonoBehaviour
{
    public int verticesPerSide = 10;

    [EasyButtons.Button]
    public void GeneratePlaneMesh()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();

        // Generate vertices and UVs
        Vector3[] vertices = new Vector3[verticesPerSide * verticesPerSide];
        Vector2[] uvs = new Vector2[verticesPerSide * verticesPerSide];

        for (int i = 0; i < verticesPerSide; i++)
        {
            for (int j = 0; j < verticesPerSide; j++)
            {
                float x = (float)j / (verticesPerSide - 1);
                float z = (float)i / (verticesPerSide - 1);

                vertices[i * verticesPerSide + j] = new Vector3(x, 0, z);
                uvs[i * verticesPerSide + j] = new Vector2(x, z);
            }
        }

        // Assign vertices and UVs to the mesh
        mesh.vertices = vertices;
        mesh.uv = uvs;

        // Generate triangles
        int[] triangles = new int[(verticesPerSide - 1) * (verticesPerSide - 1) * 6];
        int index = 0;

        for (int i = 0; i < verticesPerSide - 1; i++)
        {
            for (int j = 0; j < verticesPerSide - 1; j++)
            {
                int topLeft = i * verticesPerSide + j;
                int topRight = topLeft + 1;
                int bottomLeft = (i + 1) * verticesPerSide + j;
                int bottomRight = bottomLeft + 1;

                triangles[index++] = topLeft;
                triangles[index++] = bottomLeft;
                triangles[index++] = topRight;

                triangles[index++] = topRight;
                triangles[index++] = bottomLeft;
                triangles[index++] = bottomRight;
            }
        }

        // Assign triangles to the mesh
        mesh.triangles = triangles;

        // Recalculate normals and tangents
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        // Assign the generated mesh to the MeshFilter component
        meshFilter.mesh = mesh;
    }
}