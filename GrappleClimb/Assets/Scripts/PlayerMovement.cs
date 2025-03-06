using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

// referenced scripts from this repo: https://github.com/Matthew-J-Spencer/Ultimate-2D-Controller/blob/main/Scripts/PlayerController.cs
// and this video: https://www.youtube.com/watch?v=O6VX6Ro7EtA&list=TLPQMDQwMzIwMjXkLjMbxR-3Jg&index=2
public struct FrameInput
{
    public bool JumpDown;
    public bool JumpHeld;
    public Vector3 Move;
}

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] DefaultInputSubscription GetInput;
    Rigidbody _rb;
    CapsuleCollider _col;

    private FrameInput _frameInput;
    private Vector3 _frameVelocity;

    [SerializeField] LayerMask _wallLayer;
    [SerializeField] private PlayerStats _stats;

    public Vector2 FrameInput => _frameInput.Move;
    public event Action<bool, float> GroundedChanged;
    public event Action<bool, float> WalledChanged;
    public event Action Jumped;

    private float _time;

    private void Awake()
    {
        _rb = GetComponentInChildren<Rigidbody>();
        _col = GetComponentInChildren<CapsuleCollider>();
    }

    private void Update()
    {
        _time += Time.deltaTime;
        GatherInput();
    }

    bool _jumpDownPerformed = false;
    private void GatherInput()
    {
        _frameInput = new FrameInput
        {
            JumpDown = GetInput.JumpInput && !_jumpDownPerformed,
            JumpHeld = GetInput.JumpInput,
            Move = GetInput.AimPlayer
        };
        _frameInput.Move.z = 0;

        #region Debug Movement
        if (_frameInput.Move.x > 0)
        {
            //Debug.Log("right");
        }
        if (_frameInput.Move.x < 0)
        {
            //Debug.Log("left");
        }
        if (_frameInput.Move.y > 0)
        {
            //Debug.Log("up");
        }
        if (_frameInput.Move.y < 0)
        {
            //Debug.Log("down");
        }
        #endregion

        if (_frameInput.JumpDown)
        {
            _jumpDownPerformed = true;
            Debug.Log("i jumped");
            _jumpToBeConsumed = true;
            _timeJumpWasPressed = _time;
        }
        if (!_frameInput.JumpHeld) // reset JumpDown bool
        {
            _jumpDownPerformed = false ;
        }
    }

    private void FixedUpdate()
    {
        CheckCollisions();

        HandleHorizontalDirection();
        HandleGravity();
        HandleWallSlide();

        ApplyMovement();
    }

    private void HandleHorizontalDirection()
    {
        if (_frameInput.Move.x == 0) // if no horizontal input, decelerate
        {
            var deceleration = _grounded ? _stats.GroundDeceleration : _stats.AirDeceleration;
            _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
        }
        else // accelerate until max speed is reached
        {
            _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * _stats.MaxSpeed, _stats.Acceleration * Time.fixedDeltaTime);
        }
    }

    private void HandleGravity()
    {
        if (_grounded && _frameVelocity.y <= 0f)
        {
            _frameVelocity.y = _stats.GroundingForce;
        }
        else
        {
            var inAirGravity = _stats.FallAcceleration;
            if (_jumpEndedEarly && _frameVelocity.y > 0) inAirGravity *= _stats.JumpEndEarlyGravityModifier;
            _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_stats.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
        }
    }

    #region Jump
    private bool _jumpToBeConsumed; // I found the original name of the variable to be a bit confusing---basically describes if theres a jump to execute
    private bool _bufferedJumpUsable; // for when jump key is pressed before the jump key is ready to be executed again
    private bool _jumpEndedEarly; // has jump ended early?
    private bool _coyoteUsable; // coyote time (will be very helpful)
    private float _timeJumpWasPressed;

    private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + _stats.JumpBuffer;
    private bool CanUseCoyote => _coyoteUsable && !_grounded && _time < _frameLeftGrounded + _stats.CoyoteTime;
    private void HandleJump()
    {
        if (!_jumpEndedEarly && !_grounded && !_frameInput.JumpHeld && _rb.linearVelocity.y > 0) _jumpEndedEarly = true;
        if (!_jumpToBeConsumed && !HasBufferedJump) return;
        if (_grounded || CanUseCoyote) ExecuteJump();

        _jumpToBeConsumed = false;
    }
    private void ExecuteJump()
    {
        _jumpEndedEarly = false;
        _timeJumpWasPressed = 0;
        _bufferedJumpUsable = false;
        _coyoteUsable = false;
        _frameVelocity.y = _stats.JumpPower;
        Jumped?.Invoke();
    }
    #endregion

    #region Collisions

    private float _frameLeftGrounded = float.MinValue;
    private bool _grounded = true;

    private float _frameLeftWall = float.MinValue;  
    private bool _walled;
    private void CheckCollisions()
    {
        #region ignore
        //Vector3 p1 = transform.position + _col.center + Vector3.up * -_col.height * 0.5f; p1.z= 0f; // point at the bottom (start) of the capsule
        //Vector3 p2 = p1 + Vector3.up * _col.height; p2.z= 0f; // point at the top (end) of the capsule

        //// Ground, Ceiling, Wall
        //bool groundHit = Physics.CapsuleCast(p1, p2, _col.radius, Vector3.down, _stats.GrounderDistance, ~_stats.PlayerLayer);
        //bool ceilingHit = Physics.CapsuleCast(p1, p2, _col.radius, Vector3.up, _stats.GrounderDistance, ~_stats.PlayerLayer);

        //Vector3 p1Wall = p1 + Vector3.up * _col.height / 4; p1Wall.z = 0f;// start point is a quarter of its og height higher
        //Vector3 p2Wall = p2 - Vector3.up * _col.height / 4; p2Wall.z = 0f;// end point is a quarter of its og height lower

        //bool wallHitL = Physics.CapsuleCast(p1Wall, p2Wall, _col.radius, Vector3.left, _stats.WalledDistance, ~_stats.PlayerLayer);
        //bool wallHitR = Physics.CapsuleCast(p1Wall, p2Wall, _col.radius, Vector3.right, _stats.WalledDistance, ~_stats.PlayerLayer);
        //bool wallHit = wallHitL || wallHitR;

        //if (ceilingHit)
        //{
        //    _frameVelocity.y = Mathf.Min(0, _frameVelocity.y); // get the smaller number between 0 and the vertical velocity (so basically set vertical velocity to 0 ig)
        //}

        //if (!_grounded && groundHit) // player touches ground
        //{
        //    Debug.Log("is grounded");

        //    _grounded = true;
        //    _coyoteUsable = true;
        //    _bufferedJumpUsable = true;
        //    _jumpEndedEarly = false;
        //    GroundedChanged?.Invoke(true, Mathf.Abs(_frameVelocity.y));
        //}
        //else if (_grounded && !groundHit) // player leaves ground
        //{
        //    Debug.Log("has left ground");
        //    _grounded = false;
        //    _frameLeftGrounded = _time;
        //    GroundedChanged?.Invoke(false, 0);
        //}

        //if (!_walled && wallHit)
        //{
        //    Debug.DrawCapsule(p1Wall, Quaternion.identity, _col.height / 2, _col.radius, Color.magenta, true);
        //    Debug.Log("has touched wall");

        //    _walled = true;
        //    _coyoteUsable = true;
        //    _bufferedJumpUsable = true;
        //    _jumpEndedEarly = false;
        //    WalledChanged?.Invoke(true, Mathf.Abs(_frameVelocity.x));
        //}
        //else if (_walled && !wallHit)
        //{
        //    Debug.Log("stopped touching wall");
        //    _walled = false;
        //    _frameLeftWall = _time;
        //    WalledChanged?.Invoke(false, 0);
        //}
        #endregion

        _grounded = Physics.CheckSphere(transform.position, 0.5f, ~_stats.PlayerLayer);
        if (_grounded) Debug.Log("wow you're grounded");
    }

    private bool _isWallSliding;

    private bool IsHoldingWall() // checks to see if there's input to hold onto wall (in the correct direction)
    {
        //if (!_walled) return false;
        Vector3 pos = _col.center; pos.z = 0;
        Vector3 dir = GetInput.AimPlayer.normalized; dir.z = 0;// the player's input direction

        Ray ray = new Ray(pos, dir * 5);
            Debug.DrawRay(pos, dir * 5, Color.magenta);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Wall wall = hit.collider.gameObject.GetComponent<Wall>();

            if (wall != null)
            {
                Debug.Log("wall found");
                return true;
            }
        }
        return false;
    }    
    private void HandleWallSlide()
    {
        if (_walled && !IsHoldingWall())
        {
            _isWallSliding = true;
            Debug.Log("player is wall sliding trust me brah");
            // do the actual stuff where the player slides down the wall
        }
        else
        {
            _isWallSliding = false;
        }
    }
    #endregion

    private void ApplyMovement() => _rb.linearVelocity = _frameVelocity;
}
