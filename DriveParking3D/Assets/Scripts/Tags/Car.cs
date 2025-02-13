using System;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public CarStats stats;

    MeshRenderer meshRenderer;

    LineDrawer lineDrawer;

    public enum CarStates
    {
        Idle,
        ReadyToDrive,
        Driving
    }
    public CarStates currentState = CarStates.Idle;

    private void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();

        meshRenderer.material = stats.colorMat;
        this.name = $"{stats.colorString}Car";
    }

    private void Update()
    {
        switch (currentState) {
            case CarStates.Idle:
                UpdateIdle(); break;
            case CarStates.ReadyToDrive:
                UpdateReadyToDrive(); break;
            case CarStates.Driving:
                UpdateDriving(); break;
        }
    }

    void UpdateIdle()
    {
        if (FindAnyObjectByType<LineDrawer>().carColor.colorString == stats.colorString)
        {
            lineDrawer = FindAnyObjectByType<LineDrawer>();
            currentState = CarStates.ReadyToDrive;
        }
    }
    void UpdateReadyToDrive()
    {
        // once line exists, store 
    }
    void UpdateDriving() 
    {
    
    }
}
