using UnityEngine;
using System.Collections.Generic;
using OpenCvSharp;


public class ContourVertexLoader : MonoBehaviour
{
    public Camera captureCamera; // Caméra pour capturer l'image de l'objet
    public int imageWidth = 640; // Largeur de l'image capturée
    public int imageHeight = 480; // Hauteur de l'image capturée
    public GameObject objectToCapture; // Objet à capturer
    public float distanceFromObject = 2f; // Distance de la caméra à l'objet

    void Start()
    {
        // Capturer l'image de l'objet à partir de la caméra
        RenderTexture renderTexture = new RenderTexture(imageWidth, imageHeight, 24);
        captureCamera.targetTexture = renderTexture;
        Texture2D capturedTexture = new Texture2D(imageWidth, imageHeight, TextureFormat.RGB24, false);
        captureCamera.Render();
        RenderTexture.active = renderTexture;
        capturedTexture.ReadPixels(new UnityEngine.Rect(0, 0, imageWidth, imageHeight), 0, 0);
        captureCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        // Convertir la texture en matrice d'image OpenCV
        Mat imageMat = TextureToMat(capturedTexture);

        // Convertir l'image en niveaux de gris
        Mat grayMat = new Mat();
        Cv2.CvtColor(imageMat, grayMat, ColorConversionCodes.BGR2GRAY);

        // Détecter les contours dans l'image
        Point[][] contours;
        HierarchyIndex[] hierarchy;
        Cv2.FindContours(grayMat, out contours, out hierarchy, RetrievalModes.List, ContourApproximationModes.ApproxSimple);

        // Calculer la normale moyenne des contours détectés
        Vector3 averageNormal = Vector3.zero;
        foreach (var contour in contours)
        {
            Vector3 normal = CalculateContourNormal(contour);
            averageNormal += normal;
        }
        averageNormal /= contours.Length;

        // Positionner la caméra à la distance spécifiée de l'objet
        Vector3 cameraPosition = objectToCapture.transform.position + averageNormal * distanceFromObject;

        // Orienter la caméra selon la normale de l'objet
        Quaternion cameraRotation = Quaternion.LookRotation(-averageNormal);

        // Appliquer la position et l'orientation à la caméra
        captureCamera.transform.position = cameraPosition;
        captureCamera.transform.rotation = cameraRotation;
    }

    Vector3 CalculateContourNormal(Point[] contour)
    {
        // Calculer la normale moyenne des sommets du contour
        // (vous devrez implémenter cette fonction selon vos besoins spécifiques)
        // Pour un contour en 2D, vous pouvez calculer la normale en prenant la direction entre deux points consécutifs
        // et en tournant de 90 degrés (dans le plan XY).
        // Vous devrez adapter ce code en fonction de votre méthode spécifique pour calculer la normale.
        return Vector3.up; // Exemple : normale verticale
    }

    Mat TextureToMat(Texture2D texture)
    {
        return OpenCvSharp.Unity.TextureToMat(texture);
    }
}
