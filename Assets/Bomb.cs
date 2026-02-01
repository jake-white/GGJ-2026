using UnityEngine;

public class Bomb : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.gameObject);
        if(collision.collider.tag == "BombCollider")
        {
            Debug.Log("bomb collider detected");
            TrackedCrater crater = collision.collider.GetComponentInParent<TrackedCrater>();
            if(crater != null)
            {
                crater.GetBombed();
            }
        }
        Instantiate(FXManager.Instance.smallExplosion, transform.position, Quaternion.identity);
        GameObject.Destroy(gameObject);
    }
}
