using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class CabinetControls : MonoBehaviour
{
    public Camera droneCam;
    public LineRenderer gunLaser, droneLaser;
    public RectTransform reticle;

    public Canvas screen;
    public RenderTexture tex;

    private Collider screenCollider;
    private RectTransform screenSpace;

    private Vector2 screenSize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        screenCollider = screen.GetComponent<Collider>();
        screenSpace = screen.gameObject.GetComponent<RectTransform>();
        screenSize = new Vector2(screenSpace.rect.width, screenSpace.rect.height);
    }

    // Update is called once per frame
    void Update()
    {
        gunLaser.SetPosition(0, gunLaser.transform.position);
        gunLaser.SetPosition(1, gunLaser.transform.position + (gunLaser.transform.forward * 2));
        Ray cabinetRay = new Ray(gunLaser.transform.position, gunLaser.transform.forward);
        Debug.DrawRay(cabinetRay.origin, cabinetRay.direction * 10, Color.blue);
        RaycastHit cabinetHit;
        if(Physics.Raycast(cabinetRay, out cabinetHit))
        {
            Debug.Log($"Hit {cabinetHit.collider.gameObject.name}");
            if(cabinetHit.collider == screenCollider)
            {
                Vector3 worldPoint = cabinetHit.point;
                Vector3 localPoint = screen.transform.InverseTransformPoint(worldPoint);
                reticle.localPosition = localPoint;
                Debug.Log($"Hit screen at {localPoint}");

                // convert local point to camera coordinates
                float xVal = Mathf.InverseLerp(-screenSize.x / 2, screenSize.x / 2, localPoint.x);
                float yVal = Mathf.InverseLerp(-screenSize.y / 2, screenSize.y / 2, localPoint.y);
                Vector3 cameraPoint = new Vector3(xVal * tex.width, yVal * tex.height, 0);
                Debug.Log($"Ray origin = {cameraPoint}");
                Ray cameraRay = droneCam.ScreenPointToRay(cameraPoint);
                Debug.DrawRay(cameraRay.origin, cameraRay.direction * 100, Color.red);
                //droneLaser.SetPosition(0, cameraRay.origin);
                //droneLaser.SetPosition(1, cameraRay.origin + cameraRay.direction * 10);

            }
        }
    }

    public void PressTrigger(ActivateEventArgs args)
    {
        Fire(args.interactorObject.handedness);
    }

    private void Fire(InteractorHandedness handedness)
    {
        Debug.Log($"Shot fired by {handedness}");
    }
}
