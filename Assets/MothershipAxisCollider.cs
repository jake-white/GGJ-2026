using UnityEngine;

public class MothershipAxisCollider : TrackedObjective
{
    public Mothership.MothershipAxis axis;
    public bool isA;
    private int hp = 5;
    private Collider coll;

    private void Awake()
    {
        coll = GetComponent<Collider>();
    }

    public override void FlyThrough()
    {
        Mothership.Instance.FlyThroughAxis(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Laser" && hp > 0)
        {
            Instantiate(FXManager.Instance.explosion, transform.position, Quaternion.identity);
            hp--;
            if(hp <= 0)
            {
                coll.isTrigger = true;
            }
        }
    }
}
