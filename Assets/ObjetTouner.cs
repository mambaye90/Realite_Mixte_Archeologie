using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    // Vitesse de rotation de l'objet
    public float rotationSpeed = 5.0f;

    // Variables pour stocker la position pr�c�dente de la souris
    private Vector3 previousMousePosition;

    void Update()
    {
        // V�rifier si le bouton de la souris est enfonc�
        if (Input.GetMouseButton(0))
        {
            // Obtenir la position actuelle de la souris
            Vector3 currentMousePosition = Input.mousePosition;

            // Calculer le d�placement de la souris depuis la derni�re frame
            Vector3 mouseDelta = currentMousePosition - previousMousePosition;

            // Appliquer la rotation en fonction du d�placement de la souris
            transform.Rotate(Vector3.up, -mouseDelta.x * rotationSpeed, Space.World);
            transform.Rotate(Vector3.right, mouseDelta.y * rotationSpeed, Space.World);

            // Mettre � jour la position pr�c�dente de la souris
            previousMousePosition = currentMousePosition;
        }
        else
        {
            // Mettre � jour la position pr�c�dente de la souris lorsque le bouton de la souris est rel�ch�
            previousMousePosition = Input.mousePosition;
        }
    }
}
