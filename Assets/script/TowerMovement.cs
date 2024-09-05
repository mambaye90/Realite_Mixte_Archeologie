using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMovement : MonoBehaviour
{
    //Référence vers le composant Rigidbody du modèle 3D présent dans la scène
    private Rigidbody rb;
    //Variable d'édition permettant de modifier la vitesse de déplacement et de saut
    public float force;

    
    //Variables permettant de gérer les mouvements horizontaux et verticaux
    private float movHorizontal;
    private float movVertical;
    
    //La fonction Start s'effectue une seule fois, lors du démarrage de la scène
    void Start()
    {
        //On renseigne le composant Rigidbody du modèle 3D dans la variable rb
        rb = this.GetComponent<Rigidbody>();
    }

    // La fonction Update est appelée une fois par frame 
    void Update()
    {
        //La fonction GetAxisRaw permet d'obtenir une valeur entre -1 et 1 correspondant aux appuis sur les axes horizontaux
        //(flèches gauche - droite) et verticaux (flèches haut - bas)
        movHorizontal = Input.GetAxisRaw("Horizontal") * force;
        movVertical = Input.GetAxisRaw("Vertical") * force;
        //On multiplie la valeur obtenue par la variable force pour modifier la vitesse de déplacement

        //Si l'utilisateur appuie sur la touche "Espace", on demande à l'objet de sauter
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Vector3.up représente le vecteur (0, 1, 0) et désigne donc une orientation verticale
            //On effectue le saut en appliquant une force vers le haut au rigidbody
            rb.AddForce(Vector3.up * force, ForceMode.Impulse); 
        }
    }

    
    //La fonction FixedUpdate est quasiment similaire à la fonction Update ; 
    //Elle est appelée un nombre de fois fixe par seconde, alors que la fonction Update se lance autant de fois que possible.
    //Pour tenir compte des différences de puissance de calcul entre les ordinateurs, on préfèrera renseigner les 
    //déplacements dans la fonction FixedUpdate
    void FixedUpdate()
    {
        //On applique une force au modèle 3D correspondant aux axes recueillis dans le Update
        rb.AddForce(movHorizontal, 0f, movVertical, ForceMode.Impulse);
    }

    //La fonction "OnTriggerEnter" permet de renseigner les actions à entreprendre lorsque l'objet rentre en collision 
    //avec un autre collider dont le "isTrigger" est coché
    void OnTriggerEnter(Collider other)
    {
        //Si le collider (appelé ici "other") de l'autre objet est touché et que son tag est "Piece",
        //on effectue l'action suivante : 
        Debug.Log("Collision détectée avec : " + other.gameObject.name);

        if (other.CompareTag("Piece"))
        {
            //On détruit le gameObject associé à ce collider
            Destroy(other.gameObject);
            
        }
       
    }
    void OnCollisionStay(Collision collision)
    {
        // Vérifie si l'objet est en contact avec l'objet plan
        foreach (ContactPoint contact in collision.contacts)
        {
            if (collision.gameObject.CompareTag("Plane")) // Vérifie si l'objet avec lequel nous sommes en collision est l'objet plan
            {
                // Réajuste la position verticale de l'objet pour rester en contact avec l'objet plan
                // Vous pouvez ajuster la position verticale selon les besoins de votre scène
                transform.position = new Vector3(transform.position.x, contact.point.y + GetComponent<Collider>().bounds.extents.y, transform.position.z);
            }
        }
    }

}
/*
  
Ce script permet à un objet de se déplacer horizontalement et verticalement en réponse aux entrées de l'utilisateur 
(touches fléchées) et de sauter lorsque la touche "Espace" est enfoncée. De plus, il détruit les objets avec le tag "Piece"
lorsqu'il entre en collision avec eux. Il assure également que l'objet reste au sol en ajustant sa position verticale
lorsqu'il est en contact avec un objet plan.

Voici une explication détaillée du script :

La variable rb est une référence au composant Rigidbody attaché à l'objet.
La variable force est une variable d'édition qui permet de contrôler la vitesse de déplacement et de saut de l'objet.
Les variables movHorizontal et movVertical sont utilisées pour stocker les mouvements horizontaux et verticaux calculés
à partir des entrées de l'utilisateur.
Dans la fonction Start(), le composant Rigidbody de l'objet est récupéré et stocké dans la variable rb.
Dans la fonction Update(), les mouvements horizontaux et verticaux sont mis à jour en fonction des entrées de l'utilisateur
à l'aide de Input.GetAxisRaw(). Si la touche "Espace" est enfoncée, une force vers le haut est appliquée au Rigidbody 
pour simuler un saut.
Dans la fonction FixedUpdate(), les forces de déplacement sont appliquées au Rigidbody à l'aide de rb.AddForce() pour 
assurer un mouvement fluide et cohérent. Cette fonction est appelée à une fréquence fixe par seconde.
La fonction OnTriggerEnter(Collider other) est appelée lorsque l'objet entre en collision avec un autre objet muni
d'un Collider avec l'option "Is Trigger" activée. Si l'autre objet a le tag "Piece", il est détruit à l'aide 
de Destroy(other.gameObject).
La fonction OnCollisionStay(Collision collision) est appelée lorsqu'un objet est en contact avec un autre objet. 
Elle vérifie si l'objet est en contact avec un objet plan (avec le tag "Plane") et ajuste sa position verticale pour rester
au sol en utilisant les points de contact de la collision.
Ce script assure le mouvement et la physique de l'objet de manière interactive en réponse aux entrées de l'utilisateur 
et aux collisions avec d'autres objets.


*/


