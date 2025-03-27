using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallScript : MonoBehaviour
{
    public BallData BallData;
    float _currentSpeed;    

    #region Tim Ball Script
    public float Radius;
    public float StartingAngle;

    private Vector2 _velocity = Vector2.zero;

    private void Start()
    {
        _currentSpeed = BallData.BaseSpeed;
        _velocity = (Quaternion.AngleAxis(StartingAngle, Vector3.forward) * Vector2.right) * _currentSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 startPos = this.transform.position;
        Vector2 moveVectorThisFrame = (_velocity * Time.fixedDeltaTime);
        Vector2 endPos = startPos + moveVectorThisFrame;

        RaycastHit2D hit = Physics2D.CircleCast(startPos, Radius, moveVectorThisFrame, moveVectorThisFrame.magnitude);

        if (hit.collider != null)
        {
            // we only car if we're going "into" this collision
            float velocityComparedToNormal = Vector2.Dot(moveVectorThisFrame.normalized, hit.normal); // if this is negative, we're going into the collision

            if (velocityComparedToNormal < 0.0f) // we're going into the collision
            {
                // try bouncing!
                endPos = hit.centroid;

                _velocity = Vector2.Reflect(_velocity, hit.normal);
            }
        }


        transform.position = endPos;

    }
    #endregion
}
