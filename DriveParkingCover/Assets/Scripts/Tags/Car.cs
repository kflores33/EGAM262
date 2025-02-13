using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Car : MonoBehaviour
{
    public CarStats stats;

    SpriteRenderer spriteRenderer;

    LineDrawer lineDrawer;
    List<Vector2> waypointsCopy;

    public float maxDistance;

    public enum CarStates
    {
        Idle,
        ReadyToDrive,
        Driving
    }
    public CarStates currentState = CarStates.Idle;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        spriteRenderer.sprite = stats.carSprite;
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
        if (FindAnyObjectByType<LineDrawer>() != null)
        {
            if (FindAnyObjectByType<LineDrawer>().carColor.colorString == stats.colorString)
            {
                lineDrawer = FindAnyObjectByType<LineDrawer>();
                currentState = CarStates.ReadyToDrive;
            }
        }
    }
    void UpdateReadyToDrive()
    {
        // once line exists, store the waypoints
        if (waypointsCopy != null)
        {
            waypointsCopy = lineDrawer.waypoints;
        }
    }
    void UpdateDriving() 
    {
        // once driving, delete already reached waypoints

    }
}
