using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class LilGuy : MonoBehaviour
{
    Cursor _cursor;

    public float minDetectRadius = 0.5f;
    public float maxDetectRadius = 2.0f;

    public float currentDetectRadius = 1.0f;
    public float detectRadiusMultiplier = 1.0f; // New multiplier value between 0 and 1

    private void Start()
    {
        _cursor = FindAnyObjectByType<Cursor>();
    }

    private void Update()
    {
        Debug.Log("Detection radius is now " + AdjustDetectRadiusSize() + " big.");
        RunFromCursor();
    }

    void RunFromCursor()
    {
        Physics2D.CircleCast(transform.position, currentDetectRadius, Vector2.zero, 0f);

        // Get the mouse position in world coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Set z to 0 since we're in 2D

        // Calculate the direction away from the mouse
        Vector3 direction = (transform.position - mousePosition).normalized;
        // Move the object away from the mouse
        transform.position += direction * Time.deltaTime;
    }

    float AdjustDetectRadiusSize()
    {
        Vector2 cursorVelocity = _cursor.CurrentVelocity;
        float cursorSpeed = cursorVelocity.magnitude;

        float radius = cursorSpeed * detectRadiusMultiplier;
        radius = Mathf.Clamp(radius, minDetectRadius, maxDetectRadius);
        currentDetectRadius = radius;

        return currentDetectRadius;
    }
}
