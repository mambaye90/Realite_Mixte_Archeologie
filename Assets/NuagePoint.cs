using UnityEngine;

public class PointCloudGenerator : MonoBehaviour
{
    // Prefab pour repr�senter chaque point
    public GameObject pointPrefab;

    // Nombre de points � g�n�rer
    public int numPoints = 10;

    // �chelle de la zone dans laquelle les points seront g�n�r�s
    public float areaScale = 5f;

    void Start()
    {
        GeneratePointCloud();
    }

    void GeneratePointCloud()
    {
        // G�n�rer chaque point
        for (int i = 0; i < numPoints; i++)
        {
            // G�n�rer une position al�atoire dans la zone sp�cifi�e
            Vector3 position = new Vector3(
                Random.Range(-areaScale, areaScale),
                Random.Range(-areaScale, areaScale),
                Random.Range(-areaScale, areaScale)
            );

            // Instancier le prefab du point � la position g�n�r�e
            GameObject point = Instantiate(pointPrefab, position, Quaternion.identity);
            // Ajouter le point au parent pour une meilleure organisation dans la hi�rarchie Unity
            point.transform.parent = transform;
        }
    }
}
