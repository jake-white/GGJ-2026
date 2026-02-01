using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(CabinetControls.Instance.GetCurrentDroneCamera().transform.position);
    }
}
