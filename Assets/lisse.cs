using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshSmoother : MonoBehaviour
{
    public int subdivisions = 1; // Nombre de subdivisions à effectuer

    void Start()
    {
        SmoothMesh();
    }

    void SmoothMesh()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null || meshFilter.sharedMesh == null)
        {
            Debug.LogError("MeshFilter or Mesh not found.");
            return;
        }

        Mesh originalMesh = meshFilter.sharedMesh;
        Mesh subdividedMesh = SubdivideMesh(originalMesh, subdivisions);

        // Appliquer le nouveau maillage au MeshFilter
        meshFilter.sharedMesh = subdividedMesh;
    }

    Mesh SubdivideMesh(Mesh mesh, int subdivisions)
    {
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;
        Vector2[] uv = mesh.uv;
        int[] triangles = mesh.triangles;

        for (int sub = 0; sub < subdivisions; sub++)
        {
            int numTriangles = triangles.Length;
            Vector3[] newVertices = new Vector3[numTriangles * 3];
            Vector3[] newNormals = new Vector3[numTriangles * 3];
            Vector2[] newUV = new Vector2[numTriangles * 3];
            int[] newTriangles = new int[numTriangles * 3];

            for (int i = 0; i < numTriangles; i += 3)
            {
                Vector3 v0 = vertices[triangles[i]];
                Vector3 v1 = vertices[triangles[i + 1]];
                Vector3 v2 = vertices[triangles[i + 2]];

                Vector3 mid0 = (v0 + v1) * 0.5f;
                Vector3 mid1 = (v1 + v2) * 0.5f;
                Vector3 mid2 = (v2 + v0) * 0.5f;

                int baseIndex = i * 3;

                newVertices[baseIndex] = v0;
                newVertices[baseIndex + 1] = v1;
                newVertices[baseIndex + 2] = v2;

                newVertices[baseIndex + 3] = mid0;
                newVertices[baseIndex + 4] = mid1;
                newVertices[baseIndex + 5] = mid2;

                newVertices[baseIndex + 6] = mid0;
                newVertices[baseIndex + 7] = mid1;
                newVertices[baseIndex + 8] = mid2;

                newTriangles[baseIndex] = baseIndex;
                newTriangles[baseIndex + 1] = baseIndex + 3;
                newTriangles[baseIndex + 2] = baseIndex + 5;

                newTriangles[baseIndex + 3] = baseIndex + 3;
                newTriangles[baseIndex + 4] = baseIndex + 4;
                newTriangles[baseIndex + 5] = baseIndex + 1;

                newTriangles[baseIndex + 6] = baseIndex + 3;
                newTriangles[baseIndex + 7] = baseIndex + 1;
                newTriangles[baseIndex + 8] = baseIndex + 8;

                newTriangles[baseIndex + 9] = baseIndex + 5;
                newTriangles[baseIndex + 10] = baseIndex + 8;
                newTriangles[baseIndex + 11] = baseIndex + 2;

                newTriangles[baseIndex + 12] = baseIndex + 5;
                newTriangles[baseIndex + 13] = baseIndex + 2;
                newTriangles[baseIndex + 14] = baseIndex + 6;

                newTriangles[baseIndex + 15] = baseIndex + 2;
                newTriangles[baseIndex + 16] = baseIndex + 7;
                newTriangles[baseIndex + 17] = baseIndex + 6;

                newTriangles[baseIndex + 18] = baseIndex + 4;
                newTriangles[baseIndex + 19] = baseIndex + 7;
                newTriangles[baseIndex + 20] = baseIndex + 1;

                newTriangles[baseIndex + 21] = baseIndex + 4;
                newTriangles[baseIndex + 22] = baseIndex + 6;
                newTriangles[baseIndex + 23] = baseIndex + 7;

                // Copier les normales et les UVs
                for (int j = 0; j < 24; j++)
                {
                    newNormals[baseIndex + j] = normals[triangles[i + j / 3]];
                    newUV[baseIndex + j] = uv[triangles[i + j / 3]];
                }
            }

            // Mettre à jour les tableaux de triangles, de sommets, de normales et de coordonnées UV
            vertices = newVertices;
            normals = newNormals;
            uv = newUV;
            triangles = newTriangles;
        }

        // Créer un nouveau maillage avec les données mises à jour
        Mesh subdividedMesh = new Mesh();
        subdividedMesh.vertices = vertices;
        subdividedMesh.normals = normals;
        subdividedMesh.uv = uv;
        subdividedMesh.triangles = triangles;

        return subdividedMesh;
    }
}
