using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cs5678_2024sp.h_prism.g02
{
    /// <summary>
    /// This component is responsible for providing feedback for the Prism interaction technique. This includes rendering the line renderer between the hand and the currently selected interactable.
    /// </summary>
    public class PrismFeedback : MonoBehaviour
    {
        [SerializeField] private Material m_NoMotionLineMaterial;
        [SerializeField] private Material m_ScaledMotionLineMaterial;
        [SerializeField] private Material m_OneToOneLineMaterial;
        
        private Prism m_Prism;
        private LineRenderer m_LineRenderer;
        
        private PrismState m_LastStableState = PrismState.NoMotion;
        private int m_StateCount = 0;
        private int m_StateThreshold = 5;
        private PrismState m_AverageState;
        
        /// <summary>
        /// Red material for the line renderer used to draw the string representing the stopped motion.
        /// </summary>
        public Material NoMotionlineMaterial
        {
            get { return m_NoMotionLineMaterial; }
        }
        
        /// <summary>
        /// Yellow material for the line renderer used to draw the string representing the scaled motion.
        /// </summary>
        public Material ScaledMotionlineMaterial
        {
            get { return m_ScaledMotionLineMaterial; }
        }
        
        /// <summary>
        /// Green material for the line renderer used to draw the string representing the one to one motion.
        /// </summary>
        public Material OneToOnelineMaterial
        {
            get { return m_OneToOneLineMaterial; }
        }
        
        // Start is called before the first frame update
        void Start()
        {
            // Get the Prism component attached to this GameObject
            m_Prism = GetComponent<Prism>();

            // Add a LineRenderer component to this GameObject
            m_LineRenderer = gameObject.AddComponent<LineRenderer>();

            // Set LineRenderer properties
            m_LineRenderer.startWidth = 0.005f; // Set the width of the line
            m_LineRenderer.endWidth = 0.005f;
            m_LineRenderer.material = m_NoMotionLineMaterial; // Set the start material
            m_LineRenderer.positionCount = 2;

            // Set LineRenderer positions
            UpdateLineRenderer();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateLineRenderer();
        }
        
        /// <summary>
        /// This method updates the LineRenderer positions to connect m_Prism.interactor.transform.position and m_Prism.interactor.attachTransform.position
        /// </summary>
        private void UpdateLineRenderer()
        {
            // Set LineRenderer positions
            m_LineRenderer.SetPosition(0, m_Prism.interactor.transform.position);
            m_LineRenderer.SetPosition(1, m_Prism.interactor.attachTransform.position);

            if (m_Prism.prismStateX == PrismState.NoMotion && m_Prism.prismStateY == PrismState.NoMotion &&
                m_Prism.prismStateZ == PrismState.NoMotion)
            {
                m_AverageState = PrismState.NoMotion;
            }
            
            else if (m_Prism.prismStateX == PrismState.OneToOneMotion || m_Prism.prismStateY == PrismState.OneToOneMotion ||
                     m_Prism.prismStateZ == PrismState.OneToOneMotion)
            {
                m_AverageState = PrismState.OneToOneMotion;
            }
            
            else if (m_Prism.prismStateX == PrismState.ScaledMotion || m_Prism.prismStateY == PrismState.ScaledMotion ||
                     m_Prism.prismStateZ == PrismState.ScaledMotion)
            {
                m_AverageState = PrismState.ScaledMotion;
            }


            if (m_AverageState == m_LastStableState)
            {
                m_StateCount++;
                if (m_StateCount > m_StateThreshold)
                {
                    // Change material only if the state is stable for more than 'stateThreshold' frames
                    ChangeMaterial(m_AverageState);
                    m_StateCount = 0;
                }
            }
            else
            {
                m_LastStableState = m_AverageState;
                m_StateCount = 0;  // Reset the counter
            }
        }
        
        private void ChangeMaterial(PrismState state)
        {
            switch (state)
            {
                case PrismState.NoMotion:
                    m_LineRenderer.material = m_NoMotionLineMaterial;
                    break;
                case PrismState.OneToOneMotion:
                    m_LineRenderer.material = m_OneToOneLineMaterial;
                    break;
                case PrismState.ScaledMotion:
                    m_LineRenderer.material = m_ScaledMotionLineMaterial;
                    break;
            }
        }
    }
}
