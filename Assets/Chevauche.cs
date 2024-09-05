using UnityEngine;

public class MeshOverlapDetector : MonoBehaviour
{
    void CheckMeshOverlap(Mesh mesh1, Mesh mesh2)
    {
        // Récupérer les triangles des deux maillages
        int[] triangles1 = mesh1.triangles;
        int[] triangles2 = mesh2.triangles;
        Vector3[] vertices1 = mesh1.vertices;
        Vector3[] vertices2 = mesh2.vertices;

        // Boucle à travers les triangles du premier maillage
        for (int i = 0; i < triangles1.Length; i += 3)
        {
            // Indices des sommets du triangle du premier maillage
            int index1 = triangles1[i];
            int index2 = triangles1[i + 1];
            int index3 = triangles1[i + 2];

            // Coordonnées des sommets du triangle du premier maillage
            Vector3 vertex1 = vertices1[index1];
            Vector3 vertex2 = vertices1[index2];
            Vector3 vertex3 = vertices1[index3];

            // Calculer la normale du triangle du premier maillage
            Vector3 normal = Vector3.Cross(vertex2 - vertex1, vertex3 - vertex1).normalized;

            // Calculer les limites du triangle du premier maillage
            Vector3 minBounds1 = Vector3.Min(vertex1, Vector3.Min(vertex2, vertex3));
            Vector3 maxBounds1 = Vector3.Max(vertex1, Vector3.Max(vertex2, vertex3));

            // Boucle à travers les triangles du deuxième maillage
            for (int j = 0; j < triangles2.Length; j += 3)
            {
                // Indices des sommets du triangle du deuxième maillage
                int index1_2 = triangles2[j];
                int index2_2 = triangles2[j + 1];
                int index3_2 = triangles2[j + 2];

                // Coordonnées des sommets du triangle du deuxième maillage
                Vector3 vertex1_2 = vertices2[index1_2];
                Vector3 vertex2_2 = vertices2[index2_2];
                Vector3 vertex3_2 = vertices2[index3_2];

                // Calculer les limites du triangle du deuxième maillage
                Vector3 minBounds2 = Vector3.Min(vertex1_2, Vector3.Min(vertex2_2, vertex3_2));
                Vector3 maxBounds2 = Vector3.Max(vertex1_2, Vector3.Max(vertex2_2, vertex3_2));

                // Vérifier les intersections entre les limites des triangles des deux maillages
                if (BoundsIntersect(minBounds1, maxBounds1, minBounds2, maxBounds2))
                {
                    // Les triangles se chevauchent, vous pouvez exécuter ici une logique supplémentaire
                    Debug.Log("Overlapping triangles found between mesh 1 and mesh 2!");
                }
            }
        }
    }

    bool BoundsIntersect(Vector3 minBounds1, Vector3 maxBounds1, Vector3 minBounds2, Vector3 maxBounds2)
    {
        // Vérifier si les limites des deux triangles se chevauchent dans les trois dimensions
        return (minBounds1.x <= maxBounds2.x && maxBounds1.x >= minBounds2.x) &&
               (minBounds1.y <= maxBounds2.y && maxBounds1.y >= minBounds2.y) &&
               (minBounds1.z <= maxBounds2.z && maxBounds1.z >= minBounds2.z);
    }
}
