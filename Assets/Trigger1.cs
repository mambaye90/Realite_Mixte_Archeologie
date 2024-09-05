using Unity.VisualScripting;
using UnityEngine;

public class TriggerObject : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;
    private bool isGrounded = true;

    void Start()
    {
        // Récupérer le Rigidbody attaché à l'objet
        rb = GetComponent<Rigidbody>();

        // Récupérer le Collider attaché à l'objet
        col = GetComponent<Collider>();

        // Désactiver la gravité initialement
        rb.useGravity = false;

        // Désactiver l'option "Is Trigger" initialement
        col.isTrigger = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Vérifier si l'objet entre en collision avec le sol
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        else
        {
            // Si l'objet entre en collision avec un autre objet
            // désactivez le Rigidbody et activez l'option "Is Trigger" du Collider
            rb.useGravity = false;
            col.isTrigger = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Vérifier si l'objet quitte le sol
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
        else
        {
            // Si l'objet sort de la collision avec un autre objet
            // réactivez le Rigidbody et désactivez l'option "Is Trigger" du Collider
            rb.useGravity = true;
            col.isTrigger = false;
        }
    }

    void FixedUpdate()
    {
        // Appliquer une force vers le bas pour garder l'objet au sol s'il est au sol
        if (isGrounded && !col.isTrigger)
        {
            rb.AddForce(Vector3.down * 9.81f, ForceMode.Acceleration);
        }
    }
}
/*
Ce script gère la physique d'un objet de sorte qu'il reste au sol lorsqu'il est en collision avec le sol, et active 
l'option "Is Trigger" du Collider lorsque l'objet entre en collision avec un autre objet, tout en désactivant le Rigidbody.


Voici le fonctionnement détaillé du script :

Au démarrage(Start), le script récupère les composants Rigidbody et Collider attachés à l'objet, puis désactive la gravité
(useGravity = false) et désactive l'option "Is Trigger" (isTrigger = false) du Collider.

Lorsque l'objet entre en collision avec un autre objet (OnCollisionEnter), le script vérifie si l'objet est en collision 
avec le sol.
Si oui, il met isGrounded à vrai. Sinon, il désactive la gravité du Rigidbody et active l'option "Is Trigger" du Collider.
Lorsque l'objet quitte la collision avec un autre objet (OnCollisionExit), le script vérifie si l'objet quitte le sol.
Si oui, il met isGrounded à faux. Sinon, il réactive la gravité du Rigidbody et désactive l'option "Is Trigger" du Collider.
Dans la fonction FixedUpdate, si l'objet est au sol et que l'option "Is Trigger" n'est pas activée, une force est appliquée 
vers le bas (Vector3.down) pour maintenir l'objet au sol en utilisant la gravité (9.81f est la gravité terrestre standard),
en mode accélération (ForceMode.Acceleration).
Assurez-vous de remplacer "Ground" par le tag approprié que vous avez attribué à vos objets de sol dans votre scène Unity.

*/