using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OptitrackConstellationManager : SingletonBehavior<OptitrackConstellationManager>
{
    // ID 1 reserved for drone
    // ID 2 reserved for landing pad

    // ID 20 for mothership
    public OptitrackRigidBody landingPad;
    public OptitrackRigidBody mothership;
    public List<BombingObjective> objectives;
    public TrackedHoop hoopPrefab;
    public TrackedCrater craterPrefab;
    public Transform poolingPoint;
    public Transform droneScene;
    public int HoopIdOffset = 3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int num = 0;
        foreach (BombingObjective objective in objectives)
        {
            num++;
            TrackedHoop hoop = Instantiate(hoopPrefab, droneScene);
            hoop.transform.position = poolingPoint.position;
            hoop.name = $"Hoop {num}";
            hoop.GetComponent<OptitrackRigidBody>().RigidBodyId = objective.HoopID;
            TrackedCrater crater = Instantiate(craterPrefab, droneScene);
            crater.transform.position = poolingPoint.position;
            crater.name = $"Crater {num}";
            crater.GetComponent<OptitrackRigidBody>().RigidBodyId = objective.CraterID;
            hoop.crater = crater;
            SequenceManager.Instance.hoopsToFlyThrough.Add(hoop);
            SequenceManager.Instance.cratersToDestroy.Add(crater);
            hoop.ToggleLight(false);
            crater.ToggleLight(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class BombingObjective
{
    public int HoopID;
    public int CraterID;
    [NonSerialized] public OptitrackRigidBody Hoop;
    [NonSerialized] public OptitrackRigidBody Crater;
}