using UnityEngine;

public class ShootableObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Laser")
        {
            Instantiate(FXManager.Instance.explosion, transform.position, Quaternion.identity);
            GameObject.Destroy(gameObject);
        }
    }
}
