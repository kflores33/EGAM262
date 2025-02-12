using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public CarStats stats;

    MeshRenderer meshRenderer;

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

        }
    void UpdateReadyToDrive()
    {

    }
    void UpdateDriving() 
    {
    
    }
}
