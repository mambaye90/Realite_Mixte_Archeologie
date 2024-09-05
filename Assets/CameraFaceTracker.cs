using UnityEngine;

public class CameraFaceTracker : MonoBehaviour
{
    public Transform target; // La face � suivre par la cam�ra

    void Update()
    {
        if (target != null)
        {
            // Faire en sorte que la cam�ra regarde toujours vers la face cible
            transform.LookAt(target);
        }
    }
}
