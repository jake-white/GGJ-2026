using UnityEngine;

public class DroneCamera : MonoBehaviour
{
    public Transform followPoint;
    public Transform droneCenter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(followPoint != null)
        {
            transform.position = Vector3.Lerp(transform.position, followPoint.position, 0.75f);
            transform.LookAt(droneCenter);
        }
    }
}
