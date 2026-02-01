using UnityEngine;

public class TrackedCrater : TrackedObjective
{
    public enum CraterState { Waiting, Ready, Destroyed };
    public CraterState state;
    public GameObject building;

    public void EnableBombing()
    {
        if (state == CraterState.Waiting) state = CraterState.Ready;
    }
    public void GetBombed()
    {
        if (state == CraterState.Ready)
        {
            state = CraterState.Destroyed;
            building.SetActive(false);
            SequenceManager.Instance.CraterDestroyed(this);
        }
    }
}