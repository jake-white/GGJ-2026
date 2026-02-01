using CollabXR;
using UnityEngine;

public class Mothership : SingletonBehavior<Mothership>
{
    public Collider xAxisA, xAxisB, yAxisA, yAxisB, zAxisA, zAxisB;
    public enum MotherState { Shielded, ShieldsBroken, Dead }
    public enum MothershipAxis { None, X, Y, Z }
    public MotherState state;

    private MothershipAxis nextExpectedAxis = MothershipAxis.None;
    private MothershipAxisCollider justFlownThrough = null;

    private bool xComplete, yComplete, zComplete;

    private void Start()
    {
        ArduinoSend.Instance.TriggerMothershipMode(1);
    }

    public void BreakShields()
    {
        state = MotherState.ShieldsBroken;
        ArduinoSend.Instance.TriggerMothershipMode(2);
    }

    public void FlyThroughAxis(MothershipAxisCollider collider)
    {
        if (state == MotherState.ShieldsBroken)
        {
            if (collider.axis != nextExpectedAxis)
            {
                justFlownThrough = null;
            }
            else if(collider.axis == nextExpectedAxis)
            {
                if(justFlownThrough == null)
                {
                    justFlownThrough = collider;
                }
                else if(justFlownThrough != collider)
                {
                    CompleteAxis(collider.axis);
                    ResetExpectedAxis();
                }
            }
        }
    }

    public void AxisDestroyed(MothershipAxis axis)
    {
        nextExpectedAxis = axis;
    }

    public void ResetExpectedAxis()
    {
        nextExpectedAxis = MothershipAxis.None;
    }

    public void CompleteAxis(MothershipAxis axis)
    {
        switch(axis)
        {
            case MothershipAxis.X:
                xComplete = true;
                xAxisA.enabled = false;
                xAxisB.enabled = false;
                break;
            case MothershipAxis.Y:
                yComplete = true;
                yAxisA.enabled = false;
                yAxisB.enabled = false;
                break;
            case MothershipAxis.Z:
                zComplete = true;
                zAxisA.enabled = false;
                zAxisB.enabled = false;
                break;
        }

        if (xComplete && yComplete && zComplete)
        {
            state = MotherState.Dead;
            SequenceManager.Instance.MothershipDestroyed();
        }
    }
}
