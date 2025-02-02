﻿using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class EnsembleItem : Agent
{
    public EnsembleManager manager;

    public int position;

    void Start()
    {
    }

    void Update()
    {
    }

    public override void Initialize()
    {
        manager.SubscribeItem(this);
    }

    /*
        vectorAction[]
        Index 0: acceleration (-1 = accel backwards, +1 = accel forward)
        Index 1: turn (-1 = left, +1 = right)
    */
    public override void OnActionReceived(float[] vectorAction)
    {
        manager.PushAction(vectorAction, position);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        float[] distances = manager.GetSensorDistances();

        // 12 observations, which correspond to the GetSensorDistances() output array size
        foreach (float distance in distances)
        {
            sensor.AddObservation(distance);
        }

        if (manager.observeDistance)
        {
            // Add euclidean distance to target spot
            sensor.AddObservation(Vector3.Distance(manager.transform.localPosition, manager.targetSpot.localPosition));
        }
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Vertical");
        actionsOut[1] = Input.GetAxis("Horizontal");
    }
}
