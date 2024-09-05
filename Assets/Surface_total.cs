using UnityEngine;

public class SurfaceCalculator : MonoBehaviour
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
                GameObject obj = hit.collider.gameObject;

                // Calculer la surface totale de l'objet touché
                float surfaceTotale = CalculateTotalSurface(obj);
                Debug.Log("Surface totale de l'objet touché : " + surfaceTotale);
            }
        }
    }

    float CalculateTotalSurface(GameObject obj)
    {
        float totalSurface = 0.0f;

        // Accéder au composant MeshFilter de l'objet
        MeshFilter meshFilter = obj.GetComponent<MeshFilter>();

        if (meshFilter != null && meshFilter.sharedMesh != null)
        {
            // Obtenir le maillage
            Mesh mesh = meshFilter.sharedMesh;

            // Calculer la surface totale du maillage
            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                Vector3 p0 = mesh.vertices[mesh.triangles[i]];
                Vector3 p1 = mesh.vertices[mesh.triangles[i + 1]];
                Vector3 p2 = mesh.vertices[mesh.triangles[i + 2]];

                Vector3 v0 = p1 - p0;
                Vector3 v1 = p2 - p0;

                totalSurface += Vector3.Cross(v0, v1).magnitude / 2.0f;
            }
        }
        else
        {
            Debug.LogWarning("L'objet ne contient pas de MeshFilter ou le maillage est null.");
        }

        return totalSurface;
    }
}
