using UnityEngine;

public class LandingPad : TrackedObjective
{
    public override void EnterCollider()
    {
        Drone.Instance.EnterLandingZone();
    }
    public override void ExitCollider()
    {
        Drone.Instance.ExitLandingZone();
    }
}
