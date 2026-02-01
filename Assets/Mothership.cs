using System.Collections.Generic;
using UnityEngine;

public class Mothership : SingletonBehavior<Mothership>
{
    public List<MothershipAxisCollider> axisColliders;
    public enum MotherState { Shielded, ShieldsBroken, Dead }
    public enum MothershipAxis { None, X, Y, Z }
    public MotherState state;

    public Material shieldHealthy, shieldDamaged;

    private MothershipAxis nextExpectedAxis = MothershipAxis.None;
    private MothershipAxisCollider justFlownThrough = null;

    private bool xComplete, yComplete, zComplete;

    private void Start()
    {
        ArduinoSend.Instance.TriggerMothershipMode(0);
    }

    public void BreakShields()
    {
        state = MotherState.ShieldsBroken;
        foreach(MothershipAxisCollider c in axisColliders)
        {
            c.BecomeVulnerable();
        }

        ArduinoSend.Instance.TriggerMothershipMode(1);
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

    public void AxisDisabled(MothershipAxis axis)
    {
        if (nextExpectedAxis == MothershipAxis.None)
        {
            nextExpectedAxis = axis;
            switch (axis)
            {
                case MothershipAxis.X:
                    ArduinoSend.Instance.TriggerMothershipMode(2);
                    break;
                case MothershipAxis.Y:
                    ArduinoSend.Instance.TriggerMothershipMode(3);
                    break;
                case MothershipAxis.Z:
                    ArduinoSend.Instance.TriggerMothershipMode(4);
                    break;
            }

            foreach (MothershipAxisCollider c in axisColliders)
            {
                c.SetTrigger(true);
                if (c.axis != axis)
                {
                    c.RestoreHP();
                }
            }
        }
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
                axisColliders[0].DestroyAxis();
                axisColliders[1].DestroyAxis();
                break;
            case MothershipAxis.Y:
                yComplete = true;
                axisColliders[2].DestroyAxis();
                axisColliders[3].DestroyAxis();
                break;
            case MothershipAxis.Z:
                zComplete = true;
                axisColliders[4].DestroyAxis();
                axisColliders[5].DestroyAxis();
                break;
        }

        if (xComplete && yComplete && zComplete)
        {
            state = MotherState.Dead;
            SequenceManager.Instance.MothershipDestroyed();
            ArduinoSend.Instance.TriggerMothershipMode(5);
            AudioManager.Instance.QueueCommanderVoice(0);
        }
        else
        {
            BreakShields();
        }
    }
}
