using UnityEngine;

public class FaceIdentifier1 : MonoBehaviour
{
    void Update()
    {
        // V�rifier si le bouton de la souris est enfonc�
        if (Input.GetMouseButtonDown(0))
        {
            // Lancer un rayon depuis la cam�ra vers le pointeur de la souris
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            // Effectuer le raycasting et v�rifier s'il y a une collision
            if (Physics.Raycast(ray, out hitInfo))
            {
                // R�cup�rer l'objet touch�
                GameObject objectHit = hitInfo.collider.gameObject;

                // R�cup�rer la normale de la face touch�e
                Vector3 hitNormal = hitInfo.normal;

                // Calculer la rotation n�cessaire pour aligner la normale avec un vecteur directionnel sp�cifique
                Quaternion rotationToAlign = Quaternion.LookRotation(hitNormal, Vector3.up);

                // Appliquer la rotation � l'objet touch�
                objectHit.transform.rotation = rotationToAlign;

                // Afficher les informations de l'objet touch� et de la normale de la face touch�e
                Debug.Log("Objet touch� : " + objectHit.name);
                Debug.Log("Normale de la face touch�e : " + hitNormal);
            }
        }
    }
}
