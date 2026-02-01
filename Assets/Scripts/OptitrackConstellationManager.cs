using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OptitrackConstellationManager : MonoBehaviour
{
    // ID 1 reserved for drone
    // ID 2 reserved for landing pad
    public OptitrackRigidBody landingPad;
    public List<BombingObjective> objectives;
    public TrackedHoop hoopPrefab;
    public TrackedCrater craterPrefab;
    public Transform poolingPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (BombingObjective objective in objectives)
        {
            TrackedHoop hoop = Instantiate(hoopPrefab);
            hoop.transform.position = poolingPoint.position;
            hoop.GetComponent<OptitrackRigidBody>().RigidBodyId = objective.HoopID;
            TrackedCrater crater = Instantiate(craterPrefab);
            crater.transform.position = poolingPoint.position;
            crater.GetComponent<OptitrackRigidBody>().RigidBodyId = objective.CraterID;
            hoop.crater = crater;
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