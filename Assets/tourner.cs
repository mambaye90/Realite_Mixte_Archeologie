using UnityEngine;

public class FaceIdentifier3 : MonoBehaviour
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

                // Calculer l'angle d'alignement avec Vector3.down
                float angleToAlignWithDown = Vector3.Angle(hitNormal, Vector3.down);

                // Calculer la rotation n�cessaire pour aligner la normale avec Vector3.down
                Quaternion rotationToAlignWithDown = Quaternion.AngleAxis(angleToAlignWithDown, Vector3.Cross(hitNormal, Vector3.down));

                // Appliquer la rotation � l'objet touch�
                objectHit.transform.rotation = rotationToAlignWithDown;

                // Afficher les informations de l'objet touch�, la normale de la face touch�e et l'angle d'alignement avec le sol
                Debug.Log("Objet touch� : " + objectHit.name);
                Debug.Log("Normale de la face touch�e : " + hitNormal);
                Debug.Log("Angle d'alignement avec le bas : " + angleToAlignWithDown + " degr�s");
            }
        }
    }
}
