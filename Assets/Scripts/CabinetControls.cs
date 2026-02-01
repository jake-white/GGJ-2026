using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class CabinetControls : SingletonBehavior<CabinetControls>
{
    public Camera droneCam, bombCam;
    public DroneTurret turret;
    public LineRenderer gunLaser, droneLaser;
    public RectTransform reticle;

    public Canvas screen;
    public RenderTexture tex;
    public RawImage crtScreen;
    public Material shootView, bombView;
    public InputActionReference fireLeft, fireRight;

    private Collider screenCollider;
    private RectTransform screenSpace;
    public XRGrabInteractable interactable;

    private Vector2 screenSize;
    private bool bombingMode = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        screenCollider = screen.GetComponent<Collider>();
        screenSpace = screen.gameObject.GetComponent<RectTransform>();
        screenSize = new Vector2(screenSpace.rect.width, screenSpace.rect.height);
        fireLeft.action.performed += ctx => BombWithButton(true);
        fireRight.action.performed += ctx => BombWithButton(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (bombingMode)
        {
            gunLaser.positionCount = 0;
        }
        else
        {
            gunLaser.positionCount = 2;
            gunLaser.SetPosition(0, gunLaser.transform.position);
            gunLaser.SetPosition(1, gunLaser.transform.position + (gunLaser.transform.forward * 2));
            Ray cabinetRay = new Ray(gunLaser.transform.position, gunLaser.transform.forward);
            RaycastHit cabinetHit;
            if (Physics.Raycast(cabinetRay, out cabinetHit))
            {
                if (cabinetHit.collider == screenCollider)
                {
                    Vector3 worldPoint = cabinetHit.point;
                    Vector3 localPoint = screen.transform.InverseTransformPoint(worldPoint);
                    reticle.localPosition = localPoint;

                    // convert local point to camera coordinates
                    float xVal = Mathf.InverseLerp(-screenSize.x / 2, screenSize.x / 2, localPoint.x);
                    float yVal = Mathf.InverseLerp(-screenSize.y / 2, screenSize.y / 2, localPoint.y);
                    Vector3 cameraPoint = new Vector3(xVal * tex.width, yVal * tex.height, 0);
                    Ray cameraRay = droneCam.ScreenPointToRay(cameraPoint);
                    turret.Aim(cameraRay);
                }
            }
        }
    }

    public void ActivateTrigger(ActivateEventArgs args)
    {
        turret.ActivateTurret(args.interactorObject.handedness);
    }

    public void DeactivateTrigger(DeactivateEventArgs args)
    {
        turret.DeactivateTurret(args.interactorObject.handedness);
    }

    public void SelectEntered(SelectEnterEventArgs args)
    {
    }

    public void SelectExited(SelectExitEventArgs args)
    {
        turret.DeactivateTurret(args.interactorObject.handedness);
    }

    public void PressBombButton(SelectEnterEventArgs args)
    {
        turret.DropBomb();
    }

    public void BombWithButton(bool left)
    {
        Debug.Log("BOMBING!" + left);
        foreach(IXRSelectInteractor interactor in interactable.interactorsSelecting)
        {
            if ((left && interactor.handedness == InteractorHandedness.Left) || (!left && interactor.handedness == InteractorHandedness.Right))
            {
                turret.DropBomb();
            }
        }
    }

    public void PressStartButton(SelectEnterEventArgs args)
    {
        SequenceManager.Instance.StartGame();
    }

    public void OnLeverBomb()
    {
        bombingMode = true;
        crtScreen.material = bombView;
    }

    public void OnLeverShoot()
    {
        bombingMode = false;
        crtScreen.material = shootView;
    }

    public Camera GetCurrentDroneCamera()
    {
        if (bombingMode) return bombCam;
        return droneCam;
    }
}
