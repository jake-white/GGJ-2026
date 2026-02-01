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
            ToggleLight(false);
        }
    }

    public void ToggleLight(bool enabled)
    {
        ArduinoSend.Instance.ToggleHoop(body.RigidBodyId - OptitrackConstellationManager.Instance.HoopIdOffset, enabled);
    }

    public override void EnterCollider()
    {
        base.EnterCollider();
        FlyThrough();
    }
}
