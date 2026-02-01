using UnityEngine;

public class MothershipAxisCollider : TrackedObjective
{
    public Mothership.MothershipAxis axis;
    public MothershipAxisCollider otherSide;
    public MeshRenderer visual;
    public bool isA;
    private int hp = 5;
    private Collider coll;
    private bool vulnerable = false;

    private void Start()
    {
        coll = GetComponent<Collider>();
        visual.material = Mothership.Instance.shieldHealthy;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Hit by {other.name}");
        if (vulnerable && other.tag == "Laser" && hp > 0)
        {
            Instantiate(FXManager.Instance.explosion, transform.position, Quaternion.identity);
            GameObject.Destroy(other.gameObject);
            TakeDamage();
            otherSide.TakeDamage();
        }

        if(other.tag == "DroneCollider")
        {
            Mothership.Instance.FlyThroughAxis(this);
        }
    }

    public void TakeDamage()
    {
        hp--;
        visual.material = Mothership.Instance.shieldDamaged;

        if (hp <= 0)
        {
            DisableAxis();
        }
    }

    public void BecomeVulnerable()
    {
        vulnerable = true;
        SetTrigger(false);
    }

    public void SetTrigger(bool trigger)
    {
        coll.isTrigger = trigger;
    }
    public void DisableAxis()
    {
        hp = 0;
        otherSide.hp = 0;
        vulnerable = false;
        otherSide.vulnerable = false;
        visual.enabled = false;
        otherSide.visual.enabled = false;
        Mothership.Instance.AxisDisabled(axis);
    }

    public void DestroyAxis()
    {
        gameObject.SetActive(false);
        vulnerable = false;
        coll.enabled = false;
    }

    public void RestoreHP()
    {
        Debug.Log("Restoring HP");
        hp = 5;
        vulnerable = false;
        visual.enabled = true;
        visual.material = Mothership.Instance.shieldHealthy;
    }
}
