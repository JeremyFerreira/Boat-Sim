using UnityEngine;

public class PlaneGenerator : MonoBehaviour
{
    public int verticesPerSide = 10;

    [EasyButtons.Button]
    public void GeneratePlaneMesh()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();

        // Generate vertices
        Vector3[] vertices = new Vector3[verticesPerSide * verticesPerSide];
        for (int i = 0; i < verticesPerSide; i++)
        {
            for (int j = 0; j < verticesPerSide; j++)
            {
                float x = (float)j / (verticesPerSide - 1);
                float z = (float)i / (verticesPerSide - 1);
                vertices[i * verticesPerSide + j] = new Vector3(x, 0, z);
            }
        }

        // Assign vertices to the mesh
        mesh.vertices = vertices;

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

        // Assign the generated mesh to the MeshFilter component
        meshFilter.mesh = mesh;
    }
}