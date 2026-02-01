using UnityEngine;

public class TrackedObjective : MonoBehaviour
{
    protected OptitrackRigidBody body;
    private void Awake()
    {
        body = GetComponent<OptitrackRigidBody>();
    }
    public virtual void FlyThrough() { }
    public virtual void EnterCollider() { }
    public virtual void ExitCollider() { }
}