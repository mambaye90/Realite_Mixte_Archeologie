using UnityEngine;

public class TriangleInfo : MonoBehaviour
{
    void Update()
    {
        // Si le bouton de la souris est enfoncé
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Effectuer un raycast pour détecter les collisions avec l'objet
            if (Physics.Raycast(ray, out hit))
            {
                // Vérifier si l'objet touché est celui-ci
                if (hit.collider.gameObject == gameObject)
                {
                    // Récupérer le maillage de l'objet
                    Mesh mesh = GetComponent<MeshFilter>().mesh;

                    // Récupérer les triangles du maillage
                    int[] triangles = mesh.triangles;

                    // Récupérer les sommets du maillage
                    Vector3[] vertices = mesh.vertices;

                    // Récupérer l'index du triangle touché
                    int triangleIndex = hit.triangleIndex;

                    // Vérifier si l'index du triangle est valide
                    if (triangleIndex >= 0 && triangleIndex < triangles.Length / 3)
                    {
                        // Afficher les indices des sommets pour chaque triangle
                        for (int i = triangleIndex * 3; i < triangleIndex * 3 + 3; i++)
                        {
                            int vertexIndex = triangles[i];
                            Vector3 vertex = vertices[vertexIndex];
                            Debug.Log("Sommet " + (i - triangleIndex * 3 + 1) + ": " + vertex);
                        }
                    }
                    else
                    {
                        Debug.Log("Aucun triangle touché.");
                    }
                

            }

            // Récupérer les indices des sommets du triangle touché
            /*
            int index1 = triangles[triangleIndex * 3];
            int index2 = triangles[triangleIndex * 3 + 1];
            int index3 = triangles[triangleIndex * 3 + 2];


            // Récupérer les positions des sommets du triangle touché
            Vector3 vertex1 = vertices[index1];
            Vector3 vertex2 = vertices[index2];
            Vector3 vertex3 = vertices[index3];

            // Afficher les indices et positions des sommets du triangle touché
            Debug.Log("Triangle touché :");
            Debug.Log("Sommet 1: " + vertex1);
            Debug.Log("Sommet 2: " + vertex2);
            Debug.Log("Sommet 3: " + vertex3);

            // Calculer la surface du triangle touché
            float surface = CalculateTriangleSurface(vertex1, vertex2, vertex3);

            // Afficher la surface du triangle touché
            Debug.Log("Surface du triangle touché : " + surface + " unités carrées");
            }
            }
            }
            }

            // Méthode pour calculer la surface d'un triangle à partir de ses sommets
            float CalculateTriangleSurface(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
            {
            // Calculer les longueurs des côtés du triangle
            float a = Vector3.Distance(vertex1, vertex2);
            float b = Vector3.Distance(vertex2, vertex3);
            float c = Vector3.Distance(vertex3, vertex1);

            // Calculer le semi-périmètre
            float s = (a + b + c) / 2;

            // Calculer l'aire du triangle en utilisant la formule de Heron
            float surface = Mathf.Sqrt(s * (s - a) * (s - b) * (s - c));

            return surface;
            }
            */
        }
        }
        }
    }
