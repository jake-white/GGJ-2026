using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class DroneTurret : MonoBehaviour
{
    public Transform leftLaserOrigin, rightLaserOrigin, bombOrigin;
    public GameObject laserPrefab, bombPrefab;
    public float projectileSpeed = 10.0f;
    public float projectileInterval = 0.5f, bombingInterval = 2.0f;
    private Ray currentAim;

    bool leftFiring, rightFiring;
    float lastLeftFire, lastRightFire;
    float lastBombing;

    private void Update()
    {
        FireRoutine(true, out lastLeftFire);
        FireRoutine(false, out lastRightFire);
    }

    public void Aim(Ray r)
    {
        currentAim = r;
        Debug.DrawRay(currentAim.origin, currentAim.direction * 100, Color.red);
    }

    public void ActivateTurret(InteractorHandedness handedness)
    {
        Debug.Log($"Activated {handedness}");
        if (handedness == InteractorHandedness.Left)
        {
            leftFiring = true;
        }
        else if (handedness == InteractorHandedness.Right)
        {
            rightFiring = true;
        }
    }

    public void DeactivateTurret(InteractorHandedness handedness)
    {
        Debug.Log($"Deactivated {handedness}");
        if (handedness == InteractorHandedness.Left)
        {
            leftFiring = false;
        }
        else if (handedness == InteractorHandedness.Right)
        {
            rightFiring = false;
        }
    }

    public void FireRoutine(bool left, out float newLastFire)
    {
        bool isFiring = left ? leftFiring : rightFiring;
        float lastFire = left ? lastLeftFire : lastRightFire;
        Transform origin = left ? leftLaserOrigin : rightLaserOrigin;

        bool fireNow = false;
        float timeSince = Time.time - lastFire;

        if (isFiring)
        {
            if (timeSince > projectileInterval)
            {
                fireNow = true;
            }
        }

        newLastFire = lastFire;
        if (fireNow)
        {
            GameObject newProjectile = Instantiate(laserPrefab);
            newProjectile.transform.position = origin.position;
            newProjectile.transform.forward = currentAim.direction;
            newProjectile.GetComponent<Rigidbody>().linearVelocity = currentAim.direction * projectileSpeed;
            newLastFire = Time.time;
        }
    }

    public void DropBomb()
    {
        if(Time.time - lastBombing > bombingInterval)
        {
            GameObject newBomb = Instantiate(bombPrefab);
            newBomb.transform.position = bombOrigin.position;
            lastBombing = Time.time;
        }
    }
}
