using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;
using UnityEngine.UIElements;

// referenced scripts from this repo: https://github.com/Matthew-J-Spencer/Ultimate-2D-Controller/blob/main/Scripts/PlayerController.cs
// and this video: https://www.youtube.com/watch?v=O6VX6Ro7EtA&list=TLPQMDQwMzIwMjXkLjMbxR-3Jg&index=2
// this too: https://youtu.be/EOSjfRuh7x4?si=cNBgw-ogSuHQ6lLL
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

    float ColliderInstanceId;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponentInChildren<CapsuleCollider>();

        // Ghost collision prevention
        _col.hasModifiableContacts = true;
        _col.providesContacts = true;
        ColliderInstanceId = _col.GetInstanceID();
        Physics.ContactModifyEventCCD += PreventGhostCollisionCCD;
    }

    #region Ghost Collision Prevention Stuff
    // video referenced: https://youtu.be/GqCyz7aoar8?si=2JDAnBqpCeNPCaRe
    public enum PreventionMode
    {
        Simple,
        None
    }

    public PreventionMode GhostColPreventionMode;
    private void PreventGhostCollisionCCD(PhysicsScene scene, NativeArray<ModifiableContactPair> contactPairs)
    {
        ModifiableContactPair[] playerContactPairs =
        contactPairs.Where(pair => pair.colliderInstanceID == ColliderInstanceId).ToArray();

        switch (GhostColPreventionMode)
        {
            case PreventionMode.Simple:
                SimpleGhostColPrevention(playerContactPairs); return;
            case PreventionMode.None:
                default:
                break;
        }
    }

    private void SimpleGhostColPrevention(ModifiableContactPair[] playerContactPairs)
    {
        foreach (ModifiableContactPair pair in playerContactPairs)
        {
            for (int i = 0; i < pair.contactCount; i++)
            {
                if (pair.GetSeparation(i) > 0) // if the distance between the colliders is greater than 0, pverride the normal of the object
                {
                    pair.SetNormal(i, Vector3.up);
                }
            }
        }
    }
    #endregion

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
            //Debug.Log("i jumped");
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
        HandleJump();

        ApplyMovement();
    }

    private void HandleHorizontalDirection()
    {
        _lastPos = transform.position;

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
        
        HandleWallSlide();
    }

    #region Collisions

    private float _frameLeftGrounded = float.MinValue;
    private bool _grounded;

    private float _frameLeftWall = float.MinValue;  
    private bool _walled;

    private Vector3 _lastPos;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Vector3 boxDimensions = new Vector3(1.25f, 0.75f, 1);
            //Gizmos.DrawWireCube(transform.position + _col.center, boxDimensions);
    }
    private void CheckCollisions()
    {
        Vector3 p1 = transform.position + _col.center + Vector3.up * (-_col.height * 0.5f + _col.radius); /* half of collider height + radius */ p1.z = 0f; // point at the bottom (start) of the capsule
        Vector3 p2 = p1 + Vector3.up * (_col.height - _col.radius*2.0f); p2.z = 0f; // point at the top (end) of the capsule

        // Ground, Ceiling
        bool groundHit = Physics.OverlapCapsule(p1 - new Vector3(0.0f, _stats.GrounderDistance, 0.0f), p2, _col.radius*0.95f, ~_stats.PlayerLayer).Length > 0;
        bool ceilingHit = Physics.OverlapCapsule(p1, p2 + new Vector3(0.0f, _stats.GrounderDistance, 0.0f), _col.radius * 0.95f, ~_stats.PlayerLayer).Length > 0;

        #region Cieling Hit & Grounded check
        // cieling hit check
        if (ceilingHit)
        {
            _frameVelocity.y = Mathf.Min(0, _frameVelocity.y); // get the smaller number between 0 and the vertical velocity (so basically set vertical velocity to 0 ig)
        }

        // grounded check
        if (!_grounded && groundHit) // player touches ground
        {
            //Debug.Log("is grounded");

            _grounded = true;
            _coyoteUsable = true;
            _bufferedJumpUsable = true;
            _jumpEndedEarly = false;
            GroundedChanged?.Invoke(true, Mathf.Abs(_frameVelocity.y));
        }
        else if (_grounded && !groundHit) // player leaves ground
        {
            //Debug.Log("has left ground");
            _grounded = false;
            _frameLeftGrounded = _time;
            GroundedChanged?.Invoke(false, 0);
        }
        #endregion

        // Wall
            Vector3 rayP1 = transform.position + _col.center + Vector3.left * (_col.radius * 1.25f);
            Vector3 rayP2 = rayP1 + Vector3.right * ((_col.radius * 1.25f) * 2); // radius of collider * 1.25 * 2 (two times the length of slightly wider than radius of collider)
                //Debug.DrawLine(rayP1, rayP2, Color.magenta, 0.5f);
        // actual check
        Vector3 boxDimensions = new Vector3(0.505f, 0.5f, 0.5f);
        bool wallHit = Physics.OverlapBox(transform.position + _col.center, boxDimensions, Quaternion.identity, ~_stats.PlayerLayer).Length > 0;

        if (Physics.Linecast(rayP1, rayP2, out RaycastHit hitInfo, ~_stats.PlayerLayer))
        {
            if (_grounded) return;

            _canWallJump = true;
            _wallJumpVector = hitInfo.normal * _stats.MaxSpeed;
            //Debug.DrawRay(hitInfo.point, hitInfo.normal, Color.blue, 1.25f);
        }
        else if(Physics.Linecast(rayP2, rayP1, out RaycastHit hitInfo2, ~_stats.PlayerLayer))
        {
            if (_grounded) return;

            _canWallJump = true;
            _wallJumpVector = hitInfo2.normal * _stats.MaxSpeed;
            //Debug.DrawRay(hitInfo2.point, hitInfo2.normal, Color.blue, 1.25f);
        }

        // walled check
        if (!_walled && wallHit)
        {
            //Debug.Log("has touched wall");
            _walled = true;
            _coyoteUsable = true;
            _bufferedJumpUsable = true;
            _jumpEndedEarly = false;
            WalledChanged?.Invoke(true, Mathf.Abs(_frameVelocity.x));
        }
        else if (_walled && !wallHit)
        {
            //Debug.Log("stopped touching wall");
            _walled = false;
            _frameLeftWall = _time;
            WalledChanged?.Invoke(false, 0);
        }

        if (_walled && IsHoldingWall() && !_grounded) _rb.isKinematic = true;
        else { _rb.isKinematic = false; }

        // unity discussion (fixing inconsistent raycast normals returned by boxcast/overlapbox): https://discussions.unity.com/t/dealing-with-raycast-corner-normals/765598/2
        // can wall jump
    }

    private bool IsHoldingWall() // checks to see if there's input to hold onto wall (in the correct direction)
    {
        //if (!_walled) return false;
        Vector3 pos = transform.position + _col.center; pos.z = 0;
        Vector3 dir = GetInput.AimPlayer.normalized; dir.z = 0;// the player's input direction

        Ray ray = new Ray(pos, dir * 0.6f);
            Debug.DrawRay(pos, dir * 0.6f, Color.magenta);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Wall wall = hit.collider.gameObject.GetComponentInParent<Wall>();

            if (wall != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    private void HandleWallSlide()
    {
        if (_walled && !IsHoldingWall() && !_grounded)
        {
            _frameVelocity.y = Mathf.Clamp(_frameVelocity.y, -_stats.WallSlideSpeed, float.MaxValue);
        }
    }
    #endregion

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
        if(_walled && _canWallJump) ExecuteWallJump();
        else if (_grounded /*|| CanUseCoyote*/) ExecuteJump();

        // special case: if hitting a wall right after jumping from the ground, ignore wall..? idk just do something to make it stop sticking

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

    bool _canWallJump;
    Vector3 _wallJumpVector;
    private void ExecuteWallJump() 
    {
        // add force in direction from _lastWalledPos
        _jumpEndedEarly = false;
        _timeJumpWasPressed = 0;
        _bufferedJumpUsable = false;
        _coyoteUsable = false;

        _frameVelocity.y = 0;
        _frameVelocity.Normalize();
        _frameVelocity = _wallJumpVector;
        _frameVelocity.y = _stats.JumpPower;
    }
    #endregion

    private void ApplyMovement() => _rb.linearVelocity = _frameVelocity;
}
