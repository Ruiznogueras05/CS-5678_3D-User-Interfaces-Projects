//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.3
//     from Assets/h-balloon-selection-Ruiznogueras05CT/Samples/StarterAssets/InputActions/XRC Balloon Selection Input Actions.inputactions
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

public partial class @XRCBalloonSelectionInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @XRCBalloonSelectionInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""XRC Balloon Selection Input Actions"",
    ""maps"": [
        {
            ""name"": ""Balloon Selection"",
            ""id"": ""52f222fe-5c05-4292-b880-0da50593a71a"",
            ""actions"": [
                {
                    ""name"": ""Normalized Radius"",
                    ""type"": ""Value"",
                    ""id"": ""796fb670-dd04-4050-9a6c-86f487823e7b"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""2a597136-0683-47df-bf8f-fe9384265390"",
                    ""path"": ""<XRSimulatedController>{RightHand}/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Normalized Radius"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""91e797db-fc78-4494-8feb-bd9155b72c0f"",
                    ""path"": ""<MetaAimHand>{RightHand}/pinchStrengthIndex"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Normalized Radius"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Balloon Selection
        m_BalloonSelection = asset.FindActionMap("Balloon Selection", throwIfNotFound: true);
        m_BalloonSelection_NormalizedRadius = m_BalloonSelection.FindAction("Normalized Radius", throwIfNotFound: true);
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

    // Balloon Selection
    private readonly InputActionMap m_BalloonSelection;
    private List<IBalloonSelectionActions> m_BalloonSelectionActionsCallbackInterfaces = new List<IBalloonSelectionActions>();
    private readonly InputAction m_BalloonSelection_NormalizedRadius;
    public struct BalloonSelectionActions
    {
        private @XRCBalloonSelectionInputActions m_Wrapper;
        public BalloonSelectionActions(@XRCBalloonSelectionInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @NormalizedRadius => m_Wrapper.m_BalloonSelection_NormalizedRadius;
        public InputActionMap Get() { return m_Wrapper.m_BalloonSelection; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BalloonSelectionActions set) { return set.Get(); }
        public void AddCallbacks(IBalloonSelectionActions instance)
        {
            if (instance == null || m_Wrapper.m_BalloonSelectionActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_BalloonSelectionActionsCallbackInterfaces.Add(instance);
            @NormalizedRadius.started += instance.OnNormalizedRadius;
            @NormalizedRadius.performed += instance.OnNormalizedRadius;
            @NormalizedRadius.canceled += instance.OnNormalizedRadius;
        }

        private void UnregisterCallbacks(IBalloonSelectionActions instance)
        {
            @NormalizedRadius.started -= instance.OnNormalizedRadius;
            @NormalizedRadius.performed -= instance.OnNormalizedRadius;
            @NormalizedRadius.canceled -= instance.OnNormalizedRadius;
        }

        public void RemoveCallbacks(IBalloonSelectionActions instance)
        {
            if (m_Wrapper.m_BalloonSelectionActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IBalloonSelectionActions instance)
        {
            foreach (var item in m_Wrapper.m_BalloonSelectionActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_BalloonSelectionActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public BalloonSelectionActions @BalloonSelection => new BalloonSelectionActions(this);
    public interface IBalloonSelectionActions
    {
        void OnNormalizedRadius(InputAction.CallbackContext context);
    }
}
