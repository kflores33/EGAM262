using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class LilGuy : MonoBehaviour
{
    Cursor _cursor;

    [Header("Movement Speed")]
    public float minSpeed = 1.0f;
    public float maxSpeed = 5.0f;
    public float targetSpeed = 0.0f;

    float _currentSpeed = 0.0f;

    public float acceleration = 1.0f;
    public float deceleration = 1.0f;

    [Header("Detection Radius")]
    public float minDetectRadius = 0.5f;
    public float maxDetectRadius = 2.0f;

    public float currentDetectRadius = 1.0f;
    public float detectRadiusMultiplier = 1.0f; // New multiplier value between 0 and 1

    [Header("Launch Variables")]
    public float launchSpeed = 10.0f;
    public float maxLaunchForce = 8;

    [Header("Misc")]
    public float BufferTime = 1.0f;
    LayerMask _elephantLayer;
    public enum ElephantState
    {
        Idle,
        RunAway,
        Caught
    }
    ElephantState _state = ElephantState.Idle;

    Coroutine _bufferCoroutine;

    private void Start()
    {
        _cursor = FindAnyObjectByType<Cursor>();

        _elephantLayer = LayerMask.GetMask("Elephant");
    }

    private void Update()
    {
        AdjustDetectRadiusSize();

        Debug.Log("Detection radius is now " + AdjustDetectRadiusSize() + " big.");

        switch (_state)
        {
            case ElephantState.Idle:
                UpdateIdle();
                break;
            case ElephantState.RunAway:
                UpdateRunAway();
                break;
            case ElephantState.Caught:
                UpdateCaught();
                break;
        }
    }

    void UpdateIdle()
    {
        AdjustSpeed();

        if (ShouldRunFromCursor())
        {
            _state = ElephantState.RunAway;
            Debug.Log("Running away from cursor!");

            _bufferCoroutine = StartCoroutine(WaitToReturnIdle()); // Start the coroutine to wait before returning to idle state
        }
    }
    void UpdateRunAway()
    {
        float currentSpeed = Mathf.MoveTowards(targetSpeed, maxSpeed, acceleration);

        // Get the mouse position in world coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Set z to 0 since we're in 2D

        // Calculate the direction away from the mouse
        Vector3 direction = (transform.position - mousePosition).normalized * currentSpeed;
        direction = AdjustAngle(direction); // slight variation in angle

        // Move the object away from the mouse
        transform.position += direction * Time.deltaTime;

        _currentSpeed = currentSpeed;
    }
    void UpdateCaught()
    {
        // Handle caught behavior
        Debug.Log("Caught by the cursor!");

        Debug.DrawRay(transform.position, _cursor.transform.position, Color.red, 0.5f);
    }

    bool ShouldRunFromCursor()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, currentDetectRadius, ~_elephantLayer);
        foreach (Collider2D collider in colliders)
        {
            if (collider.GetComponent<Cursor>() != null)
            {
                return true; // The cursor is within the detection radius
            }
        }

        return false; // The cursor is not within the detection radius
    }

    float AdjustDetectRadiusSize()
    {
        Vector2 cursorVelocity = _cursor.CurrentVelocity;
        float cursorSpeed = cursorVelocity.magnitude;

        float radius = cursorSpeed * detectRadiusMultiplier;
        radius = Mathf.Clamp(radius, minDetectRadius, maxDetectRadius);
        currentDetectRadius = radius;

        Vector2 xLeft = transform.position;
        xLeft.x -= radius;
        Vector2 xRight = transform.position;
        xRight.x += radius;

        Debug.DrawLine(xLeft, xRight, Color.blue, 0.5f);

        return currentDetectRadius;
    }

    float AdjustSpeed()
    {
        Vector2 cursorVelocity = _cursor.CurrentVelocity;
        float cursorSpeed = cursorVelocity.magnitude;

        targetSpeed = Mathf.Clamp(cursorSpeed, minSpeed, maxSpeed);

        return targetSpeed;
    }

    Vector2 AdjustAngle(Vector2 direction)
    {
        // slightly adjust angle to randomize the direction
        float angle = Random.Range(-0.2f, 0.2f);
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        Vector2 adjustedDirection = rotation * direction;

        return adjustedDirection;
    }

    IEnumerator WaitToReturnIdle()
    {
        yield return new WaitForSeconds(BufferTime); 

        if (!ShouldRunFromCursor())
        {
            yield return new WaitForSeconds(0.25f);

            _state = ElephantState.Idle;
            Debug.Log("No longer running away from cursor.");

            _bufferCoroutine = null; // Reset the coroutine reference
        }
        else
        {
            _bufferCoroutine = StartCoroutine(WaitToReturnIdle());
        }
    }

    public void OnClickedByPlayer()
    {
        if (_state == ElephantState.RunAway)
        {
            _state = ElephantState.Caught;
            Debug.Log("Caught by the player!");
        }
    }
    public void OnReleasedByPlayer()
    {
        // distance between cursor and elephant = launch force
        float distance = Vector2.Distance(transform.position, _cursor.transform.position);
        float launchForce = Mathf.Clamp(distance, 0, maxLaunchForce);

        Vector2 direction = (transform.position - _cursor.transform.position).normalized;

        LaunchElephant(direction, launchForce);

        if (_state == ElephantState.Caught)
        {
            _state = ElephantState.Idle;
            Debug.Log("Released by the player!");
        }
    }

    void LaunchElephant(Vector2 direction, float force)
    {

    }
}
