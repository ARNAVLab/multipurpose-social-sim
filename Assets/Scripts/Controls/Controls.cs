//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Scripts/Controls/Controls.inputactions
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

public partial class @Controls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""player"",
            ""id"": ""24bfad34-11c7-480f-b587-c83fa5875411"",
            ""actions"": [
                {
                    ""name"": ""leftMouse"",
                    ""type"": ""Button"",
                    ""id"": ""ad4de3d5-ecef-4efd-bfb9-607566df6751"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""rightMouse"",
                    ""type"": ""Button"",
                    ""id"": ""e856a2e9-3d74-46ca-bb77-aadb96e19f91"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""middleMouse"",
                    ""type"": ""Button"",
                    ""id"": ""a5cdbe46-b6c4-41d0-b944-8e1c6ef12cd8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""mousePosition"",
                    ""type"": ""Value"",
                    ""id"": ""1b6440c8-9b18-4bc5-9771-4b8cef19c350"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""mouseDelta"",
                    ""type"": ""Value"",
                    ""id"": ""fa5dd25e-ed62-4031-ba55-0985764f15e0"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""wasd"",
                    ""type"": ""Value"",
                    ""id"": ""8399f0e4-afe8-4cf3-9ee4-bdc071c4ee95"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""control"",
                    ""type"": ""Button"",
                    ""id"": ""33e298cc-78c0-49bd-96cd-3736623f62ba"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""shift"",
                    ""type"": ""Button"",
                    ""id"": ""dee2757b-3198-44a8-8c3c-8893082467e2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""alt"",
                    ""type"": ""Button"",
                    ""id"": ""ba1d4d0f-dc2f-468a-8088-eafcb2ef583f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""scroll"",
                    ""type"": ""Value"",
                    ""id"": ""173f0cd9-dac9-4e95-adc1-179ca3b4183b"",
                    ""expectedControlType"": ""Delta"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""zoom"",
                    ""type"": ""Value"",
                    ""id"": ""2276ae2c-6f5e-4375-9426-6543f54f1e89"",
                    ""expectedControlType"": ""Integer"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ef11d44c-c5b5-4ddb-a3bf-54e3ce7fb075"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""leftMouse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""62c33b21-4006-44d9-8c46-c7a00a324f1a"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""rightMouse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aa5c191f-a601-4dc6-b28d-d6d38e5daf6e"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""middleMouse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4b2adbf6-9a05-4444-bb5b-d75434246d46"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""control"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a0dd25a6-7f85-4827-a785-5a1649d9b105"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""shift"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""23458ad5-f829-411a-9ec5-e815ecf52e52"",
                    ""path"": ""<Keyboard>/alt"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""alt"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0562570e-e24e-4007-8567-9e95f4bcfd0e"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""mousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2874a856-b345-4e33-b3f8-9ad5d60e102c"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""mouseDelta"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""40026adc-f161-4d8e-bfae-f00bc496f5b2"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""wasd"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""1d7a7d3d-ddfc-4965-8f20-0c949c448a06"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""wasd"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""178fba5f-e40b-4fea-b48f-ef4ac70fb267"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""wasd"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b2ac5d5d-d217-4782-b2b6-cc33d2fe2a84"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""wasd"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""aaa0307b-4159-4640-9406-90c664ce7c46"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""wasd"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""aec494ad-d912-425b-9fb0-71fcf71bb860"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""scroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""137d768c-0c8a-4c49-b646-5b0507a5aba2"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""zoom"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""092ae5e0-315c-403d-8f1e-9124555c9e46"",
                    ""path"": ""<Keyboard>/minus"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""11c70310-6dc9-4561-a1e7-5d83d4506c80"",
                    ""path"": ""<Keyboard>/equals"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // player
        m_player = asset.FindActionMap("player", throwIfNotFound: true);
        m_player_leftMouse = m_player.FindAction("leftMouse", throwIfNotFound: true);
        m_player_rightMouse = m_player.FindAction("rightMouse", throwIfNotFound: true);
        m_player_middleMouse = m_player.FindAction("middleMouse", throwIfNotFound: true);
        m_player_mousePosition = m_player.FindAction("mousePosition", throwIfNotFound: true);
        m_player_mouseDelta = m_player.FindAction("mouseDelta", throwIfNotFound: true);
        m_player_wasd = m_player.FindAction("wasd", throwIfNotFound: true);
        m_player_control = m_player.FindAction("control", throwIfNotFound: true);
        m_player_shift = m_player.FindAction("shift", throwIfNotFound: true);
        m_player_alt = m_player.FindAction("alt", throwIfNotFound: true);
        m_player_scroll = m_player.FindAction("scroll", throwIfNotFound: true);
        m_player_zoom = m_player.FindAction("zoom", throwIfNotFound: true);
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

    // player
    private readonly InputActionMap m_player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_player_leftMouse;
    private readonly InputAction m_player_rightMouse;
    private readonly InputAction m_player_middleMouse;
    private readonly InputAction m_player_mousePosition;
    private readonly InputAction m_player_mouseDelta;
    private readonly InputAction m_player_wasd;
    private readonly InputAction m_player_control;
    private readonly InputAction m_player_shift;
    private readonly InputAction m_player_alt;
    private readonly InputAction m_player_scroll;
    private readonly InputAction m_player_zoom;
    public struct PlayerActions
    {
        private @Controls m_Wrapper;
        public PlayerActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @leftMouse => m_Wrapper.m_player_leftMouse;
        public InputAction @rightMouse => m_Wrapper.m_player_rightMouse;
        public InputAction @middleMouse => m_Wrapper.m_player_middleMouse;
        public InputAction @mousePosition => m_Wrapper.m_player_mousePosition;
        public InputAction @mouseDelta => m_Wrapper.m_player_mouseDelta;
        public InputAction @wasd => m_Wrapper.m_player_wasd;
        public InputAction @control => m_Wrapper.m_player_control;
        public InputAction @shift => m_Wrapper.m_player_shift;
        public InputAction @alt => m_Wrapper.m_player_alt;
        public InputAction @scroll => m_Wrapper.m_player_scroll;
        public InputAction @zoom => m_Wrapper.m_player_zoom;
        public InputActionMap Get() { return m_Wrapper.m_player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @leftMouse.started += instance.OnLeftMouse;
            @leftMouse.performed += instance.OnLeftMouse;
            @leftMouse.canceled += instance.OnLeftMouse;
            @rightMouse.started += instance.OnRightMouse;
            @rightMouse.performed += instance.OnRightMouse;
            @rightMouse.canceled += instance.OnRightMouse;
            @middleMouse.started += instance.OnMiddleMouse;
            @middleMouse.performed += instance.OnMiddleMouse;
            @middleMouse.canceled += instance.OnMiddleMouse;
            @mousePosition.started += instance.OnMousePosition;
            @mousePosition.performed += instance.OnMousePosition;
            @mousePosition.canceled += instance.OnMousePosition;
            @mouseDelta.started += instance.OnMouseDelta;
            @mouseDelta.performed += instance.OnMouseDelta;
            @mouseDelta.canceled += instance.OnMouseDelta;
            @wasd.started += instance.OnWasd;
            @wasd.performed += instance.OnWasd;
            @wasd.canceled += instance.OnWasd;
            @control.started += instance.OnControl;
            @control.performed += instance.OnControl;
            @control.canceled += instance.OnControl;
            @shift.started += instance.OnShift;
            @shift.performed += instance.OnShift;
            @shift.canceled += instance.OnShift;
            @alt.started += instance.OnAlt;
            @alt.performed += instance.OnAlt;
            @alt.canceled += instance.OnAlt;
            @scroll.started += instance.OnScroll;
            @scroll.performed += instance.OnScroll;
            @scroll.canceled += instance.OnScroll;
            @zoom.started += instance.OnZoom;
            @zoom.performed += instance.OnZoom;
            @zoom.canceled += instance.OnZoom;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @leftMouse.started -= instance.OnLeftMouse;
            @leftMouse.performed -= instance.OnLeftMouse;
            @leftMouse.canceled -= instance.OnLeftMouse;
            @rightMouse.started -= instance.OnRightMouse;
            @rightMouse.performed -= instance.OnRightMouse;
            @rightMouse.canceled -= instance.OnRightMouse;
            @middleMouse.started -= instance.OnMiddleMouse;
            @middleMouse.performed -= instance.OnMiddleMouse;
            @middleMouse.canceled -= instance.OnMiddleMouse;
            @mousePosition.started -= instance.OnMousePosition;
            @mousePosition.performed -= instance.OnMousePosition;
            @mousePosition.canceled -= instance.OnMousePosition;
            @mouseDelta.started -= instance.OnMouseDelta;
            @mouseDelta.performed -= instance.OnMouseDelta;
            @mouseDelta.canceled -= instance.OnMouseDelta;
            @wasd.started -= instance.OnWasd;
            @wasd.performed -= instance.OnWasd;
            @wasd.canceled -= instance.OnWasd;
            @control.started -= instance.OnControl;
            @control.performed -= instance.OnControl;
            @control.canceled -= instance.OnControl;
            @shift.started -= instance.OnShift;
            @shift.performed -= instance.OnShift;
            @shift.canceled -= instance.OnShift;
            @alt.started -= instance.OnAlt;
            @alt.performed -= instance.OnAlt;
            @alt.canceled -= instance.OnAlt;
            @scroll.started -= instance.OnScroll;
            @scroll.performed -= instance.OnScroll;
            @scroll.canceled -= instance.OnScroll;
            @zoom.started -= instance.OnZoom;
            @zoom.performed -= instance.OnZoom;
            @zoom.canceled -= instance.OnZoom;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnLeftMouse(InputAction.CallbackContext context);
        void OnRightMouse(InputAction.CallbackContext context);
        void OnMiddleMouse(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
        void OnMouseDelta(InputAction.CallbackContext context);
        void OnWasd(InputAction.CallbackContext context);
        void OnControl(InputAction.CallbackContext context);
        void OnShift(InputAction.CallbackContext context);
        void OnAlt(InputAction.CallbackContext context);
        void OnScroll(InputAction.CallbackContext context);
        void OnZoom(InputAction.CallbackContext context);
    }
}
