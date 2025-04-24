using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class LilGuy : MonoBehaviour
{
    #region variables
    Cursor _cursor;
    LineRenderer _lineRenderer;
    RespawnPlayer RespawnPlayer;

    [Header("Movement Speed")]
    public float minSpeed = 1.0f;
    public float maxSpeed = 5.0f;
    public float targetSpeed = 0.0f;

    public float speedMultiplier = 2.0f; // multiplier for speed adjustment

    float _currentSpeed = 0.0f;
    Vector2 _currentDirection;

    public float defaultAcceleration = 1.0f;
    public float defaultDeceleration = 1.0f;

    public float acceleration = 1.0f;
    public float deceleration = 1.0f;

    [Header("Detection Radius")]
    public float minDetectRadius = 0.5f;
    public float maxDetectRadius = 2.0f;

    public float currentDetectRadius = 1.0f;
    public float detectRadiusMultiplier = 1.0f; 

    [Header("Launch Variables")]
    public float launchSpeed = 10.0f;
    public float maxLaunchForce = 8;
    public float launchForceMultiplier = 2.0f; 

    [Header("Misc")]
    public float BufferTime = 1.0f;
    [Tooltip("Adjusts the randomness of Elephant's direction"), Range(0f, 10f)] public float AngleVariance = 0.5f; // angle variance in degrees
    LayerMask _elephantLayer;
    public enum ElephantState
    {
        Idle,
        RunAway,
        Caught,
        Launching
    }
    [SerializeField] ElephantState _state = ElephantState.Idle;

    Coroutine _bufferCoroutine;
    #endregion

    private void Start()
    {
        _cursor = FindAnyObjectByType<Cursor>();
        RespawnPlayer = RespawnPlayer.Instance;
        _lineRenderer = GetComponentInChildren<LineRenderer>();

        _elephantLayer = LayerMask.GetMask("Elephant");
    }

    Vector2 _startPos;
    Vector2 _endPos;

    private void Update()
    {
        AdjustDetectRadiusSize();

        //Debug.Log("Detection radius is now " + AdjustDetectRadiusSize() + " big.");

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
            case ElephantState.Launching:
                UpdateLaunching();
                break;
        }     

        // moving
        if (targetSpeed <= _currentSpeed) // if moving faster than target speed, decelerate
        {
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, targetSpeed, deceleration * Time.deltaTime);
        }
        else _currentSpeed = Mathf.MoveTowards(_currentSpeed, targetSpeed, acceleration * Time.deltaTime); // otherwise accelerate

        //transform.position += (Vector3)(_currentDirection * _currentSpeed * Time.deltaTime);

        MoveElephant();
    }

    #region Switch States
    void UpdateIdle()
    {
        acceleration = defaultAcceleration; // Reset acceleration to default
        deceleration = defaultDeceleration; // Reset deceleration to default
        targetSpeed = 0.0f; // Set target speed to 0 for idle state

        if (ShouldRunFromCursor())
        {
            _state = ElephantState.RunAway;
            Debug.Log("Running away from cursor!");

            _bufferCoroutine = StartCoroutine(WaitToReturnIdle()); // Start the coroutine to wait before returning to idle state
        }
    }
    void UpdateRunAway()
    {
        // Move the object away from the mouse
        ChangeDirectionBasedOnCursorPos();

        targetSpeed = AdjustSpeed(); // Adjust speed based on cursor velocity
    }
    void UpdateCaught()
    {
        // Handle caught behavior
        Debug.Log("Caught by the cursor!");

        HandleLineRenderer();

        Debug.DrawRay(transform.position, _cursor.transform.position, Color.red, 0.5f);
    }
    void UpdateLaunching()
    {
        if (_currentSpeed == targetSpeed) // set back to idle
        {
            _state = ElephantState.Idle;
        }
    }
    #endregion

    bool _ignoreCursorInfluence = false; // flag to ignore cursor influence
    void MoveElephant()
    {
        _startPos = transform.position; // position before movement
        Vector2 currentVelocity = _currentDirection * _currentSpeed;
        Vector2 moveVectorThisFrame = currentVelocity * Time.deltaTime; // move elephant by this amount/direction
        _endPos = (Vector2)transform.position + moveVectorThisFrame; // position to move to

        LayerMask layersToInclude = Physics2D.AllLayers & ~(_elephantLayer | _cursor.ThisLayer); // Include all layers except the elephant and cursor layers

        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.8f, _currentDirection, moveVectorThisFrame.magnitude, layersToInclude);

        if (hit.collider != null)
        {
            Debug.Log("Hit something: " + hit.collider.name);

            // Calculate the reflection vector only if the object is moving toward the surface
            float velocityComparedToNormal = Vector2.Dot(currentVelocity.normalized, hit.normal);

            ObstacleBehavior obstacleHit = hit.collider.GetComponent<ObstacleBehavior>();
            if (obstacleHit != null)
            {
                HandleCollision(hit.collider.GetComponent<ObstacleBehavior>(),
                    velocityComparedToNormal, currentVelocity, hit);
            }
        }

        transform.position = _endPos; // Move the elephant to the new position
    }

    #region collision behavior
    public void HandleCollision(ObstacleBehavior obstacleHit,
        float velocityComparedToNormal, Vector2 currentVelocity, RaycastHit2D hit)
    {
        switch (obstacleHit.ObstacleType.type)
        {
            case ObstacleType.Type.Wall:
                // Handle wall collision
                WallBehavior(velocityComparedToNormal, currentVelocity, hit);
                break;
            case ObstacleType.Type.Spike:
                // Handle spike collision
                SpikeBehavior();
                break;
            case ObstacleType.Type.SpeedUp:
                // Handle speed up collision
                SpeedUpBehavior();
                break;
            case ObstacleType.Type.Destroyable:
                DestroyableBehavior(velocityComparedToNormal, currentVelocity, hit);
                break;
            case ObstacleType.Type.Checkpoint:
                // Handle checkpoint collision
                CheckpointBehavior(hit);
                break;
            case ObstacleType.Type.Goal:
                // Handle goal collision
                GoalBehavior();
                break;
        }
    }

    void WallBehavior(float velocityComparedToNormal, Vector2 currentVelocity, RaycastHit2D hit)
    { 
        if (velocityComparedToNormal < 0.0f)
        {
            // Reflect the velocity and adjust the direction
            currentVelocity = Vector2.Reflect(currentVelocity, hit.normal);
            _currentDirection = currentVelocity.normalized;

            // Adjust position to avoid overlapping with the hit object
            _endPos = hit.centroid;

            // Temporarily ignore cursor influence to allow natural reaction
            if (_generalBuffer == null)
            {
                _generalBuffer = StartCoroutine(Buffer(0.5f, _ignoreCursorInfluence));
            }
        }
    }
    void SpikeBehavior()
    {
        // hurt elephant (die and restart from checkpoint)
        Die();
    }
    void SpeedUpBehavior()
    {
        // increase speed of elephant to max on contact (naturally decelerate over time)
        if (_currentSpeed <= maxSpeed) _currentSpeed = maxSpeed + 5;
        else _currentSpeed = maxLaunchForce; // reset to max launch speed if already at max speed
    }
    void DestroyableBehavior(float velocityComparedToNormal, Vector2 currentVelocity, RaycastHit2D hit)
    {
        // destroy the object
        if(_currentSpeed > maxSpeed)
        {
            Destroy(hit.collider.gameObject);
            _currentSpeed -= maxSpeed / 4; // slow down after collision
        }
        else
        {
            WallBehavior(velocityComparedToNormal, currentVelocity, hit); // Default behavior for unknown types
        }
    }
    void CheckpointBehavior(RaycastHit2D hit)
    {
        if (hit.collider.gameObject == RespawnPlayer.LatestCheckpoint)
        {
            Debug.Log("Already at this checkpoint!");
            return;
        }

        if (RespawnPlayer.Checkpoints.Count != 0)
        {
            for (int i = 0; i < RespawnPlayer.Checkpoints.Count; i++)
            {
                if (hit.collider.gameObject == RespawnPlayer.Checkpoints[i])
                {
                    Debug.Log("Already at this checkpoint!");
                    return;
                }
            }
        }

        // save the current position as a checkpoint
        RespawnPlayer.SetNewCheckpoint(hit.collider.gameObject); // set the checkpoint to the object hit
        UIManager.Instance.CheckpointReached(); // show the checkpoint reached UI
    }

    void GoalBehavior()
    {
        UIManager.Instance.EndGame(); // show the win screen
    }
    #endregion

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

    void ChangeDirectionBasedOnCursorPos()
    {
        if (_ignoreCursorInfluence) return;

        // Get the mouse position in world coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Set z to 0 since we're in 2D

        // Calculate the direction away from the mouse
        _currentDirection = (transform.position - mousePosition).normalized;
        _currentDirection = AdjustAngle(_currentDirection); // slight variation in angle
    }

    #region Adjustments
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
        float cursorSpeed = cursorVelocity.magnitude * speedMultiplier;

        targetSpeed = Mathf.Clamp(cursorSpeed, minSpeed, maxSpeed);

        return targetSpeed;
    }

    Vector2 AdjustAngle(Vector2 direction)
    {
        // slightly adjust angle to randomize the direction
        float angle = Random.Range(-AngleVariance, AngleVariance);
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        Vector2 adjustedDirection = rotation * direction;

        return adjustedDirection;
    }
    #endregion


    public void OnClickedByPlayer()
    {
        if (_state == ElephantState.RunAway)
        {
            _state = ElephantState.Caught;
            Debug.Log("Caught by the player!");

            targetSpeed = 0;
            _currentSpeed = 0;

            if (_bufferCoroutine != null) 
            {                
                StopCoroutine(_bufferCoroutine); // Stop the coroutine if it's running
                _bufferCoroutine = null; // Stop the coroutine if it's running
            }

            _lineRenderer.enabled = true; // Enable the line renderer
        }
    }
    public void OnReleasedByPlayer()
    {
        // distance between cursor and elephant = launch force
        float distance = Vector2.Distance(transform.position, _cursor.transform.position);
        float launchForce = Mathf.Clamp(distance, 0, maxLaunchForce);

        Vector2 direction = (transform.position - _cursor.transform.position).normalized;

        if (_state == ElephantState.Caught)
        {
            _state = ElephantState.Launching;
            Debug.Log("Released by the player!");

            LaunchElephant(direction, launchForce);

            _lineRenderer.enabled = false;
        }
    }

    void LaunchElephant(Vector2 direction, float force)
    {
        _currentDirection = direction;
        launchSpeed = force * launchForceMultiplier; // Adjust the launch speed based on the force applied

        acceleration = 100.0f; // Set a high acceleration for the launch
        targetSpeed = launchSpeed;
    }

    public Color startColorLight;
    public Color endColorLight;
    public Color startColorHeavy;
    public Color endColorHeavy;

    void HandleLineRenderer()
    {
        float distance = Vector2.Distance(transform.position, _cursor.transform.position);
        float launchForce = Mathf.Clamp(distance, 0, maxLaunchForce);

        Vector2 direction = (_cursor.transform.position - transform.position).normalized;

        Vector2 endPosition = _cursor.transform.position; // Get the cursor position

        if (distance > maxLaunchForce)
        {
            endPosition = (Vector2)transform.position + direction * maxLaunchForce; // Clamp the end position to the max launch force
        }

        if (_lineRenderer.GetPosition(0) != transform.position) _lineRenderer.SetPosition(0, transform.position); // Set the start position to the elephant's position
        _lineRenderer.SetPosition(1, endPosition); // Set the end position

        Color currentStartColor = Color.Lerp(startColorLight, startColorHeavy, launchForce / maxLaunchForce);
        Color currentEndColor = Color.Lerp(endColorLight, endColorHeavy, launchForce / maxLaunchForce);

        _lineRenderer.startWidth = Mathf.Lerp(0.1f, 0.25f, launchForce / maxLaunchForce);
        _lineRenderer.endWidth = Mathf.Lerp(0.1f, 1f, launchForce / maxLaunchForce);

        _lineRenderer.startColor = currentStartColor;
        _lineRenderer.endColor = currentEndColor;
    }

    void Die()
    {
        Destroy(gameObject); // Destroy the elephant object
        // Handle death behavior
        Debug.Log("Elephant died!");
        // Reset the elephant's position to the last checkpoint
        RespawnPlayer.Respawn();
    }

    #region coroutines
    Coroutine _generalBuffer = null;
    public IEnumerator Buffer(float time, bool optionalSwitch = false)
    {
        optionalSwitch = true;

        yield return new WaitForSeconds(time);

        if (optionalSwitch)
        {
            optionalSwitch = false;
        }

        _generalBuffer = null; // Reset the coroutine reference
    }
    IEnumerator WaitToReturnIdle()
    {
        yield return new WaitForSeconds(BufferTime); 

        if (!ShouldRunFromCursor())
        {
            //yield return new WaitForSeconds(0.25f);
            _state = ElephantState.Idle;
            Debug.Log("No longer running away from cursor.");

            _bufferCoroutine = null; // Reset the coroutine reference
        }
        else
        {
            _bufferCoroutine = StartCoroutine(WaitToReturnIdle());
        }
    }
    #endregion
}
