using UnityEngine;

public class CameraFaceTracker : MonoBehaviour
{
    public Transform target; // La face à suivre par la caméra

    void Update()
    {
        if (target != null)
        {
            // Faire en sorte que la caméra regarde toujours vers la face cible
            transform.LookAt(target);
        }
    }
}
