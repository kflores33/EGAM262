using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEditor;
using UnityEngine;

// referenced scripts from this repo: https://github.com/Matthew-J-Spencer/Ultimate-2D-Controller/blob/main/Scripts/PlayerController.cs
// and this video: https://www.youtube.com/watch?v=O6VX6Ro7EtA&list=TLPQMDQwMzIwMjXkLjMbxR-3Jg&index=2
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] DefaultInputSubscription GetInput;
    Rigidbody rb;

    [SerializeField] LayerMask wallLayer;

    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();
    }

    private bool _jumpToBeConsumed; // I found the original name of the variable to be a bit confusing---basically describes if theres a jump to execute
    private bool _bufferedJumpUsable; // for when jump key is pressed before the jump key is ready to be executed again
    private bool _jumpEndedEarly; // has jump ended early?
    private float _timeJumpWasPressed;

    //private bool _holdWall;
    private bool _isWallSliding;

    private void HandleJump()
    {
        // handles input queuing, whether or not a jump can actually happen
    }
    private void ExecuteJump()
    {

    }

    private void FixedUpdate()
    {

    }

    private bool IsWalled()
    {
        return false;
        // checks if the player has come in contact with a wall (probably using capsule cast but im stupid so im not doing allat rn)
    }
    private void WallSlide()
    {
        if (IsWalled() && !IsHoldingWall())
        {
            _isWallSliding = true;
            // do the actual stuff where the player slides down the wall
        }
        else
        {
            _isWallSliding = false;
        }
    }
    private bool IsHoldingWall() // 
    {
        if (!IsWalled()) return false;
        Vector3 pos = transform.position;
        Vector3 dir = GetInput.AimPlayer.normalized; // the player's input direction

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
}
