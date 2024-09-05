using UnityEngine;
using UnityEngine.UI;

public class DisplayCameraTextures : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;
    public Transform object1;
    public Transform object2;
    public RawImage rawImage1;
    public RawImage rawImage2;

    void Update()
    {
        // Associer chaque cam�ra � son objet de la sc�ne et placer la cam�ra � 3 m�tres de l'objet
        SetupCamera(camera1, object1, 3f);
        SetupCamera(camera2, object2, 3f);

        // Capturer les images des deux cam�ras
        Texture2D texture1 = CaptureCameraImage(camera1);
        Texture2D texture2 = CaptureCameraImage(camera2);

        // Afficher les textures sur les objets RawImage
        //DisplayTextureOnRawImage(texture1, rawImage1);
        //DisplayTextureOnRawImage(texture2, rawImage2);
    }

    void SetupCamera(Camera cam, Transform obj, float distance)
    {
        // Placer la cam�ra � une distance sp�cifique de l'objet
        Vector3 direction = obj.position - cam.transform.position;
        cam.transform.position += direction.normalized * distance;

        // Orienter la cam�ra vers l'objet
        cam.transform.LookAt(obj);
    }

    Texture2D CaptureCameraImage(Camera cam)
    {
        // Capture d'image � partir de la cam�ra
        // Impl�mentez cette fonction selon votre m�thode de capture d'image

        int height = 1024;
        int width = 1024;
        int depth = 24;
        RenderTexture renderTexture = new RenderTexture(width, height, depth);
        Rect rect = new Rect(0, 0, width, height);
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
    }

    void DisplayTextureOnRawImage(Texture2D texture, RawImage rawImage)
    {
        rawImage.texture = texture;
    }
}
