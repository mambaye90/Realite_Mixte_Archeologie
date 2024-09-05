using UnityEngine;
using System.Collections.Generic;

public class FaceIdentifier2 : MonoBehaviour
{
    private Vector3 lastNormal = Vector3.zero;

    void Update()
    {
        // Vérifier si le bouton de la souris est enfoncé
        if (Input.GetMouseButtonDown(0))
        {
            // Lancer un rayon depuis la caméra vers le pointeur de la souris
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            // Effectuer le raycasting et vérifier s'il y a une collision
            if (Physics.Raycast(ray, out hitInfo))
            {
                // Récupérer l'objet touché
                GameObject objectHit = hitInfo.collider.gameObject;

                // Récupérer la normale de la face touchée
                Vector3 hitNormal = hitInfo.normal;

                // Afficher les informations de l'objet touché et de la normale de la face touchée
                Debug.Log("Objet touché : " + objectHit.name);
                Debug.Log("Normale de la face touchée : " + hitNormal);

                // Récupérer le composant MeshFilter de l'objet touché
                MeshFilter meshFilter = objectHit.GetComponent<MeshFilter>();
                if (meshFilter != null)
                {
                    // Récupérer le maillage du composant MeshFilter
                    Mesh mesh = meshFilter.sharedMesh;
                    if (mesh != null)
                    {
                        // Récupérer les triangles du maillage
                        int[] triangles = mesh.triangles;
                        int j = 0;
                        // Parcourir tous les triangles pour récupérer les informations de chaque face
                        for (int i = 0; i < triangles.Length; i += 3)
                        {
                            int index1 = triangles[i];
                            int index2 = triangles[i + 1];
                            int index3 = triangles[i + 2];

                            // Récupérer les sommets des triangles
                            Vector3 vertex1 = mesh.vertices[index1];
                            Vector3 vertex2 = mesh.vertices[index2];
                            Vector3 vertex3 = mesh.vertices[index3];

                            // Calculer la normale de la face
                            Vector3 normal = Vector3.Cross(vertex2 - vertex1, vertex3 - vertex1).normalized;

                            // Vérifier si la normale est différente de la dernière normale
                            

                            if (normal != lastNormal)
                            {
                                // Afficher les informations de la face
                                j++;
                                Debug.Log("Face " + (j)+ ": Normal: " + normal);

                                // Mettre à jour la dernière normale
                                lastNormal = normal;
                            }
                        }
                    }
                }
            }
        }
    }
}
