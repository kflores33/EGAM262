using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class BallScript : MonoBehaviour
{
    public Transform RefPos;
    public BallData BallData;
    public float CurrentSpeed = 0;    

    #region Tim Ball Script
    public float Radius;
    public float StartingAngle;

    public bool IsSlowed = false;

    private Vector2 _velocity = Vector2.zero;
    public float StoredAngle;

    private void Start()
    {
        RefPos = FindAnyObjectByType<FailPointRef>().transform;

        StoredAngle = StartingAngle;

        _velocity = (Quaternion.AngleAxis(StartingAngle, Vector3.forward) * Vector2.right)/* * CurrentSpeed*/;
    }

    private void Update()
    {
        if(this.transform.position.y < RefPos.position.y)
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        BallMovement(CurrentSpeed);
    }
    #endregion

    public void ChangeSpeed(float newSpeed)
    {
        CurrentSpeed = newSpeed;
    }

    void BallMovement(float speed)
    {
        _velocity = _velocity.normalized * speed;

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

            if(hit.collider.GetComponent<BrickScript>() != null)
            {
                hit.collider.GetComponent<BrickScript>().TakeDamage();
            }

            if (hit.collider.GetComponent<VausPaddle>() != null)
            {
                // if vaus is in catch state
                if(hit.collider.GetComponent<VausPaddle>().CurrentState == VausPaddle.VausState.Catch && !_ignoreCollision)
                {
                    endPos = hit.centroid;

                    if(StoredAngle == 0) StoredAngle = Mathf.Atan2(_velocity.y, _velocity.x) * Mathf.Rad2Deg;

                    _velocity = Vector2.zero;

                    transform.SetParent(hit.collider.transform);
                }
            }
        }

        transform.position = endPos;

    }

    public float CurrentAngle()
    {
        return Mathf.Atan2(_velocity.y, _velocity.x) * Mathf.Rad2Deg;
    }

    bool _ignoreCollision = false;
    public void ReleaseBall()
    {
        transform.SetParent(null);
        _velocity = (Quaternion.AngleAxis(StoredAngle, Vector3.forward) * Vector2.up) * CurrentSpeed;

        StoredAngle = 0;

        StartCoroutine(_ignoreCollisionCoroutine());
    }

    IEnumerator _ignoreCollisionCoroutine()
    {
        _ignoreCollision = true;
        yield return new WaitForSeconds(0.2f);
        _ignoreCollision = false;

        StopCoroutine(_ignoreCollisionCoroutine());
    }
}
