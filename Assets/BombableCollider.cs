using System.Security.AccessControl;
using UnityEngine;

public class BombableCollider : MonoBehaviour
{
    public TrackedCrater crater;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BombCollider")
        {
            crater.GetBombed();
        }
    }
}
