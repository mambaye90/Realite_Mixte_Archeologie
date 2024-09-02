using UnityEngine;
using Accord.Math;
using System.Collections.Generic;

using MathNet.Numerics.LinearAlgebra;
using Unity.VisualScripting;
using MathNet.Numerics.RootFinding;
using System.Security.Cryptography;
using System;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine.Diagnostics;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UI;
using OpenCvSharp;
using static OpenCvSharp.Unity;








namespace MyNamespace
{
    public class ICPWithMesh : MonoBehaviour
    {
        public GameObject cibleToDetect; // L'objet dont vous souhaitez aligner les sommets
        public GameObject sourceToDetect; // L'objet source
        public float distanceFromFace = 2f; // Distance � laquelle placer la cam�ra de la surface s�lectionn�e
        public float matchingThreshold = 0.01f; // Seuil de correspondance pour d�terminer si les faces s'embo�tent
        public Camera camera1; // Premi�re cam�ra
        public Camera camera2; // Deuxi�me cam�ra
        public GameObject canvasPrefab1;
        public GameObject canvasPrefab2;
        
        
        void Start()
        {
        } 
        void Update()
        {
            // Capturer les images des deux cam�ras
            //Texture2D sprite1 = CaptureCameraImage(camera1);
            //Texture2D sprite2 = CaptureCameraImage(camera2);

            // Extraire les sommets des faces � partir des images captur�es
            List<UnityEngine.Vector3> targetVertices = LoadMeshVertices(cibleToDetect);
            List<UnityEngine.Vector3> sourceVertices = LoadSourceVertices(sourceToDetect);

            // Aligner les points source avec les points cibles
            AlignPoints(sourceVertices.ToArray(), targetVertices.ToArray(), 0.001f);
            // V�rifier si les faces s'embo�tent
           //CheckFaceMatching(camera1, camera2);


        }


        private void AlignPoints(UnityEngine.Vector3[] sourceVertices, UnityEngine.Vector3[] targetVertices, float threshold)
        {
            UnityEngine.Matrix4x4 transformationMatrix = UnityEngine.Matrix4x4.identity;
            float prevError = float.PositiveInfinity;

            // D�finir le nombre maximum d'it�rations
            int maxIterations = 1000; // ou tout autre nombre souhait�

            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                // Appliquer la transformation actuelle aux points source
                UnityEngine.Vector3[] alignedSourceVertices = ApplyTransformation(sourceVertices, transformationMatrix);

                // Trouver les paires de points les plus proches entre les points source align�s et les points cibles
                List<(UnityEngine.Vector3, UnityEngine.Vector3)> closestPairs = FindClosestPairs(alignedSourceVertices, targetVertices);

                // Mettre � jour la transformation en fonction des paires de points
                UnityEngine.Matrix4x4 newTransformationMatrix = UpdateTransformation(alignedSourceVertices, targetVertices, closestPairs, transformationMatrix);

                // Calculer l'erreur d'alignement actuelle
                float currentError = CalculateAlignmentError(alignedSourceVertices, targetVertices, closestPairs);

                // V�rifier si l'erreur a converg� en dessous du seuil
                if (Mathf.Abs(currentError - prevError) < threshold)
                {
                    Debug.Log("Alignement r�ussi !");

                    // Indique que l'alignement est r�ussi
                }
                else
                {
                    Debug.Log("Alignement NON r�ussi !");
                }
                // Mettre � jour la transformation et l'erreur pr�c�dente pour la prochaine it�ration
                transformationMatrix = newTransformationMatrix;
                prevError = currentError;
            
        }
    }


