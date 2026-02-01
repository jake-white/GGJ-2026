using CollabXR;
using UnityEngine;

public class Drone : SingletonBehavior<Drone>
{
    public LayerMask trackedObjectives;
    public enum FlightState { Landed, InLandingZone, Flying }
    public FlightState state;
    Vector3 lastPosition, currentPosition;
    float averageDistanceMovedRecently;
    void Start()
    {
        lastPosition = transform.position;
        currentPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        lastPosition = currentPosition;
        currentPosition = transform.position;
        TrackedObjective potentialObjective = ObjectiveIntersected();
        if (potentialObjective != null)
        {
            potentialObjective.FlyThrough();
        }
        float distanceMovedThisFrame = Vector3.Distance(lastPosition, currentPosition)/Time.deltaTime;
        averageDistanceMovedRecently = Mathf.Lerp(averageDistanceMovedRecently, distanceMovedThisFrame, 0.25f);
        if (state == FlightState.InLandingZone && IsMostlyStationary())
        {
            state = FlightState.Landed;
        }
        else if(state == FlightState.Landed && !IsMostlyStationary())
        {
            state = FlightState.Flying;
        }
    }

    public void EnterLandingZone()
    {
        state = FlightState.InLandingZone;
    }

    public void ExitLandingZone()
    {
        state = FlightState.Flying;
    }

    public bool IsMostlyStationary()
    {
        return averageDistanceMovedRecently < 0.1;
    }

    public TrackedObjective ObjectiveIntersected()
    {
        Ray path = new Ray(currentPosition, lastPosition - currentPosition);
        Debug.DrawRay(path.origin, path.direction * 100);
        RaycastHit hit;
        if(Physics.Raycast(path, out hit, 1000, trackedObjectives))
        {
            return hit.collider.GetComponentInParent<TrackedObjective>();
        }
        return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "ObjectiveCollider")
        {
            other.GetComponentInParent<TrackedObjective>().EnterCollider();
        }
    }
}
