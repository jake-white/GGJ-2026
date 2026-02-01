using UnityEngine;

public class Bomb : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "BombCollider")
        {
            TrackedCrater crater = collision.collider.GetComponentInParent<TrackedCrater>();
            if(crater != null)
            {
                crater.GetBombed();
            }
        }
        Instantiate(FXManager.Instance.explosion, transform.position, Quaternion.identity);
        GameObject.Destroy(gameObject);
    }
}
