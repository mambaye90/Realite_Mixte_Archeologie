using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    // Vitesse de rotation de l'objet
    public float rotationSpeed = 5.0f;

    // Variables pour stocker la position précédente de la souris
    private Vector3 previousMousePosition;

    void Update()
    {
        // Vérifier si le bouton de la souris est enfoncé
        if (Input.GetMouseButton(0))
        {
            // Obtenir la position actuelle de la souris
            Vector3 currentMousePosition = Input.mousePosition;

            // Calculer le déplacement de la souris depuis la dernière frame
            Vector3 mouseDelta = currentMousePosition - previousMousePosition;

            // Appliquer la rotation en fonction du déplacement de la souris
            transform.Rotate(Vector3.up, -mouseDelta.x * rotationSpeed, Space.World);
            transform.Rotate(Vector3.right, mouseDelta.y * rotationSpeed, Space.World);

            // Mettre à jour la position précédente de la souris
            previousMousePosition = currentMousePosition;
        }
        else
        {
            // Mettre à jour la position précédente de la souris lorsque le bouton de la souris est relâché
            previousMousePosition = Input.mousePosition;
        }
    }
}
