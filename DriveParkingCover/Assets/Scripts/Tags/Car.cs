using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public CarStats stats;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

    LineDrawer lineDrawer;
    List<Vector2> waypoints;

    public Coroutine followPathCoroutine = null;
    public enum CarStates
    {
        Idle,
        ReadyToDrive
    }
    public CarStates currentState = CarStates.Idle;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponentInChildren<Rigidbody2D>();

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
        if (waypoints == null)
        {
            waypoints = lineDrawer.waypoints;
        }
    }

    public void StartDriving()
    {
        StartCoroutine(FollowPath());
    }
    IEnumerator FollowPath()
    {
        Debug.Log("start driving");

        foreach (Vector2 waypoint in waypoints)
        {
            // when the distance between the car and the waypoint is greater than maxDistance...
            while (Vector2.Distance(transform.position, waypoint) >= stats.maxDistance) 
            {
                transform.right = Vector2.Lerp(new Vector2(transform.position.x, transform.position.y), 
                    (new Vector2(transform.position.x, transform.position.y) - (waypoint)) * -1, Time.deltaTime * stats.carSpeed); // rotate car in direction of waypoint

                rb.AddForce(transform.right * stats.carSpeed * Time.deltaTime); // move car forward

                yield return null;
            }
            yield return null;
        }
        while (rb.linearVelocity.sqrMagnitude > 0) // sqrMagnitude = absolute distance squared
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, Time.deltaTime); //interpolate between 
            yield return null;
        }
    }
}
