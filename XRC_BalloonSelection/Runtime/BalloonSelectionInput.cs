using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace cs5678_2024sp.h_balloon_selection.ruiz.ger83
{
    /// <summary>
    /// This component handles user input and updates the BalloonSelection component accordingly.
    /// </summary>
    public class BalloonSelectionInput : MonoBehaviour
    {
        [SerializeField] private InputActionProperty m_normalizedRadius;
        private BalloonSelection m_BalloonSelection;
        
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// These are all my Properties
        
        ///<summary>
        /// Input action property for gathering user input to change the balloon radius. The input action provides a value between 0-1.
        /// </summary>
        public InputActionProperty normalizedRadius
        {
            get => m_normalizedRadius;
            set => m_normalizedRadius = value;
        }
        
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// These are all the methods
        ///
        
        // Start is called before the first frame update
        private void Start()
        {
            m_BalloonSelection = GetComponent<BalloonSelection>();
            m_normalizedRadius.action.Enable();
        }

        // Update is called once per frame
        private void Update()
        {
            float radiusValue = m_normalizedRadius.action.ReadValue<float>();

            float invertedPinch = 1 - radiusValue;
            m_BalloonSelection.SetNormalizedRadius(invertedPinch);
            
        }
        
    }
}