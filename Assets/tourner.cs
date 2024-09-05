using UnityEngine;

public class FaceIdentifier3 : MonoBehaviour
{
    void Update()
    {
        // Vérifier si le bouton de la souris est enfoncé
        if (Input.GetMouseButtonDown(0))
        {
            // Lancer un rayon depuis la caméra vers le pointeur de la souris
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            // Effectuer le raycasting et vérifier s'il y a une collision
            if (Physics.Raycast(ray, out hitInfo))
            {
                // Récupérer l'objet touché
                GameObject objectHit = hitInfo.collider.gameObject;

                // Récupérer la normale de la face touchée
                Vector3 hitNormal = hitInfo.normal;

                // Calculer l'angle d'alignement avec Vector3.down
                float angleToAlignWithDown = Vector3.Angle(hitNormal, Vector3.down);

                // Calculer la rotation nécessaire pour aligner la normale avec Vector3.down
                Quaternion rotationToAlignWithDown = Quaternion.AngleAxis(angleToAlignWithDown, Vector3.Cross(hitNormal, Vector3.down));

                // Appliquer la rotation à l'objet touché
                objectHit.transform.rotation = rotationToAlignWithDown;

                // Afficher les informations de l'objet touché, la normale de la face touchée et l'angle d'alignement avec le sol
                Debug.Log("Objet touché : " + objectHit.name);
                Debug.Log("Normale de la face touchée : " + hitNormal);
                Debug.Log("Angle d'alignement avec le bas : " + angleToAlignWithDown + " degrés");
            }
        }
    }
}
