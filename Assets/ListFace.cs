using UnityEngine;

public class FaceIdentifierWithCollider : MonoBehaviour
{
    private Vector3 lastNormal = Vector3.zero;

    void Update()
    {
        // V�rifier si le bouton de la souris est enfonc�
        if (Input.GetMouseButtonDown(0))
        {
            // Lancer un rayon depuis la cam�ra vers le pointeur de la souris
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            // Effectuer le raycasting et v�rifier s'il y a une collision
            if (Physics.Raycast(ray, out hitInfo))
            {
                // R�cup�rer l'objet touch�
                GameObject objectHit = hitInfo.collider.gameObject;

                // R�cup�rer le composant MeshCollider de l'objet touch�
                MeshCollider meshCollider = objectHit.GetComponent<MeshCollider>();
                if (meshCollider != null)
                {
                    // R�cup�rer le maillage du MeshCollider
                    Mesh mesh = meshCollider.sharedMesh;
                    if (mesh != null)
                    {
                        // R�cup�rer les triangles du maillage
                        int[] triangles = mesh.triangles;

                        // Parcourir tous les triangles pour r�cup�rer les informations de chaque face
                        for (int i = 0; i < triangles.Length; i += 3)
                        {
                            int index1 = triangles[i];
                            int index2 = triangles[i + 1];
                            int index3 = triangles[i + 2];

                            // R�cup�rer les sommets des triangles
                            Vector3 vertex1 = mesh.vertices[index1];
                            Vector3 vertex2 = mesh.vertices[index2];
                            Vector3 vertex3 = mesh.vertices[index3];

                            // Calculer la normale de la face
                            Vector3 normal = Vector3.Cross(vertex2 - vertex1, vertex3 - vertex1).normalized;

                            // V�rifier si la normale est diff�rente de la derni�re normale
                            if (normal != lastNormal)
                            {
                                // Afficher les informations de la face
                                Debug.Log("Face " + (i / 3 + 1) + ": Vertices " + index1 + ", " + index2 + ", " + index3 + " | Normal: " + normal);

                                // Mettre � jour la derni�re normale
                                lastNormal = normal;
                            }
                        }
                    }
                }
            }
        }
    }
}
