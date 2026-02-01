using UnityEngine;

public class TrackedHoop : TrackedObjective
{
    public TrackedCrater crater;
    private bool flownThrough = false;

    public override void FlyThrough()
    {
        if (!flownThrough)
        {
            base.FlyThrough();
            crater.EnableBombing();
            Debug.Log("Flew through hoop!");
            flownThrough = true;
        }
    }

    public override void EnterCollider()
    {
        base.EnterCollider();
        FlyThrough();
    }
}
