using UnityEngine;
using System.Collections.Generic;

namespace MyNamespace
{
    public class FaceMatching : MonoBehaviour
    {
        public Camera camera1; // Premi�re cam�ra
        public Camera camera2; // Deuxi�me cam�ra
        public float matchingThreshold = 0.1f; // Seuil de correspondance pour d�terminer si les faces s'embo�tent

        void Update()
        {
            // Capturer les images des deux cam�ras
            Texture2D texture1 = CaptureCameraImage(camera1);
            Texture2D texture2 = CaptureCameraImage(camera2);

            // Extraire les sommets des faces � partir des images captur�es
            List<Vector3> vertices1 = ExtractVerticesFromImage(texture1);
            List<Vector3> vertices2 = ExtractVerticesFromImage(texture2);

            // Comparer les ensembles de sommets pour d�terminer si les faces s'embo�tent
            bool facesMatch = CompareVertices(vertices1, vertices2);

            // Afficher le r�sultat
            if (facesMatch)
            {
                Debug.Log("Les faces s'embo�tent !");
            }
            else
            {
                Debug.Log("Les faces ne s'embo�tent pas.");
            }
        }

        // Capturer l'image de la cam�ra et la retourner sous forme de Texture2D
        private Texture2D CaptureCameraImage(Camera cam)
        {
            int height = 1024;
            int width = 1024;
            int depth = 24;
            RenderTexture renderTexture = new RenderTexture(width, height, depth);
            cam.targetTexture = renderTexture;
            cam.Render();

            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            RenderTexture.active = renderTexture;
            texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            texture.Apply();

            cam.targetTexture = null;
            RenderTexture.active = null;
            Destroy(renderTexture);

            return texture;
        }

        // Extraire les sommets des faces � partir d'une image
        private List<Vector3> ExtractVerticesFromImage(Texture2D image)
        {
            // Ajoutez ici la logique pour extraire les sommets des faces � partir de l'image
            // Cette logique d�pend du type de traitement d'image que vous souhaitez effectuer
            // Par exemple, si vous avez un algorithme pour d�tecter les sommets dans une image, vous l'utiliseriez ici

            // Pour cet exemple, nous allons simplement cr�er une liste de sommets al�atoires pour simuler les sommets extraits.
            List<Vector3> vertices = new List<Vector3>();
            int numVertices = Random.Range(3, 10); // Nombre al�atoire de sommets

            for (int i = 0; i < numVertices; i++)
            {
                vertices.Add(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
            }

            return vertices;
        }

        // Comparer les ensembles de sommets pour d�terminer si les faces s'embo�tent
        private bool CompareVertices(List<Vector3> vertices1, List<Vector3> vertices2)
        {
            // V�rifiez si chaque sommet de la premi�re liste est proche de l'un des sommets de la deuxi�me liste
            foreach (Vector3 vertex1 in vertices1)
            {
                foreach (Vector3 vertex2 in vertices2)
                {
                    if (Vector3.Distance(vertex1, vertex2) < matchingThreshold)
                    {
                        return true; // Les faces s'embo�tent
                    }
                }
            }

            return false; // Les faces ne s'embo�tent pas
        }
    }
}
