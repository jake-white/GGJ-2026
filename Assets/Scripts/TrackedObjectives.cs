using UnityEngine;

public class TrackedObjective : MonoBehaviour
{
    public virtual void FlyThrough() { }
    public virtual void EnterCollider() { }
    public virtual void ExitCollider() { }
}