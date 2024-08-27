using UnityEngine;

public class FaceIdentifier1 : MonoBehaviour
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

                // Calculer la rotation nécessaire pour aligner la normale avec un vecteur directionnel spécifique
                Quaternion rotationToAlign = Quaternion.LookRotation(hitNormal, Vector3.up);

                // Appliquer la rotation à l'objet touché
                objectHit.transform.rotation = rotationToAlign;

                // Afficher les informations de l'objet touché et de la normale de la face touchée
                Debug.Log("Objet touché : " + objectHit.name);
                Debug.Log("Normale de la face touchée : " + hitNormal);
            }
        }
    }
}
