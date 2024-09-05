using UnityEngine;

namespace MyNamespace
{
    public class GenerateSelectedFace : MonoBehaviour
    {
        public GameObject objectToGenerateFrom; // Objet contenant le mesh d'origine
        public int selectedFaceIndex; // Indice de la face à générer
        public Material generatedFaceMaterial; // Matériau pour la face générée

        void Start()
        {
            if (objectToGenerateFrom == null)
            {
                Debug.LogError("Veuillez assigner l'objet contenant le mesh d'origine.");
                return;
            }

            Mesh originalMesh = objectToGenerateFrom.GetComponent<MeshFilter>().sharedMesh;
            if (originalMesh == null)
            {
                Debug.LogError("Mesh non trouvé sur l'objet spécifié.");
                return;
            }

            int[] triangles = originalMesh.GetTriangles(selectedFaceIndex); // Récupérer les triangles de la face sélectionnée
            Vector3[] vertices = originalMesh.vertices;
            Vector3[] normals = originalMesh.normals;

            Mesh generatedMesh = new Mesh();
            Vector3[] newVertices = new Vector3[triangles.Length];
            Vector3[] newNormals = new Vector3[triangles.Length];
            int[] newTriangles = new int[triangles.Length];

            // Copier les vertices et les normals des triangles sélectionnés
            for (int i = 0; i < triangles.Length; i++)
            {
                newVertices[i] = vertices[triangles[i]];
                newNormals[i] = normals[triangles[i]];
                newTriangles[i] = i; // Recréer les triangles à partir de 0
            }

            generatedMesh.vertices = newVertices;
            generatedMesh.normals = newNormals;
            generatedMesh.triangles = newTriangles;

            // Créer un nouvel objet pour la face générée
            GameObject generatedObject = new GameObject("GeneratedFace");
            generatedObject.transform.position = objectToGenerateFrom.transform.position;
            generatedObject.transform.rotation = objectToGenerateFrom.transform.rotation;

            // Ajouter un MeshFilter et un MeshRenderer à l'objet
            MeshFilter meshFilter = generatedObject.AddComponent<MeshFilter>();
            meshFilter.mesh = generatedMesh;

            MeshRenderer meshRenderer = generatedObject.AddComponent<MeshRenderer>();
            meshRenderer.material = generatedFaceMaterial;
        }
    }
}
