using UnityEngine;

public class PointCloudGenerator : MonoBehaviour
{
    // Prefab pour représenter chaque point
    public GameObject pointPrefab;

    // Nombre de points à générer
    public int numPoints = 10;

    // Échelle de la zone dans laquelle les points seront générés
    public float areaScale = 5f;

    void Start()
    {
        GeneratePointCloud();
    }

    void GeneratePointCloud()
    {
        // Générer chaque point
        for (int i = 0; i < numPoints; i++)
        {
            // Générer une position aléatoire dans la zone spécifiée
            Vector3 position = new Vector3(
                Random.Range(-areaScale, areaScale),
                Random.Range(-areaScale, areaScale),
                Random.Range(-areaScale, areaScale)
            );

            // Instancier le prefab du point à la position générée
            GameObject point = Instantiate(pointPrefab, position, Quaternion.identity);
            // Ajouter le point au parent pour une meilleure organisation dans la hiérarchie Unity
            point.transform.parent = transform;
        }
    }
}
