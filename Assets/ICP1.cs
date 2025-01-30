using System.Collections.Generic;
using UnityEngine;

public class FacePointGenerator : MonoBehaviour
{
    public int numPoints = 1000; // Nombre de points à générer par face
    private List<Vector3> pointsFace1 = new List<Vector3>();
    private List<Vector3> pointsFace2 = new List<Vector3>();
    private bool isFirstFaceSelected = false;
    private bool isSecondFaceSelected = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 0 = clic gauche
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                MeshCollider meshCollider = hit.collider.GetComponent<MeshCollider>();

                if (meshCollider != null)
                {
                    MeshFilter meshFilter = meshCollider.GetComponent<MeshFilter>();
                    if (meshFilter != null)
                    {
                        Mesh mesh = meshFilter.mesh;
                        int triangleIndex = hit.triangleIndex;

                        Vector3[] vertices = mesh.vertices;
                        int[] triangles = mesh.triangles;

                        if (triangleIndex >= 0 && triangleIndex * 3 + 2 < triangles.Length)
                        {
                            // Extraction des sommets du triangle sélectionné
                            Vector3 v0 = meshFilter.transform.TransformPoint(vertices[triangles[triangleIndex * 3]]);
                            Vector3 v1 = meshFilter.transform.TransformPoint(vertices[triangles[triangleIndex * 3 + 1]]);
                            Vector3 v2 = meshFilter.transform.TransformPoint(vertices[triangles[triangleIndex * 3 + 2]]);

                            // Générer des points sur la face
                            List<Vector3> generatedPoints = GeneratePointsOnFace(v0, v1, v2, numPoints);

                            if (!isFirstFaceSelected)
                            {
                                pointsFace1 = generatedPoints;
                                Debug.Log($"Première face sélectionnée avec {pointsFace1.Count} points.");
                                isFirstFaceSelected = true;
                            }
                            else if (!isSecondFaceSelected)
                            {
                                pointsFace2 = generatedPoints;
                                Debug.Log($"Deuxième face sélectionnée avec {pointsFace2.Count} points.");

                                // Appliquer l'algorithme ICP
                                Matrix4x4 transformation = ICP(new List<Vector3>(pointsFace1), new List<Vector3>(pointsFace2));

                                // Transformation des points de la deuxième face
                                List<Vector3> transformedPointsFace2 = ApplyTransformation(pointsFace2, transformation);

                                // Vérification d'emboîtement
                                bool facesMatch = CheckIfFacesInterlock(pointsFace1, transformedPointsFace2);

                                if (facesMatch)
                                {
                                    Debug.Log("Les faces s'emboîtent correctement !");
                                }
                                else
                                {
                                    Debug.Log("Les faces ne s'emboîtent pas.");
                                }

                                // Réinitialisation
                                ResetSelection();
                            }
                        }
                    }
                }
            }
        }
    }

    private List<Vector3> GeneratePointsOnFace(Vector3 v0, Vector3 v1, Vector3 v2, int numPoints)
    {
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < numPoints; i++)
        {
            float r1 = Random.value;
            float r2 = Random.value;

            if (r1 + r2 > 1)
            {
                r1 = 1 - r1;
                r2 = 1 - r2;
            }

            float r3 = 1 - r1 - r2;

            Vector3 point = r1 * v0 + r2 * v1 + r3 * v2;
            points.Add(point);
        }

        return points;
    }

    private Matrix4x4 ICP(List<Vector3> sourcePoints, List<Vector3> targetPoints)
    {
        Matrix4x4 transformation = Matrix4x4.identity;
        int maxIterations = 50;
        float epsilon = 0.0001f;

        for (int iteration = 0; iteration < maxIterations; iteration++)
        {
            // Étape 1 : Correspondances
            List<Vector3> correspondences = FindCorrespondences(sourcePoints, targetPoints);

            // Étape 2 : Calcul de la transformation
            Matrix4x4 newTransformation = CalculateTransformation(sourcePoints, correspondences);

            // Étape 3 : Application de la transformation
            for (int i = 0; i < sourcePoints.Count; i++)
            {
                sourcePoints[i] = newTransformation.MultiplyPoint3x4(sourcePoints[i]);
            }

            // Étape 4 : Convergence
            if (HasConverged(newTransformation, epsilon))
            {
                Debug.Log($"ICP convergé en {iteration + 1} itérations.");
                return newTransformation;
            }
        }

        Debug.LogWarning("ICP n'a pas convergé.");
        return transformation;
    }

    private List<Vector3> FindCorrespondences(List<Vector3> sourcePoints, List<Vector3> targetPoints)
    {
        List<Vector3> correspondences = new List<Vector3>();

        foreach (var sourcePoint in sourcePoints)
        {
            Vector3 closestPoint = targetPoints[0];
            float minDistance = Vector3.Distance(sourcePoint, closestPoint);

            foreach (var targetPoint in targetPoints)
            {
                float distance = Vector3.Distance(sourcePoint, targetPoint);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestPoint = targetPoint;
                }
            }

            correspondences.Add(closestPoint);
        }

        return correspondences;
    }

    private Matrix4x4 CalculateTransformation(List<Vector3> sourcePoints, List<Vector3> targetPoints)
    {
        Vector3 sourceCenter = CalculateCenter(sourcePoints);
        Vector3 targetCenter = CalculateCenter(targetPoints);

        // Calculer la matrice de covariance
        Matrix4x4 covariance = Matrix4x4.zero;
        for (int i = 0; i < sourcePoints.Count; i++)
        {
            Vector3 sourcePoint = sourcePoints[i] - sourceCenter;
            Vector3 targetPoint = targetPoints[i] - targetCenter;

            covariance.m00 += sourcePoint.x * targetPoint.x;
            covariance.m01 += sourcePoint.x * targetPoint.y;
            covariance.m02 += sourcePoint.x * targetPoint.z;

            covariance.m10 += sourcePoint.y * targetPoint.x;
            covariance.m11 += sourcePoint.y * targetPoint.y;
            covariance.m12 += sourcePoint.y * targetPoint.z;

            covariance.m20 += sourcePoint.z * targetPoint.x;
            covariance.m21 += sourcePoint.z * targetPoint.y;
            covariance.m22 += sourcePoint.z * targetPoint.z;
        }

        // Calculer la rotation à partir de la matrice de covariance
        Quaternion rotation = Quaternion.FromToRotation(sourceCenter, targetCenter);

        // Calculer la translation
        Vector3 translation = targetCenter - rotation * sourceCenter;

        return Matrix4x4.TRS(translation, rotation, Vector3.one);
    }

    private Vector3 CalculateCenter(List<Vector3> points)
    {
        Vector3 center = Vector3.zero;

        foreach (var point in points)
        {
            center += point;
        }

        return center / points.Count;
    }

    private bool HasConverged(Matrix4x4 transformation, float epsilon)
    {
        return transformation.GetPosition().magnitude < epsilon && Quaternion.Angle(Quaternion.identity, transformation.rotation) < epsilon;
    }

    private List<Vector3> ApplyTransformation(List<Vector3> points, Matrix4x4 transformation)
    {
        List<Vector3> transformedPoints = new List<Vector3>();

        foreach (var point in points)
        {
            transformedPoints.Add(transformation.MultiplyPoint3x4(point));
        }

        return transformedPoints;
    }

    private bool CheckIfFacesInterlock(List<Vector3> face1, List<Vector3> face2)
    {
        const float thresholdDistance = 0.05f;
        int matchCount = 0;

        foreach (var point1 in face1)
        {
            foreach (var point2 in face2)
            {
                if (Vector3.Distance(point1, point2) < thresholdDistance)
                {
                    matchCount++;
                    break;
                }
            }
        }

        // Si au moins 90% des points correspondent, les faces s'emboîtent
        return (float)matchCount / face1.Count >= 0.9f;
    }

    private void ResetSelection()
    {
        isFirstFaceSelected = false;
        isSecondFaceSelected = false;
        pointsFace1.Clear();
        pointsFace2.Clear();
        Debug.Log("État réinitialisé.");
    }
}
