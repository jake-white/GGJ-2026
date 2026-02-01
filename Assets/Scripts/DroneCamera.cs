using UnityEngine;

public class DroneCamera : MonoBehaviour
{
    public Transform turretFollowPoint, orbitFollowPoint;
    public Transform droneCenter;
    public bool orbiting = false;
    public bool followRotation = true;

    private Transform followPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(orbiting)
        {
            followPoint = orbitFollowPoint;
            transform.LookAt(droneCenter);
        }
        else
        {
            followPoint = turretFollowPoint;
            if(followRotation) transform.forward = turretFollowPoint.forward;

        }
        transform.position = Vector3.Lerp(transform.position, followPoint.position, 0.75f);
    }
}