        // Charger les sommets de la face cible
        private List<UnityEngine.Vector3> LoadMeshVertices(GameObject obj)
        {
            List<UnityEngine.Vector3> cibleVertices = new List<UnityEngine.Vector3>();

            if (obj != null)
            {
                // Assumons que l'information de l'impact est obtenue par ailleurs dans la logique du jeu
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    if (hit.collider.gameObject == obj)
                    {
                        MeshFilter meshFilter = obj.GetComponent<MeshFilter>();

                        if (meshFilter != null && meshFilter.mesh != null)
                        {
                            Mesh mesh = meshFilter.mesh;
                            UnityEngine.Vector3[] meshVertices = mesh.vertices;
                            int[] triangles = mesh.triangles;

                            for (int i = 0; i < triangles.Length; i += 3)
                            {
                                UnityEngine.Vector3 p1 = obj.transform.TransformPoint(meshVertices[triangles[i]]);
                                UnityEngine.Vector3 p2 = obj.transform.TransformPoint(meshVertices[triangles[i + 1]]);
                                UnityEngine.Vector3 p3 = obj.transform.TransformPoint(meshVertices[triangles[i + 2]]);

                                if (IsPointInTriangle(hit.point, p1, p2, p3))
                                {
                                    cibleVertices.Add(p1);
                                    cibleVertices.Add(p2);
                                    cibleVertices.Add(p3);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Debug.LogWarning("Aucun MeshFilter ou maillage trouv� sur l'objet cible.");
                        }
                    }
                }
            }

            return cibleVertices;
        }

        // Charger les sommets de la face source

        private List<UnityEngine.Vector3> LoadSourceVertices(GameObject sourceObject)
        {
            List<UnityEngine.Vector3> faceVertices = new List<UnityEngine.Vector3>();

            // Assuming that hit information is obtained from somewhere in the game logic.
            // Example: RaycastHit hit;
            // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // if (Physics.Raycast(ray, out hit)) { /* process hit */ }
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.collider.gameObject == sourceObject)
                {
                    MeshFilter sourceMeshFilter = sourceObject.GetComponent<MeshFilter>();

                    if (sourceMeshFilter != null && sourceMeshFilter.mesh != null)
                    {
                        Mesh mesh = sourceMeshFilter.mesh;
                        UnityEngine.Vector3[] vertices = mesh.vertices;
                        int[] triangles = mesh.triangles;

                        for (int i = 0; i < triangles.Length; i += 3)
                        {
                            UnityEngine.Vector3 p1 = sourceObject.transform.TransformPoint(vertices[triangles[i]]);
                            UnityEngine.Vector3 p2 = sourceObject.transform.TransformPoint(vertices[triangles[i + 1]]);
                            UnityEngine.Vector3 p3 = sourceObject.transform.TransformPoint(vertices[triangles[i + 2]]);

                            if (IsPointInTriangle(hit.point, p1, p2, p3))
                            {
                                faceVertices.Add(p1);
                                faceVertices.Add(p2);
                                faceVertices.Add(p3);
                                break;
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Aucun MeshFilter ou maillage trouv� sur l'objet source.");
                    }
                }
            }

            return faceVertices;
        }

        private bool IsPointInTriangle(UnityEngine.Vector3 point, UnityEngine.Vector3 v1, UnityEngine.Vector3 v2, UnityEngine.Vector3 v3)
        {
            UnityEngine.Vector3 d1 = point - v1;
            UnityEngine.Vector3 d2 = point - v2;
            UnityEngine.Vector3 d3 = point - v3;

            UnityEngine.Vector3 normal = UnityEngine.Vector3.Cross(v2 - v1, v3 - v1);
            float dot = UnityEngine.Vector3.Dot(normal, UnityEngine.Vector3.Cross(d1, d2));
            float dot1 = UnityEngine.Vector3.Dot(normal, UnityEngine.Vector3.Cross(d2, d3));
            float dot2 = UnityEngine.Vector3.Dot(normal, UnityEngine.Vector3.Cross(d3, d1));

            return (dot >= 0) && (dot1 >= 0) && (dot2 >= 0);
        }


        // Appliquer la transformation actuelle aux points source
        private UnityEngine.Vector3[] ApplyTransformation(UnityEngine.Vector3[] sourceVertices, UnityEngine.Matrix4x4 transformationMatrix)
        {
            UnityEngine.Vector3[] alignedVertices = new UnityEngine.Vector3[sourceVertices.Length];
            for (int i = 0; i < sourceVertices.Length; i++)
            {
                alignedVertices[i] = transformationMatrix.MultiplyPoint(sourceVertices[i]);
            }
            return alignedVertices;
        }

        // Trouver les paires de points les plus proches entre les points source align�s et les points cibles
        private List<(UnityEngine.Vector3, UnityEngine.Vector3)> FindClosestPairs(UnityEngine.Vector3[] alignedSourceVertices, UnityEngine.Vector3[] targetVertices)
        {
            List<(UnityEngine.Vector3, UnityEngine.Vector3)> closestPairs = new List<(UnityEngine.Vector3, UnityEngine.Vector3)>();
            foreach (UnityEngine.Vector3 sourcePoint in alignedSourceVertices)
            {
                float minDistance = float.MaxValue;
                UnityEngine.Vector3 closestTargetPoint = UnityEngine.Vector3.zero;
                foreach (UnityEngine.Vector3 targetPoint in targetVertices)
                {
                    float distance = UnityEngine.Vector3.Distance(sourcePoint, targetPoint);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestTargetPoint = targetPoint;
                    }
                }
                closestPairs.Add((sourcePoint, closestTargetPoint));
            }
            return closestPairs;
        }

