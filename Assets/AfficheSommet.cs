using UnityEngine;
using System.Collections.Generic;

public class DisplayVerticesOfHitObject : MonoBehaviour
{
    void Update()
    {
        // Vérifier si le bouton de la souris est enfoncé
        if (Input.GetMouseButtonDown(0))
        {
            // Lancer un rayon depuis la caméra vers le pointeur de la souris
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Effectuer le raycasting et vérifier s'il y a une collision avec un MeshCollider
            if (Physics.Raycast(ray, out hit))
            {
                // Obtenez le GameObject touché par le rayon
                GameObject hitObject = hit.collider.gameObject;

                // Obtenez le MeshFilter attaché au GameObject touché
                MeshFilter meshFilter = hitObject.GetComponent<MeshFilter>();

                // Vérifiez si le MeshFilter existe
                if (meshFilter != null)
                {
                    // Obtenez le maillage du MeshFilter
                    Mesh mesh = meshFilter.mesh;

                    // Obtenez les sommets du maillage
                    Vector3[] vertices = mesh.vertices;

                    // Utilisez un ensemble pour stocker les sommets déjà rencontrés
                    HashSet<Vector3> uniqueVertices = new HashSet<Vector3>();

                    // Affichez les positions des sommets uniques
                    for (int i = 0; i < vertices.Length; i++)
                    {
                        // Vérifiez si le sommet n'a pas déjà été rencontré
                        if (!uniqueVertices.Contains(vertices[i]))
                        {
                            Debug.Log("Sommet " + i + ": " + vertices[i]);
                            uniqueVertices.Add(vertices[i]);
                        }
                    }
                }
                else
                {
                    Debug.LogError("Le GameObject touché n'a pas de MeshFilter attaché !");
                }
            }
        }
    }
}
