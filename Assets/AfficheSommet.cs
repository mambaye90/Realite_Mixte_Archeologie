using UnityEngine;
using System.Collections.Generic;

public class DisplayVerticesOfHitObject : MonoBehaviour
{
    void Update()
    {
        // V�rifier si le bouton de la souris est enfonc�
        if (Input.GetMouseButtonDown(0))
        {
            // Lancer un rayon depuis la cam�ra vers le pointeur de la souris
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Effectuer le raycasting et v�rifier s'il y a une collision avec un MeshCollider
            if (Physics.Raycast(ray, out hit))
            {
                // Obtenez le GameObject touch� par le rayon
                GameObject hitObject = hit.collider.gameObject;

                // Obtenez le MeshFilter attach� au GameObject touch�
                MeshFilter meshFilter = hitObject.GetComponent<MeshFilter>();

                // V�rifiez si le MeshFilter existe
                if (meshFilter != null)
                {
                    // Obtenez le maillage du MeshFilter
                    Mesh mesh = meshFilter.mesh;

                    // Obtenez les sommets du maillage
                    Vector3[] vertices = mesh.vertices;

                    // Utilisez un ensemble pour stocker les sommets d�j� rencontr�s
                    HashSet<Vector3> uniqueVertices = new HashSet<Vector3>();

                    // Affichez les positions des sommets uniques
                    for (int i = 0; i < vertices.Length; i++)
                    {
                        // V�rifiez si le sommet n'a pas d�j� �t� rencontr�
                        if (!uniqueVertices.Contains(vertices[i]))
                        {
                            Debug.Log("Sommet " + i + ": " + vertices[i]);
                            uniqueVertices.Add(vertices[i]);
                        }
                    }
                }
                else
                {
                    Debug.LogError("Le GameObject touch� n'a pas de MeshFilter attach� !");
                }
            }
        }
    }
}
