using UnityEngine;
using UnityEngine.UI;

public class ExtractVerticesFromRawImage : MonoBehaviour
{
    public RawImage rawImage; // Le composant RawImage contenant l'image
    public int textureWidth = 1920; // Largeur de la texture capturée
    public int textureHeight = 1080; // Hauteur de la texture capturée

    private Texture2D rawTexture;

    void Start()
    {
        rawTexture = new Texture2D(textureWidth, textureHeight);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ExtractVerticesFromTexture((Texture2D)rawImage.texture);
        }
    }

    void ExtractVerticesFromTexture(Texture2D texture)
    {
        if (texture == null)
        {
            Debug.LogError("No texture found in RawImage.");
            return;
        }

        // Parcourir les pixels de l'image pour obtenir les informations de couleur
        UnityEngine.Color[] pixels = texture.GetPixels();
        for (int i = 0; i < pixels.Length; i++)
        {
            UnityEngine.Color pixelColor = pixels[i];

            // Utiliser la couleur du pixel pour extraire les informations de géométrie ou de profondeur
            // Créer les sommets et les triangles en fonction des informations extraites
        }
    }
}
