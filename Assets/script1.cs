using UnityEngine;
public class Rotate90 : MonoBehaviour
{
    Quaternion goal;
 
    void Start()
    {
        goal=transform.rotation;
    }
 
    void Update()
    {
        if (Quaternion.Dot(transform.rotation,goal)>0.9999f)
        {
            transform.rotation=goal;
            Vector3 d=new Vector3(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"),0);
            goal=Quaternion.Euler(90*-d.y,90*d.x,0)*transform.rotation;
        }
        else
            transform.rotation=Quaternion.Lerp(transform.rotation,goal,10*Time.deltaTime);
    }
}