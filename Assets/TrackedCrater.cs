using UnityEngine;

public class TrackedCrater : TrackedObjective
{
    public enum CraterState { Shielded, Ready, Destroyed };
    public CraterState state;
    public GameObject building;
    public LineRenderer line;
    public MeshRenderer shield;

    private void Start()
    {
        line.enabled = true;
    }

    private void Update()
    {
        shield.enabled = state == CraterState.Shielded;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, Mothership.Instance.transform.position);
    }

    public void EnableBombing()
    {
        if (state == CraterState.Shielded) state = CraterState.Ready;
        ToggleLight(true);
    }

    public void ToggleLight(bool enabled)
    {
        ArduinoSend.Instance.ToggleHoop(body.RigidBodyId - OptitrackConstellationManager.Instance.HoopIdOffset, enabled);
    }

    public void GetBombed()
    {
        if (state == CraterState.Ready)
        {
            line.enabled = false;
            state = CraterState.Destroyed;
            building.SetActive(false);
            SequenceManager.Instance.CraterDestroyed(this);
            ToggleLight(false);
        }
    }
}