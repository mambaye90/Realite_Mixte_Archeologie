using UnityEngine;

public class MeshGrouping : MonoBehaviour
{
    public MeshFilter meshFilter; // Référence au MeshFilter contenant le mesh à traiter
    public float thresholdAngle = 30f; // Angle seuil pour la similitude des normales
    public UnityEngine.Color groupColor = UnityEngine.Color.red; // Couleur pour les triangles regroupés

    void Start()
    {
        if (meshFilter == null)
        {
            Debug.LogError("Veuillez attribuer un MeshFilter dans l'inspecteur.");
            return;
        }

        Mesh mesh = meshFilter.mesh;
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;
        UnityEngine.Color[] colors = new UnityEngine.Color[vertices.Length];
        int[] triangles = mesh.triangles;

        bool[] grouped = new bool[triangles.Length / 3];

        for (int i = 0; i < triangles.Length; i += 3)
        {
            if (grouped[i / 3])
                continue;

            Vector3 normal1 = normals[triangles[i]];
            Vector3 normal2 = normals[triangles[i + 1]];
            Vector3 normal3 = normals[triangles[i + 2]];

            if (Vector3.Angle(normal1, normal2) < thresholdAngle && Vector3.Angle(normal1, normal3) < thresholdAngle)
            {
                // Group triangles if normals are similar
                grouped[i / 3] = true;
                UnityEngine.Color groupColor = new UnityEngine.Color(Random.value, Random.value, Random.value); // Random color for each group
                colors[triangles[i]] = groupColor;
                colors[triangles[i + 1]] = groupColor;
                colors[triangles[i + 2]] = groupColor;
            }
        }

        mesh.colors = colors; // Assign the colors to the mesh
    }
}
