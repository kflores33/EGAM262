using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VausPaddle : MonoBehaviour
{
    public GameObject LaserPrefab;

    float _playerSpeed = 38f;
    Vector2 _velocity = Vector2.zero;

    public LayerMask PlayerLayer;

    public Animator Animator;

    public BoxCollider2D VausCollider;
    float _defaultWidth = 8f;
    float _enlargeWidth = 12f;
    public float CurrentWidth = 8f;

    public bool AlreadyInState = false;

    public enum VausState
    {
        Default,
        Catch,
        Laser,
        Enlarge,
        Start
    }
    public VausState CurrentState = VausState.Default;

    void Start()
    {
        Animator = GetComponent<Animator>();
        VausCollider = GetComponent<BoxCollider2D>();
        PlayerLayer = LayerMask.GetMask("Vaus");

        CurrentState = VausState.Start;
    }

    private void Update()
    {
        switch (CurrentState)
        {
            case VausState.Default:
                UpdateDefault(); break;
            case VausState.Catch:
                UpdateCatch(); break;
            case VausState.Laser:
                UpdateLaser(); break;
            case VausState.Enlarge:
                UpdateEnlarge(); break;
            case VausState.Start:
                UpdateStart(); break;
        }
    }

    void FixedUpdate()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        Vector2 startPos = this.transform.position;
        Vector2 moveVectorThisFrame = (_velocity  * Time.fixedDeltaTime);
        Vector2 endPos = startPos + moveVectorThisFrame;

        RaycastHit2D hit = Physics2D.BoxCast(startPos, new Vector2(CurrentWidth, 2), 0, moveVectorThisFrame, moveVectorThisFrame.magnitude, ~PlayerLayer);

        if (hit.collider != null)
        {
            // we only car if we're going "into" this collision
            float velocityComparedToNormal = Vector2.Dot(moveVectorThisFrame.normalized, hit.normal); // if this is negative, we're going into the collision

            if (velocityComparedToNormal < 0.0f) // we're going into the collision
            {
                // try bouncing!
                endPos = hit.centroid;

                _velocity = Vector2.zero;
            }
        }

        if (Input.GetButton("Horizontal"))
        {
            float move = Input.GetAxis("Horizontal");
            _velocity = new Vector2(move, 0) * _playerSpeed;
        }
        else
        {
            _velocity = Vector2.zero;
        }

        transform.position = endPos;

    }

    public void SetVausState(VausState state)
    {
        AlreadyInState = false;
        CurrentState = state;
    }
    public void ResetVausState()
    {
        if (CurrentWidth != _defaultWidth)
        {
            CurrentWidth = _defaultWidth;
            VausCollider.size = new Vector2(CurrentWidth, 2);
        }

        AlreadyInState = false;
        CurrentState = VausState.Default;
    }

    void UpdateDefault()
    {
        if (!AlreadyInState) {
            AlreadyInState = true;
            Animator.SetTrigger("Default"); 
        }
    }
    void UpdateCatch()
    {
        if (!AlreadyInState)
        {
            AlreadyInState = true;
            CurrentWidth = _defaultWidth;
            VausCollider.size = new Vector2(CurrentWidth, 2);
            Animator.SetTrigger("Default");
        }
        if (GetComponentInChildren<BallScript>() != null)
        {
            if(_releaseHoldCoroutine == null)
            {
                _releaseHoldCoroutine = StartCoroutine(ReleaseHold());
            }

            BallScript ball = GetComponentInChildren<BallScript>();

            if(Input.GetKeyDown(KeyCode.X))
            {
                StopCoroutine(_releaseHoldCoroutine);
                _releaseHoldCoroutine = null;

                ball.ReleaseBall();
            }
        }
    }

    public Transform LaserL;
    public Transform LaserR;
    bool _canShoot = true;
    void UpdateLaser()
    {
        if (!AlreadyInState)
        {
            AlreadyInState = true;
            CurrentWidth = _defaultWidth;
            VausCollider.size = new Vector2(CurrentWidth, 2);
            Animator.SetTrigger("Laser");
        }
        if (Input.GetKeyDown(KeyCode.X) && _canShoot)
        {
            GameObject laserL = Instantiate(LaserPrefab, LaserL.position, Quaternion.identity);
            GameObject laserR = Instantiate(LaserPrefab, LaserR.position, Quaternion.identity);

            StartCoroutine(ShootCD());
        }
    }

    IEnumerator ShootCD()
    {
        _canShoot = false;
        yield return new WaitForSeconds(0.1f);
        _canShoot = true;

        StopCoroutine(ShootCD());
    }

    void UpdateEnlarge()
    {
        if (!AlreadyInState)
        {
            AlreadyInState = true;
            CurrentWidth = _enlargeWidth;
            VausCollider.size = new Vector2(CurrentWidth, 2);

            Animator.SetTrigger("Enlarge");
        }
    }

    void UpdateStart()
    {
        if (!AlreadyInState)
        {
            AlreadyInState = true;
            CurrentWidth = _defaultWidth;
            VausCollider.size = new Vector2(CurrentWidth, 2);
            Animator.SetTrigger("Default");
        }
        if (GetComponentInChildren<BallScript>() != null)
        {
            if (_releaseHoldCoroutine == null)
            {
                _releaseHoldCoroutine = StartCoroutine(ReleaseHold());
            }

            BallScript ball = GetComponentInChildren<BallScript>();
            ball.ChangeSpeed(0f);
            if (ball.transform.position.y != -26.9f)
            {
                ball.transform.position = new Vector2(this.transform.position.x, -26.9f);
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                StopCoroutine(_releaseHoldCoroutine);
                _releaseHoldCoroutine = null;

                ball.ChangeSpeed(ball.BallData.BaseSpeed);

                ball.ReleaseBall();
                ResetVausState();
            }
        }
    }

    Coroutine _releaseHoldCoroutine;
    IEnumerator ReleaseHold()
    {
        yield return new WaitForSeconds(6f);
        if (GetComponentInChildren<BallScript>() != null)
        {
            BallScript ball = GetComponentInChildren<BallScript>();
            if (CurrentState == VausState.Start)
            {
                ball.ChangeSpeed(ball.BallData.BaseSpeed);
                ResetVausState();
            }

            ball.ReleaseBall();
        }

        StopCoroutine(_releaseHoldCoroutine);
        _releaseHoldCoroutine = null;
    }
}
