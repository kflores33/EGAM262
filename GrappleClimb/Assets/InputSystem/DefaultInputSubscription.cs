using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// video reference: https://youtu.be/D3LQh_3VMso?si=tkI9T56di-42XOTo
public class DefaultInputSubscription : MonoBehaviour
{
    public bool JumpInput { get; private set; } = false;
    public bool GrappleShootInput { get; private set; } = false;
    public bool GrappleRetractInput { get; set; } = false;
    public Vector2 GrappleAimInput { get; private set; } = Vector2.zero;
    public Vector2 AimPlayer { get; private set; } = Vector2.zero;

    InputMap _input = null;

    private void OnEnable() // subscribe to inputs 
    {
        _input = new InputMap();
        _input.PlayerInputDefault.Enable();

        // by default, these are performed constantly (methinks)
        _input.PlayerInputDefault.GrappleAimInput.performed += SetGrappleAim; // .performed checks constantly?
        _input.PlayerInputDefault.GrappleAimInput.canceled += SetGrappleAim;

        _input.PlayerInputDefault.AimPlayer.performed += SetPlayerAim; 
        _input.PlayerInputDefault.AimPlayer.canceled += SetPlayerAim;
    }
    private void OnDisable() // unsubscribe to inputs
    {
        _input.PlayerInputDefault.GrappleAimInput.performed -= SetGrappleAim;
        _input.PlayerInputDefault.GrappleAimInput.canceled -= SetGrappleAim;

        _input.PlayerInputDefault.AimPlayer.performed -= SetPlayerAim;
        _input.PlayerInputDefault.AimPlayer.canceled -= SetPlayerAim;

        _input.PlayerInputDefault.Disable();
    }

    private void Update() 
    {
        // this stuff is not constantly called (even though its in update) because of WasPressedThisFrame()
        JumpInput = _input.PlayerInputDefault.JumpInput.WasPressedThisFrame();
        GrappleShootInput = _input.PlayerInputDefault.GrappleShootInput.WasPressedThisFrame();
        GrappleRetractInput = _input.PlayerInputDefault.GrappleRetractInput.WasPressedThisFrame();
    }

    //void SetJump(InputAction.CallbackContext ctx)
    //{
    //    JumpInput = ctx.started;
    //}
    //void SetGrappleShoot(InputAction.CallbackContext ctx)
    //{
    //    GrappleShootInput = ctx.started;
    //}
    //void SetGrappleRetract(InputAction.CallbackContext ctx)
    //{
    //    GrappleRetractInput = ctx.started;
    //}
    void SetGrappleAim(InputAction.CallbackContext ctx)
    {
        GrappleAimInput = ctx.ReadValue<Vector2>();
    }
    void SetPlayerAim(InputAction.CallbackContext ctx)
    {
        AimPlayer = ctx.ReadValue<Vector2>();
    }
}