        // Mettre � jour la transformation en fonction des paires de points
        private UnityEngine.Matrix4x4 UpdateTransformation(UnityEngine.Vector3[] alignedSourceVertices, UnityEngine.Vector3[] targetVertices, List<(UnityEngine.Vector3, UnityEngine.Vector3)> closestPairs, UnityEngine.Matrix4x4 transformationMatrix)
        {
            // Construire les matrices A et b pour r�soudre Ax = b avec la m�thode des moindres carr�s
            MathNet.Numerics.LinearAlgebra.Matrix<double> A = new MathNet.Numerics.LinearAlgebra.Double.DenseMatrix((closestPairs.Count * 3), 6);
            MathNet.Numerics.LinearAlgebra.Vector<double> b = new MathNet.Numerics.LinearAlgebra.Double.DenseVector(closestPairs.Count * 3);





            // Remplir les matrices A et b
            for (int i = 0; i < closestPairs.Count; i++)
            {
                UnityEngine.Vector3 sourcePoint = closestPairs[i].Item1;
                UnityEngine.Vector3 targetPoint = closestPairs[i].Item2;

                // Ajouter les �l�ments correspondants � A et b
                A[(i * 3), 0] = sourcePoint.x;
                A[(i * 3), 1] = sourcePoint.y;
                A[(i * 3), 2] = sourcePoint.z;
                A[(i * 3), 3] = 1;
                A[(i * 3 + 1), 4] = sourcePoint.x;
                A[(i * 3 + 1), 5] = sourcePoint.y;
              //  A[(i * 3 + 1), 6] = sourcePoint.z;
              //  A[(i * 3 + 1), 7] = 1;
              //  A[(i * 3 + 2), 8] = sourcePoint.x;
               //A[(i * 3 + 2), 9] = sourcePoint.y;
                //A[((i * 3) + 2), 10] = sourcePoint.z;
                //A[(i * 3 + 2), 11] = 1;

                b[(i * 3)] = targetPoint.x;
                b[(i * 3 + 1)] = targetPoint.y;
                b[(i * 3 + 2)] = targetPoint.z;
            }

            // R�soudre Ax = b pour obtenir la mise � jour de la transformation
            MathNet.Numerics.LinearAlgebra.Vector<double> x = A.Solve(b);
            UnityEngine.Matrix4x4 updatedTransformationMatrix = UnityEngine.Matrix4x4.identity;
            updatedTransformationMatrix.SetRow(0, new UnityEngine.Vector4((float)x[0], (float)x[1], (float)x[2], (float)x[3]));
            //updatedTransformationMatrix.SetRow(1, new UnityEngine.Vector4((float)x[4], (float)x[5], (float)x[6], (float)x[7]));
            //updatedTransformationMatrix.SetRow(2, new UnityEngine.Vector4((float)x[8], (float)x[9], (float)x[10], (float)x[11]));

            // Combiner la nouvelle transformation avec la transformation pr�c�dente
            UnityEngine.Matrix4x4 newTransformationMatrix = updatedTransformationMatrix * transformationMatrix;
            return newTransformationMatrix;
        }

