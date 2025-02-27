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

    InputMap _input = null;

    private void OnEnable() // subscribe to inputs
    {
        _input = new InputMap();
        _input.PlayerInputDefault.Enable();
        // subscribe to each input
        _input.PlayerInputDefault.JumpInput.performed += SetJump;
        _input.PlayerInputDefault.JumpInput.canceled += SetJump;
    }
    private void OnDisable() // unsubscribe to inputs
    {
        _input.PlayerInputDefault.JumpInput.performed -= SetJump;
        _input.PlayerInputDefault.JumpInput.canceled -= SetJump;

        _input.PlayerInputDefault.Disable();
    }

    void SetJump(InputAction.CallbackContext ctx)
    {
        JumpInput = ctx.started;
    }
}
