// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Controller Generated/Controles.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controles : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controles()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controles"",
    ""maps"": [
        {
            ""name"": ""Deteccion"",
            ""id"": ""cdab4033-259c-40b0-a311-e187e301fdba"",
            ""actions"": [
                {
                    ""name"": ""Left Analog Stick"",
                    ""type"": ""Value"",
                    ""id"": ""bd423c94-3d0a-48d3-bd6f-020ee83f1ec4"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Right Analog Stick"",
                    ""type"": ""Value"",
                    ""id"": ""0cf384d8-853a-40fb-a543-3b2599d033a3"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Start"",
                    ""type"": ""Button"",
                    ""id"": ""1111e178-de56-41d9-9de6-feca3695304b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Y"",
                    ""type"": ""Button"",
                    ""id"": ""a66d7d34-951e-4ecb-9be5-fc35ffc3a86e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""A"",
                    ""type"": ""Button"",
                    ""id"": ""b75f8a04-4834-4b45-9c68-8217c08ec846"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LT"",
                    ""type"": ""Button"",
                    ""id"": ""fdfaec64-959a-4a56-b5bf-dc446d972494"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RT"",
                    ""type"": ""Button"",
                    ""id"": ""9c8e5049-fc39-4c13-8693-17a98bbea462"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""D-pad"",
                    ""type"": ""Value"",
                    ""id"": ""17a2d688-ff05-4f51-a439-06f1df673f54"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""c37474bd-4826-44ea-8841-30fae9e17cb2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LB"",
                    ""type"": ""Button"",
                    ""id"": ""ea5937a5-41e7-41a5-98d0-c933c54e489d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""7600c53c-74bd-4b18-91a9-179d22b7f6fe"",
                    ""path"": ""<XInputController>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left Analog Stick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fcfeddb0-3193-4ecd-9cda-28cceaaa6647"",
                    ""path"": ""<XInputController>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right Analog Stick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fa869073-5953-48bd-893f-bf758244ea9d"",
                    ""path"": ""<XInputController>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Start"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5c1ff740-92d1-4920-85be-0e2ce4391518"",
                    ""path"": ""<XInputController>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Y"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ac5db23a-5f1d-4628-9fff-21b72771ce8f"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""A"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1cb6f5aa-d102-4f64-9d8b-9fb154e76fb4"",
                    ""path"": ""<XInputController>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LT"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""048f27f5-58d1-452f-844c-08a1a75d12c3"",
                    ""path"": ""<XInputController>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RT"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9adfc641-7aa5-4122-966d-aea544082b95"",
                    ""path"": ""<XInputController>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""D-pad"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aeaca719-1b2d-4a57-bec8-e900de9022ec"",
                    ""path"": ""<XInputController>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""05c2ddec-2882-4e09-baa3-e810cefff33d"",
                    ""path"": ""<XInputController>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LB"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Deteccion
        m_Deteccion = asset.FindActionMap("Deteccion", throwIfNotFound: true);
        m_Deteccion_LeftAnalogStick = m_Deteccion.FindAction("Left Analog Stick", throwIfNotFound: true);
        m_Deteccion_RightAnalogStick = m_Deteccion.FindAction("Right Analog Stick", throwIfNotFound: true);
        m_Deteccion_Start = m_Deteccion.FindAction("Start", throwIfNotFound: true);
        m_Deteccion_Y = m_Deteccion.FindAction("Y", throwIfNotFound: true);
        m_Deteccion_A = m_Deteccion.FindAction("A", throwIfNotFound: true);
        m_Deteccion_LT = m_Deteccion.FindAction("LT", throwIfNotFound: true);
        m_Deteccion_RT = m_Deteccion.FindAction("RT", throwIfNotFound: true);
        m_Deteccion_Dpad = m_Deteccion.FindAction("D-pad", throwIfNotFound: true);
        m_Deteccion_Select = m_Deteccion.FindAction("Select", throwIfNotFound: true);
        m_Deteccion_LB = m_Deteccion.FindAction("LB", throwIfNotFound: true);
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

    // Deteccion
    private readonly InputActionMap m_Deteccion;
    private IDeteccionActions m_DeteccionActionsCallbackInterface;
    private readonly InputAction m_Deteccion_LeftAnalogStick;
    private readonly InputAction m_Deteccion_RightAnalogStick;
    private readonly InputAction m_Deteccion_Start;
    private readonly InputAction m_Deteccion_Y;
    private readonly InputAction m_Deteccion_A;
    private readonly InputAction m_Deteccion_LT;
    private readonly InputAction m_Deteccion_RT;
    private readonly InputAction m_Deteccion_Dpad;
    private readonly InputAction m_Deteccion_Select;
    private readonly InputAction m_Deteccion_LB;
    public struct DeteccionActions
    {
        private @Controles m_Wrapper;
        public DeteccionActions(@Controles wrapper) { m_Wrapper = wrapper; }
        public InputAction @LeftAnalogStick => m_Wrapper.m_Deteccion_LeftAnalogStick;
        public InputAction @RightAnalogStick => m_Wrapper.m_Deteccion_RightAnalogStick;
        public InputAction @Start => m_Wrapper.m_Deteccion_Start;
        public InputAction @Y => m_Wrapper.m_Deteccion_Y;
        public InputAction @A => m_Wrapper.m_Deteccion_A;
        public InputAction @LT => m_Wrapper.m_Deteccion_LT;
        public InputAction @RT => m_Wrapper.m_Deteccion_RT;
        public InputAction @Dpad => m_Wrapper.m_Deteccion_Dpad;
        public InputAction @Select => m_Wrapper.m_Deteccion_Select;
        public InputAction @LB => m_Wrapper.m_Deteccion_LB;
        public InputActionMap Get() { return m_Wrapper.m_Deteccion; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DeteccionActions set) { return set.Get(); }
        public void SetCallbacks(IDeteccionActions instance)
        {
            if (m_Wrapper.m_DeteccionActionsCallbackInterface != null)
            {
                @LeftAnalogStick.started -= m_Wrapper.m_DeteccionActionsCallbackInterface.OnLeftAnalogStick;
                @LeftAnalogStick.performed -= m_Wrapper.m_DeteccionActionsCallbackInterface.OnLeftAnalogStick;
                @LeftAnalogStick.canceled -= m_Wrapper.m_DeteccionActionsCallbackInterface.OnLeftAnalogStick;
                @RightAnalogStick.started -= m_Wrapper.m_DeteccionActionsCallbackInterface.OnRightAnalogStick;
                @RightAnalogStick.performed -= m_Wrapper.m_DeteccionActionsCallbackInterface.OnRightAnalogStick;
                @RightAnalogStick.canceled -= m_Wrapper.m_DeteccionActionsCallbackInterface.OnRightAnalogStick;
                @Start.started -= m_Wrapper.m_DeteccionActionsCallbackInterface.OnStart;
                @Start.performed -= m_Wrapper.m_DeteccionActionsCallbackInterface.OnStart;
                @Start.canceled -= m_Wrapper.m_DeteccionActionsCallbackInterface.OnStart;
                @Y.started -= m_Wrapper.m_DeteccionActionsCallbackInterface.OnY;
                @Y.performed -= m_Wrapper.m_DeteccionActionsCallbackInterface.OnY;
                @Y.canceled -= m_Wrapper.m_DeteccionActionsCallbackInterface.OnY;
                @A.started -= m_Wrapper.m_DeteccionActionsCallbackInterface.OnA;
                @A.performed -= m_Wrapper.m_DeteccionActionsCallbackInterface.OnA;
                @A.canceled -= m_Wrapper.m_DeteccionActionsCallbackInterface.OnA;
                @LT.started -= m_Wrapper.m_DeteccionActionsCallbackInterface.OnLT;
                @LT.performed -= m_Wrapper.m_DeteccionActionsCallbackInterface.OnLT;
                @LT.canceled -= m_Wrapper.m_DeteccionActionsCallbackInterface.OnLT;
                @RT.started -= m_Wrapper.m_DeteccionActionsCallbackInterface.OnRT;
                @RT.performed -= m_Wrapper.m_DeteccionActionsCallbackInterface.OnRT;
                @RT.canceled -= m_Wrapper.m_DeteccionActionsCallbackInterface.OnRT;
                @Dpad.started -= m_Wrapper.m_DeteccionActionsCallbackInterface.OnDpad;
                @Dpad.performed -= m_Wrapper.m_DeteccionActionsCallbackInterface.OnDpad;
                @Dpad.canceled -= m_Wrapper.m_DeteccionActionsCallbackInterface.OnDpad;
                @Select.started -= m_Wrapper.m_DeteccionActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_DeteccionActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_DeteccionActionsCallbackInterface.OnSelect;
                @LB.started -= m_Wrapper.m_DeteccionActionsCallbackInterface.OnLB;
                @LB.performed -= m_Wrapper.m_DeteccionActionsCallbackInterface.OnLB;
                @LB.canceled -= m_Wrapper.m_DeteccionActionsCallbackInterface.OnLB;
            }
            m_Wrapper.m_DeteccionActionsCallbackInterface = instance;
            if (instance != null)
            {
                @LeftAnalogStick.started += instance.OnLeftAnalogStick;
                @LeftAnalogStick.performed += instance.OnLeftAnalogStick;
                @LeftAnalogStick.canceled += instance.OnLeftAnalogStick;
                @RightAnalogStick.started += instance.OnRightAnalogStick;
                @RightAnalogStick.performed += instance.OnRightAnalogStick;
                @RightAnalogStick.canceled += instance.OnRightAnalogStick;
                @Start.started += instance.OnStart;
                @Start.performed += instance.OnStart;
                @Start.canceled += instance.OnStart;
                @Y.started += instance.OnY;
                @Y.performed += instance.OnY;
                @Y.canceled += instance.OnY;
                @A.started += instance.OnA;
                @A.performed += instance.OnA;
                @A.canceled += instance.OnA;
                @LT.started += instance.OnLT;
                @LT.performed += instance.OnLT;
                @LT.canceled += instance.OnLT;
                @RT.started += instance.OnRT;
                @RT.performed += instance.OnRT;
                @RT.canceled += instance.OnRT;
                @Dpad.started += instance.OnDpad;
                @Dpad.performed += instance.OnDpad;
                @Dpad.canceled += instance.OnDpad;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @LB.started += instance.OnLB;
                @LB.performed += instance.OnLB;
                @LB.canceled += instance.OnLB;
            }
        }
    }
    public DeteccionActions @Deteccion => new DeteccionActions(this);
    public interface IDeteccionActions
    {
        void OnLeftAnalogStick(InputAction.CallbackContext context);
        void OnRightAnalogStick(InputAction.CallbackContext context);
        void OnStart(InputAction.CallbackContext context);
        void OnY(InputAction.CallbackContext context);
        void OnA(InputAction.CallbackContext context);
        void OnLT(InputAction.CallbackContext context);
        void OnRT(InputAction.CallbackContext context);
        void OnDpad(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
        void OnLB(InputAction.CallbackContext context);
    }
}