        // Calculer l'erreur d'alignement actuelle
        private float CalculateAlignmentError(UnityEngine.Vector3[] alignedSourceVertices, UnityEngine.Vector3[] targetVertices, List<(UnityEngine.Vector3, UnityEngine.Vector3)> closestPairs)
        {
            float error = 0;
            for (int i = 0; i < closestPairs.Count; i++)
            {
                error += UnityEngine.Vector3.Distance(alignedSourceVertices[i], closestPairs[i].Item2);
            }
            return error / closestPairs.Count;
        }
        private bool CheckFaceMatching(Camera cam1, Camera cam2)
        {
            // Capturer les images des deux cam�ras
            Texture2D texture1 = CaptureCameraImage(cam1);
            Texture2D texture2 = CaptureCameraImage(cam2);

            // Associer les textures aux canevas RawImage

            


            // Extraire les sommets des faces � partir des images captur�es
            List<UnityEngine.Vector3> vertices1 = ExtractVerticesFromImage(texture1);
            List<UnityEngine.Vector3> vertices2 = ExtractVerticesFromImage(texture2);


            // Aligner les sommets de la premi�re liste avec ceux de la deuxi�me liste
            AlignPoints(vertices1.ToArray(), vertices2.ToArray(), 0.001f);
            FindClosestPairs(vertices1.ToArray(), vertices2.ToArray());
            

            // V�rifier si chaque sommet align� de la premi�re liste est proche de l'un des sommets align�s de la deuxi�me liste
            foreach (UnityEngine.Vector3 vertex1 in vertices1)
            {
                foreach (UnityEngine.Vector3 vertex2 in vertices2)
                {
                    // Si la distance entre les sommets est inf�rieure au seuil de correspondance, les faces s'embo�tent
                    if (UnityEngine.Vector3.Distance(vertex1, vertex2) < matchingThreshold)
                    {
                       // return true; // Les faces s'embo�tent

                        Debug.Log("Alignement r�ussi !");
                    }
                }
            }

            // Si aucun embo�tement n'est trouv�, les faces ne s'embo�tent pas
            return false;
           

        }
        // Capturer l'image de la cam�ra et la retourner sous forme de Texture2D
        private Texture2D CaptureCameraImage(Camera cam)
        {
            int height = 1024;
            int width = 1024;
            int depth = 24;
            RenderTexture renderTexture = new RenderTexture(width, height, depth);
            UnityEngine.Rect rect = new UnityEngine.Rect(0, 0, width, height);
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

            cam.targetTexture = renderTexture;
            cam.Render();

            RenderTexture.active = renderTexture;
            texture.ReadPixels(rect, 0, 0);
            texture.Apply();

            cam.targetTexture = null;
            RenderTexture.active = null;
            //Destroy(renderTexture);
            return texture;
            

            //Sprite sprite = Sprite.Create(texture, rect, Vector2.zero);

            //return sprite;
        }

        // Capturer l'image de la cam�ra et la retourner sous forme de Texture2D
        // Extraire les sommets des faces � partir d'une image


private List<UnityEngine.Vector3> ExtractVerticesFromImage(Texture2D image)
    {
        List<UnityEngine.Vector3> vertices = new List<UnityEngine.Vector3>();

        // Convertir l'image en niveaux de gris
        Mat grayImage =  TextureToMat(image).CvtColor(ColorConversionCodes.BGR2GRAY);

        // Initialiser le d�tecteur ORB
        ORB orb = ORB.Create();

        // D�tecter les points d'int�r�t dans l'image
        KeyPoint[] keypoints = orb.Detect(grayImage);

        // Convertir les points d'int�r�t en vertices 3D
        foreach (var keypoint in keypoints)
        {
            // Convertir la position du point d'int�r�t en coordonn�es de pixel
            int x = (int)keypoint.Pt.X;
            int y = (int)keypoint.Pt.Y;

                // Convertir les coordonn�es de pixel en coordonn�es 3D
                UnityEngine.Vector3 vertex = new UnityEngine.Vector3(x / (float)grayImage.Width, y / (float)grayImage.Height, 0f);

            // Ajouter le vertex � la liste
            vertices.Add(vertex);
        }

        return vertices;
    }

    // Comparer les ensembles de sommets pour d�terminer si les faces s'embo�tent
    private bool CompareVertices(List<UnityEngine.Vector3> vertices1, List<UnityEngine.Vector3> vertices2)
        {
            // V�rifiez si chaque sommet de la premi�re liste est proche de l'un des sommets de la deuxi�me liste
            foreach (UnityEngine.Vector3 vertex1 in vertices1)
            {
                foreach (UnityEngine.Vector3 vertex2 in vertices2)
                {
                    if (UnityEngine.Vector3.Distance(vertex1, vertex2) < matchingThreshold)
                    {
                        return true; // Les faces s'embo�tent
                    }
                }
            }

            return false; // Les faces ne s'embo�tent 
        }
    }
}
