using UnityEngine;
using UnityEngine.UI;

namespace MyNamespace
{
    public class CreateCanvasAndCameraOnSelectedFace : MonoBehaviour
    {
        public GameObject canvasPrefab1;
        public GameObject cameraPrefab1;
        public GameObject objectToDetect1;

        public GameObject canvasPrefab2;
        public GameObject cameraPrefab2;
        public GameObject objectToDetect2;

        public float distanceFromFace = 1f;

        public Button resultButton;

        private GameObject cameraObject1;

        void Start()
        {
            if (resultButton != null)
            {
                resultButton.gameObject.SetActive(false);
            }
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo))
                {
                    GameObject objectHit = hitInfo.collider.gameObject;

                    if (objectHit == objectToDetect1)
                    {
                        CreateCanvasAndCamera(hitInfo, canvasPrefab1, cameraPrefab1, ref cameraObject1);
                    }
                    else if (objectHit == objectToDetect2)
                    {
                        GameObject cameraObject2 = null;
                        CreateCanvasAndCamera(hitInfo, canvasPrefab2, cameraPrefab2, ref cameraObject2);

                        if (cameraObject1 != null)
                        {
                            bool normalsMatch = Vector3.Dot(cameraObject1.transform.forward, cameraObject2.transform.forward) > 0.99f;

                            if (normalsMatch)
                            {
                                DisplayResult("Les deux faces s'emboîtent !");
                            }
                            else
                            {
                                DisplayResult("Les deux faces ne s'emboîtent pas !");
                            }
                        }
                    }
                }
            }
        }

        // Méthode pour créer le canvas et la caméra
        private void CreateCanvasAndCamera(RaycastHit hitInfo, GameObject canvasPrefab, GameObject cameraPrefab, ref GameObject cameraObject)
        {
            GameObject canvasObject = Instantiate(canvasPrefab, hitInfo.point, Quaternion.identity);
            Vector3 cameraPosition = hitInfo.point + hitInfo.normal * distanceFromFace;
            cameraObject = Instantiate(cameraPrefab, cameraPosition, Quaternion.identity);
            cameraObject.transform.LookAt(hitInfo.point + hitInfo.normal);
        }

        // Méthode pour afficher le résultat dans le bouton
        private void DisplayResult(string message)
        {
            if (resultButton != null)
            {
                resultButton.gameObject.SetActive(true);
                Text buttonText = resultButton.GetComponentInChildren<Text>();
                if (buttonText != null)
                {
                    buttonText.text = message;
                }
            }
        }
    }
}

