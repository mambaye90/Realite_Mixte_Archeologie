using UnityEngine;

public class CubeFaceCapture : MonoBehaviour
{
    public Camera captureCamera; // Référence à la caméra utilisée pour la capture
    public GameObject cube; // Référence au cube à capturer
    public GameObject spriteHolder; // Référence à l'objet qui affichera le sprite

    private Texture2D capturedTexture; // Texture capturée

    // Capture et affichage de la face du cube
    public void CaptureAndDisplayCubeFace(Vector3 hitPoint, Vector3 hitNormal)
    {
        // Capturer la face du cube
        CaptureCubeFace(hitPoint, hitNormal);

        // Convertir la texture capturée en sprite
        Sprite sprite = ConvertTextureToSprite();

        // Afficher le sprite sur l'objet désigné
        DisplaySprite(sprite);
    }

    // Capturer la face du cube
    private void CaptureCubeFace(Vector3 hitPoint, Vector3 hitNormal)
    {
        // Positionner et orienter la caméra pour capturer la face du cube
        captureCamera.transform.position = hitPoint - hitNormal * 0.1f; // Ajustez la distance si nécessaire
        captureCamera.transform.rotation = Quaternion.LookRotation(hitNormal, Vector3.up);

        // Capturer la vue de la caméra
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        captureCamera.targetTexture = renderTexture;
        captureCamera.Render();

        // Lire les pixels de la texture de rendu
        RenderTexture.active = renderTexture;
        capturedTexture = new Texture2D(renderTexture.width, renderTexture.height);
        capturedTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        capturedTexture.Apply();

        // Libérer la texture de rendu
        RenderTexture.active = null;
        captureCamera.targetTexture = null;
        Destroy(renderTexture);
    }

    // Convertir la texture capturée en sprite
    private Sprite ConvertTextureToSprite()
    {
        return Sprite.Create(capturedTexture, new Rect(0, 0, capturedTexture.width, capturedTexture.height), Vector2.one * 0.5f);
    }

    // Afficher le sprite sur l'objet désigné
    private void DisplaySprite(Sprite sprite)
    {
        // Créer ou mettre à jour le composant SpriteRenderer sur l'objet désigné
        SpriteRenderer spriteRenderer = spriteHolder.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = spriteHolder.AddComponent<SpriteRenderer>();
        }

        // Définir le sprite à afficher
        spriteRenderer.sprite = sprite;
    }
}
