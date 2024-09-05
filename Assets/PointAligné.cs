using UnityEngine;

public class FaceAlignmentChecker : MonoBehaviour
{
    public GameObject cube1;
    public GameObject cube2;

    public float alignmentThreshold = 0.1f;

    void Update()
    {
        // Vérifier si les deux faces touchées des cubes sont alignées
        bool areFacesAligned = CheckTouchingFacesAlignment();

        // Afficher le résultat
        if (areFacesAligned)
        {
            Debug.Log("Les deux faces touchées des cubes sont alignées.");
        }
        else
        {
            Debug.Log("Les deux faces touchées des cubes ne sont pas alignées.");
        }
    }

    bool CheckTouchingFacesAlignment()
    {
        // Obtenir les collisions entre les deux cubes
        Collider[] colliders1 = cube1.GetComponentsInChildren<Collider>();
        Collider[] colliders2 = cube2.GetComponentsInChildren<Collider>();

        // Vérifier les collisions entre les deux cubes
        foreach (Collider collider1 in colliders1)
        {
            foreach (Collider collider2 in colliders2)
            {
                if (collider1.bounds.Intersects(collider2.bounds))
                {
                    // Les deux cubes sont en collision, vérifions si les faces touchées sont alignées
                    if (AreTouchingFacesAligned(collider1, collider2))
                    {
                        return true;
                    }
                }
            }
        }

        // Aucune collision ou aucune face alignée détectée
        return false;
    }

    bool AreTouchingFacesAligned(Collider collider1, Collider collider2)
    {
        // Vérifier si les faces touchées des deux cubes sont alignées
        // Implémentez cette méthode en fonction de vos besoins et de la géométrie de vos cubes
        // Vous pouvez utiliser des méthodes similaires à celles utilisées dans la réponse précédente pour comparer les positions des sommets des faces
        // Assurez-vous de considérer les orientations des cubes pour déterminer les faces touchées et leurs positions relatives
        return false; // Remplacez cela par votre logique d'alignement
    }
}
