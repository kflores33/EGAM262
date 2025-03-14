//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.11.2
//     from Assets/InputSystem/InputMap.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @InputMap: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputMap()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputMap"",
    ""maps"": [
        {
            ""name"": ""PlayerInputDefault"",
            ""id"": ""9c32aa5a-139b-41ba-bf60-eeec16ff13ec"",
            ""actions"": [
                {
                    ""name"": ""JumpInput"",
                    ""type"": ""Button"",
                    ""id"": ""d9812f27-6fbc-40d5-9d4c-1985f2d84f54"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""GrappleShootInput"",
                    ""type"": ""Button"",
                    ""id"": ""87ba570c-778f-459e-a11a-5fb9fd362370"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""GrappleRetractInput"",
                    ""type"": ""Button"",
                    ""id"": ""a2c4fa50-39e4-4f7a-8113-de43a94e598f"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""GrappleAimInput"",
                    ""type"": ""Value"",
                    ""id"": ""5059688e-cb31-4720-8d22-72a674427ca6"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""AimPlayer"",
                    ""type"": ""Value"",
                    ""id"": ""8515b246-4a46-4984-a36c-5d0cdc01b556"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a6fc145f-2e96-4d80-a970-d6fbe34e4572"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""JumpInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bf41ef21-b8a6-4218-ac47-8993bfab4603"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""JumpInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fbaa3f19-f09f-4e45-b174-09553bc27423"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""JumpInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0635d48c-b0ad-4144-8037-c63a0e8306aa"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""JumpInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0bc21444-42e6-425f-9146-37e07fdfd578"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GrappleShootInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""24f6e2d5-9a04-4cc3-8641-d6d333534955"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GrappleShootInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9632333b-bbdb-43bf-bebe-fd1949fe49f4"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GrappleRetractInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""08f9bd3f-9dcb-40e0-bd4e-89e3afcacb5e"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GrappleRetractInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3f4d347d-a0a2-4cce-a239-181211758696"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GrappleAimInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""AnalogInput [Gamepad]"",
                    ""id"": ""3cdad58d-3d4c-4469-8587-265cfc33e503"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GrappleAimInput"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Up"",
                    ""id"": ""e5714de2-b159-4690-bc19-7c726d3bc915"",
                    ""path"": ""<Gamepad>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GrappleAimInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Down"",
                    ""id"": ""d219883c-3303-41a6-9c59-1e9c9df58fbc"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GrappleAimInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left"",
                    ""id"": ""c3165501-6eb7-45e0-8cbd-4295d18de9fd"",
                    ""path"": ""<Gamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GrappleAimInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Right"",
                    ""id"": ""2f5886e1-6d71-4359-bc1e-bc525f5e7efe"",
                    ""path"": ""<Gamepad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GrappleAimInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector WASD"",
                    ""id"": ""3300578c-18df-4b86-8eb7-ce161dc0bccb"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AimPlayer"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6bc2e101-2fee-4bf5-9f68-815ce16f3edd"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AimPlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ab8cca7e-823c-4b87-a062-90c589fe26b3"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AimPlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""0703e8e1-c255-4fb4-b9ed-5981e35f80d8"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AimPlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d13bf0d0-f065-4c2e-9a1f-8a6e8eac8e5f"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AimPlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""6b129015-6072-4d7a-bdc1-a610f65a9f45"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AimPlayer"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""929c7020-eb11-46cb-af97-0590d4dc857e"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AimPlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""32c0bd39-9f98-45bb-a328-ac1570be2a89"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AimPlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""f5cce0e7-467e-4019-9194-7f5395ebd882"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AimPlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""2388d33b-f16f-4223-9840-d0d7f59d16d9"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AimPlayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerInputDefault
        m_PlayerInputDefault = asset.FindActionMap("PlayerInputDefault", throwIfNotFound: true);
        m_PlayerInputDefault_JumpInput = m_PlayerInputDefault.FindAction("JumpInput", throwIfNotFound: true);
        m_PlayerInputDefault_GrappleShootInput = m_PlayerInputDefault.FindAction("GrappleShootInput", throwIfNotFound: true);
        m_PlayerInputDefault_GrappleRetractInput = m_PlayerInputDefault.FindAction("GrappleRetractInput", throwIfNotFound: true);
        m_PlayerInputDefault_GrappleAimInput = m_PlayerInputDefault.FindAction("GrappleAimInput", throwIfNotFound: true);
        m_PlayerInputDefault_AimPlayer = m_PlayerInputDefault.FindAction("AimPlayer", throwIfNotFound: true);
    }

    ~@InputMap()
    {
        UnityEngine.Debug.Assert(!m_PlayerInputDefault.enabled, "This will cause a leak and performance issues, InputMap.PlayerInputDefault.Disable() has not been called.");
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // PlayerInputDefault
    private readonly InputActionMap m_PlayerInputDefault;
    private List<IPlayerInputDefaultActions> m_PlayerInputDefaultActionsCallbackInterfaces = new List<IPlayerInputDefaultActions>();
    private readonly InputAction m_PlayerInputDefault_JumpInput;
    private readonly InputAction m_PlayerInputDefault_GrappleShootInput;
    private readonly InputAction m_PlayerInputDefault_GrappleRetractInput;
    private readonly InputAction m_PlayerInputDefault_GrappleAimInput;
    private readonly InputAction m_PlayerInputDefault_AimPlayer;
    public struct PlayerInputDefaultActions
    {
        private @InputMap m_Wrapper;
        public PlayerInputDefaultActions(@InputMap wrapper) { m_Wrapper = wrapper; }
        public InputAction @JumpInput => m_Wrapper.m_PlayerInputDefault_JumpInput;
        public InputAction @GrappleShootInput => m_Wrapper.m_PlayerInputDefault_GrappleShootInput;
        public InputAction @GrappleRetractInput => m_Wrapper.m_PlayerInputDefault_GrappleRetractInput;
        public InputAction @GrappleAimInput => m_Wrapper.m_PlayerInputDefault_GrappleAimInput;
        public InputAction @AimPlayer => m_Wrapper.m_PlayerInputDefault_AimPlayer;
        public InputActionMap Get() { return m_Wrapper.m_PlayerInputDefault; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerInputDefaultActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerInputDefaultActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerInputDefaultActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerInputDefaultActionsCallbackInterfaces.Add(instance);
            @JumpInput.started += instance.OnJumpInput;
            @JumpInput.performed += instance.OnJumpInput;
            @JumpInput.canceled += instance.OnJumpInput;
            @GrappleShootInput.started += instance.OnGrappleShootInput;
            @GrappleShootInput.performed += instance.OnGrappleShootInput;
            @GrappleShootInput.canceled += instance.OnGrappleShootInput;
            @GrappleRetractInput.started += instance.OnGrappleRetractInput;
            @GrappleRetractInput.performed += instance.OnGrappleRetractInput;
            @GrappleRetractInput.canceled += instance.OnGrappleRetractInput;
            @GrappleAimInput.started += instance.OnGrappleAimInput;
            @GrappleAimInput.performed += instance.OnGrappleAimInput;
            @GrappleAimInput.canceled += instance.OnGrappleAimInput;
            @AimPlayer.started += instance.OnAimPlayer;
            @AimPlayer.performed += instance.OnAimPlayer;
            @AimPlayer.canceled += instance.OnAimPlayer;
        }

        private void UnregisterCallbacks(IPlayerInputDefaultActions instance)
        {
            @JumpInput.started -= instance.OnJumpInput;
            @JumpInput.performed -= instance.OnJumpInput;
            @JumpInput.canceled -= instance.OnJumpInput;
            @GrappleShootInput.started -= instance.OnGrappleShootInput;
            @GrappleShootInput.performed -= instance.OnGrappleShootInput;
            @GrappleShootInput.canceled -= instance.OnGrappleShootInput;
            @GrappleRetractInput.started -= instance.OnGrappleRetractInput;
            @GrappleRetractInput.performed -= instance.OnGrappleRetractInput;
            @GrappleRetractInput.canceled -= instance.OnGrappleRetractInput;
            @GrappleAimInput.started -= instance.OnGrappleAimInput;
            @GrappleAimInput.performed -= instance.OnGrappleAimInput;
            @GrappleAimInput.canceled -= instance.OnGrappleAimInput;
            @AimPlayer.started -= instance.OnAimPlayer;
            @AimPlayer.performed -= instance.OnAimPlayer;
            @AimPlayer.canceled -= instance.OnAimPlayer;
        }

        public void RemoveCallbacks(IPlayerInputDefaultActions instance)
        {
            if (m_Wrapper.m_PlayerInputDefaultActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerInputDefaultActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerInputDefaultActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerInputDefaultActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerInputDefaultActions @PlayerInputDefault => new PlayerInputDefaultActions(this);
    public interface IPlayerInputDefaultActions
    {
        void OnJumpInput(InputAction.CallbackContext context);
        void OnGrappleShootInput(InputAction.CallbackContext context);
        void OnGrappleRetractInput(InputAction.CallbackContext context);
        void OnGrappleAimInput(InputAction.CallbackContext context);
        void OnAimPlayer(InputAction.CallbackContext context);
    }
}
