using UnityEngine;

public class LandingPad : TrackedObjective
{
    public override void EnterCollider()
    {
        Debug.Log("Drone entered");
        Drone.Instance.EnterLandingZone();
    }
    public override void ExitCollider()
    {
        Debug.Log("Drone exited");
        Drone.Instance.ExitLandingZone();
    }
}
