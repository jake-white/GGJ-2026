using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class DroneTurret : MonoBehaviour
{
    public LineRenderer leftLaser, rightLaser;
    public Transform leftLaserOrigin, rightLaserOrigin;
    private Ray currentAim;

    bool leftFiring, rightFiring;
    float lastLeftFire, lastRightFire;

    private void Update()
    {
        FireRoutine(true, out lastLeftFire);
        FireRoutine(false, out lastRightFire);
    }

    public void Aim(Ray r)
    {
        currentAim = r;
        Debug.DrawRay(currentAim.origin, currentAim.direction * 100, Color.red);
        leftLaser.SetPosition(0, leftLaserOrigin.position);
        leftLaser.SetPosition(1, currentAim.origin + currentAim.direction * 10);

        rightLaser.SetPosition(0, rightLaserOrigin.position);
        rightLaser.SetPosition(1, currentAim.origin + currentAim.direction * 10);
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
        LineRenderer laser = left ? leftLaser : rightLaser;

        bool fireNow = false;
        float timeSince = Time.time - lastFire;

        if (timeSince > 0.2)
        {
            laser.enabled = false;
        }

        if (isFiring)
        {
            if (timeSince > 0.5)
            {
                fireNow = true;
            }
        }

        newLastFire = lastFire;
        if (fireNow)
        {
            Debug.Log("Fire now!");
            laser.enabled = true;
            newLastFire = Time.time;
        }
    }
}
