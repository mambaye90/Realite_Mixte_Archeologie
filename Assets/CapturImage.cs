using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CameraToRawImage : MonoBehaviour
{
    public Camera captureCamera; // Caméra dont vous voulez capturer le rendu
    public RawImage rawImage; // RawImage où afficher le rendu de la caméra
    

    void Start()
    {
        // Assurez-vous que la caméra et la RawImage sont correctement référencées
        if (captureCamera == null || rawImage == null)
        {
            Debug.LogError("Veuillez attribuer une caméra et une RawImage dans l'inspecteur.");
            return;
        }

        // Capturer le rendu de la caméra
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        captureCamera.targetTexture = renderTexture;
        captureCamera.Render();

        // Créer une texture2D à partir du rendu de la caméra
        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();
        RenderTexture.active = null;

        // Créer un sprite à partir de la texture2D
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

        // Appliquer le sprite à la RawImage
       

        // Libérer la mémoire en détruisant les objets temporaires
        captureCamera.targetTexture = null;
        Destroy(renderTexture);
        Destroy(texture);
    }
}
