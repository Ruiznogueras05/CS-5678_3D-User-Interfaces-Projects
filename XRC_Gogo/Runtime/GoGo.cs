using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace cs5678_2024sp.h_go_go.ruiz.ger83
{
    
    /// <summary>
    ///This component implements the logic for the Go-Go interaction technique as presented in the original paper. This component holds a reference to an interactor (XR Interaction Toolkit), which will be modified to produce a behaviour identical to the originally proposed technique. The Go-Go mapping function calculates the "virtual hand" position that is applied to the interactor's attach transform and collider.
    ///
    ///Original paper:
    ///
    ///Ivan Poupyrev, Mark Billinghurst, Suzanne Weghorst, and Tadao Ichikawa. 1996. The go-go interaction technique: non-linear mapping for direct manipulation in VR. In Proceedings of the 9th annual ACM symposium on User interface software and technology (UIST '96). Association for Computing Machinery, New York, NY, USA, 79–80. https://doi.org/10.1145/237091.237102.
    /// 
    /// </summary>
    public class GoGo : MonoBehaviour
    {
        
        
        [Header("Go-Go Properties")] 
        
        [SerializeField] private XRBaseInteractor m_interactor; // The interactor for selecting objects
        [SerializeField] private float m_headToChest = 0.3f; // The distance from head (between eyes) to chest

        private bool m_isRunning; // The current state of the technique

        [Header("Go-Go parameters")] 
        [SerializeField] private float m_k = 0.1666667f; // The coefficient for the mapping function 
        [SerializeField] private float m_threshold = 0.3f;
   

        private SphereCollider m_Collider;
        public event Action OnRunStarted;
        public event Action OnRunStopped;


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// These are all my Properties

        /// <summary>
        /// User-provided value that represents the distance from head (between eyes) to chest, used to calculate the origin, as presented in the original paper
        /// </summary>
        public float headToChest
        {
            get => m_headToChest;
            set => m_headToChest = value;
        }

        /// <summary>
        /// The interactor responsible for selecting the object of interest
        /// </summary>
        public XRBaseInteractor interactor
        {
            get => m_interactor;
            set => m_interactor = value;
        }

        /// <summary>
        /// Represents the current state of the technique. If true, then the technique is active and running. If false, then no mapping will be applied to interactor's attach transform and collider.
        /// </summary>
        public bool isRunning => m_isRunning;

        /// <summary>
        /// Coefficient for the mapping function. This corresponds to k in the original paper.
        /// </summary>
        public float K
        {
            get => m_k;
            set => m_k = value;
        }

        /// <summary>
        /// Threshold for the mapping function. This corresponds to D in the original paper.
        /// </summary>
        public float Threshold
        {
            get => m_threshold;
            set => m_threshold = value;
        }

        /// <summary>
        /// This corresponds to the origin in the original paper, where it is defined as the user's chest position. To calculate the origin, we subtract the user-provided headToChest from the position of the user's head, which can be retrieved from the tracked position of the head-mounted display.
        /// </summary>
        public Vector3 Origin { get; private set; }

        /// <summary>
        /// The world position of the real hand.
        /// </summary>
        public Vector3 realHandPosition { get; private set; }

        /// <summary>
        /// The world position of the virtual hand.
        /// </summary>
        public Vector3 virtualHandPosition { get; private set; }
    

        /// <summary>
        /// The rotation of the virtual hand.
        /// </summary>
        public Quaternion virtualHandRotation { get; private set; }
        

        private Transform virtualHandTransform;
        // Start is called before the first frame update
        void Start()
        {
                
                m_Collider = m_interactor.GetComponent<SphereCollider>();
                m_Collider.enabled = true;
        }

        // Update is called once per frame
        void Update()
        {

            if (m_isRunning)
            {
                
                realHandPosition = m_interactor.transform.position;
                Origin = CalculateOrigin();
                CalculateVirtualHandPosition();
                ApplyVirtualHandPosition();

            }
        }

        /// <summary>
        /// This function toggles the running state of the Go-Go technique
        /// </summary>
        public void ToggleRun()
        {
            m_isRunning = !m_isRunning;
            if (m_isRunning)
            {
                OnRunStarted?.Invoke();
            }
            else
            {
                OnRunStopped?.Invoke();
            }
        }


        private Vector3 CalculateOrigin()
        {
            return Camera.main.transform.position - new Vector3(0, headToChest, 0);
        }

        private void CalculateVirtualHandPosition()
        {
            Vector3 realHandDirection = (realHandPosition - Origin);
            float scalingFactor = 100.0f; // Or any other scaling factor you wish to use
            
            // Calculate the magnitude of the direction vector.
            float Rr = realHandDirection.magnitude * scalingFactor;
            float Rv;
            
            // If the real hand's distance is less than the threshold, Rv equals Rr.
            // If the real hand's distance is greater than the threshold, apply the non-linear mapping function.
            if (Rr < Threshold * scalingFactor)
            {
                Rv = Rr;
            
            }
            else
            {
                Rv = Rr + K * Mathf.Pow((Rr - Threshold * scalingFactor), 2);

            }

            // Use the normalized direction for the virtual hand to ensure it starts at the same position as the real hand.
            Vector3 virtualHandDirection = realHandDirection.normalized * (Rv / scalingFactor);

            // return Origin + virtualHandDirection;
            virtualHandPosition = Origin + virtualHandDirection;
            
            // Rotation should match the interactor's rotation.
            virtualHandRotation = m_interactor.transform.rotation;
            
        }

        private void ApplyVirtualHandPosition()
        {
                m_interactor.attachTransform.position = virtualHandPosition;
                m_interactor.attachTransform.rotation = virtualHandRotation;
                m_Collider.center = m_interactor.attachTransform.localPosition;
                
        }


    }
}