using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace cs5678_2024sp.h_go_go.ruiz.ger83
{
    /// <summary>
    /// This component  handles user input and updates the GoGo component accordingly.
    /// </summary>
    public class GoGoInput : MonoBehaviour
    {
       
        [SerializeField] private InputActionProperty m_toggle; // Input action to toggle run state of the GoGo component
        private GoGo goGoScript;
        
        /// <summary>
        /// Input action to toggle run state of the GoGo component. This will call ToggleRun() on GoGo.
        /// </summary>
        public InputActionProperty Toggle
        {
            get => m_toggle;
            set => m_toggle = value;
        }

        private void OnEnable()
        {
            goGoScript = GetComponent<GoGo>();
            m_toggle.action.performed += ToggleRunState;
            Toggle.action.Enable();
        }

        private void OnDisable()
        {
            m_toggle.action.performed -= ToggleRunState;
            m_toggle.action.Disable();
        }

        private void ToggleRunState(InputAction.CallbackContext context)
        {
            if (goGoScript != null)
            {
                goGoScript.ToggleRun();
            }
        }
    }
}