using Unity.VisualScripting;
using UnityEngine;

public class TriggerObject : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;
    private bool isGrounded = true;

    void Start()
    {
        // R�cup�rer le Rigidbody attach� � l'objet
        rb = GetComponent<Rigidbody>();

        // R�cup�rer le Collider attach� � l'objet
        col = GetComponent<Collider>();

        // D�sactiver la gravit� initialement
        rb.useGravity = false;

        // D�sactiver l'option "Is Trigger" initialement
        col.isTrigger = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        // V�rifier si l'objet entre en collision avec le sol
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        else
        {
            // Si l'objet entre en collision avec un autre objet
            // d�sactivez le Rigidbody et activez l'option "Is Trigger" du Collider
            rb.useGravity = false;
            col.isTrigger = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // V�rifier si l'objet quitte le sol
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
        else
        {
            // Si l'objet sort de la collision avec un autre objet
            // r�activez le Rigidbody et d�sactivez l'option "Is Trigger" du Collider
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
Ce script g�re la physique d'un objet de sorte qu'il reste au sol lorsqu'il est en collision avec le sol, et active 
l'option "Is Trigger" du Collider lorsque l'objet entre en collision avec un autre objet, tout en d�sactivant le Rigidbody.


Voici le fonctionnement d�taill� du script :

Au d�marrage(Start), le script r�cup�re les composants Rigidbody et Collider attach�s � l'objet, puis d�sactive la gravit�
(useGravity = false) et d�sactive l'option "Is Trigger" (isTrigger = false) du Collider.

Lorsque l'objet entre en collision avec un autre objet (OnCollisionEnter), le script v�rifie si l'objet est en collision 
avec le sol.
Si oui, il met isGrounded � vrai. Sinon, il d�sactive la gravit� du Rigidbody et active l'option "Is Trigger" du Collider.
Lorsque l'objet quitte la collision avec un autre objet (OnCollisionExit), le script v�rifie si l'objet quitte le sol.
Si oui, il met isGrounded � faux. Sinon, il r�active la gravit� du Rigidbody et d�sactive l'option "Is Trigger" du Collider.
Dans la fonction FixedUpdate, si l'objet est au sol et que l'option "Is Trigger" n'est pas activ�e, une force est appliqu�e 
vers le bas (Vector3.down) pour maintenir l'objet au sol en utilisant la gravit� (9.81f est la gravit� terrestre standard),
en mode acc�l�ration (ForceMode.Acceleration).
Assurez-vous de remplacer "Ground" par le tag appropri� que vous avez attribu� � vos objets de sol dans votre sc�ne Unity.

*/