using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CameraToRawImage : MonoBehaviour
{
    public Camera captureCamera; // Cam�ra dont vous voulez capturer le rendu
    public RawImage rawImage; // RawImage o� afficher le rendu de la cam�ra
    

    void Start()
    {
        // Assurez-vous que la cam�ra et la RawImage sont correctement r�f�renc�es
        if (captureCamera == null || rawImage == null)
        {
            Debug.LogError("Veuillez attribuer une cam�ra et une RawImage dans l'inspecteur.");
            return;
        }

        // Capturer le rendu de la cam�ra
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        captureCamera.targetTexture = renderTexture;
        captureCamera.Render();

        // Cr�er une texture2D � partir du rendu de la cam�ra
        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();
        RenderTexture.active = null;

        // Cr�er un sprite � partir de la texture2D
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

        // Appliquer le sprite � la RawImage
       

        // Lib�rer la m�moire en d�truisant les objets temporaires
        captureCamera.targetTexture = null;
        Destroy(renderTexture);
        Destroy(texture);
    }
}
