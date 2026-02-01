using CollabXR;
using UnityEngine;

public class Drone : SingletonBehavior<Drone>
{
    public LayerMask trackedObjectives;
    public enum FlightState { Landed, Hovering, Flying }
    public FlightState state;
    Vector3 lastPosition, currentPosition;
    float averageDistanceMovedRecently;
    bool inLandingZone;
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
            Debug.Log(potentialObjective.name);
            potentialObjective.FlyThrough();
        }
        float distanceMovedThisFrame = Vector3.Distance(lastPosition, currentPosition)/Time.deltaTime;
        averageDistanceMovedRecently = Mathf.Lerp(averageDistanceMovedRecently, distanceMovedThisFrame, 0.25f);
        if (inLandingZone && IsMostlyStationary())
        {
            state = FlightState.Landed;
        }
         if(state == FlightState.Landed && !IsMostlyStationary())
        {
            state = inLandingZone ? FlightState.Hovering : FlightState.Flying;
        }
         if(state == FlightState.Hovering && !inLandingZone)
        {
            state = FlightState.Flying;
        }
    }

    public void EnterLandingZone()
    {
        inLandingZone = true;
        state = FlightState.Hovering;
    }

    public void ExitLandingZone()
    {
        inLandingZone = false;
        state = FlightState.Flying;
    }

    public bool IsMostlyStationary()
    {
        return averageDistanceMovedRecently < 0.1;
    }

    public TrackedObjective ObjectiveIntersected()
    {
        Ray path = new Ray(currentPosition, lastPosition - currentPosition);
        Debug.DrawRay(path.origin, path.direction);
        RaycastHit hit;
        if(Physics.Raycast(path, out hit, Vector3.Distance(currentPosition, lastPosition), trackedObjectives))
        {
            TrackedObjective obj = hit.collider.GetComponentInParent<TrackedObjective>();
            return obj;
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

    public Vector3 GetApproximateVelocity()
    {
        return currentPosition - lastPosition;
    }
}
