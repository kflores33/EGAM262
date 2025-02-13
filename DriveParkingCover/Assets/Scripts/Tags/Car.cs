using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public CarStats stats;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

    LineDrawer lineDrawer;
    List<Vector2> waypoints = new List<Vector2>();

    public bool canGetLine = false;

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
        if (canGetLine)
        {
            LineDrawer[] lines = FindObjectsByType<LineDrawer>(FindObjectsSortMode.None);
            foreach (LineDrawer line in lines)
            {
                Debug.Log("entered for each loop");
                if (line.carColor.colorString == stats.colorString)
                {
                    lineDrawer = line;

                    foreach (Vector2 waypoint in lineDrawer.waypoints)
                    {
                        Debug.Log("got lineDrawer reference!");
                        waypoints.Add(waypoint);
                    }

                    currentState = CarStates.ReadyToDrive;

                    break;
                }
            }
            canGetLine = false;
        }
    }
    void UpdateReadyToDrive()
    {

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
                transform.up = Vector2.Lerp(new Vector2(transform.position.x, transform.position.y), 
                    (new Vector2(transform.position.x, transform.position.y) - (waypoint)) * -1, Time.deltaTime * stats.carSpeed); // rotate car in direction of (next) waypoint

                //rb.AddForce(transform.up * stats.carSpeed * Time.deltaTime);
                transform.position = Vector2.Lerp(new Vector2(transform.position.x, transform.position.y), waypoint, Time.deltaTime * stats.carSpeed);

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponentInParent<Car>() != null)
        {
            Debug.Log("hit car");
            RestartScene restart = FindFirstObjectByType<RestartScene>();
            restart.restartGame();
        }
    }
}
