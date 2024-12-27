using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace cs5678_2024sp.h_balloon_selection.ruiz.ger83
{
    /// <summary>
    /// This component implements the logic for the Balloon Selection interaction technique as presented in the original paper, with modifications as outlined below.
    ///
    /// The package differs from the original paper in several ways, primarily due to the fact that the original technique was implemented using a touch surface, while this package uses hand tracking:
    ///
    ///   Clutching: The package does not implement clutching
    ///
    ///   One-handed operation: The package does not implement one-handed operation
    /// 
    ///   Assignment of “anchor” and “stretching”: The paper sets the first surface touch as the anchor, and the second one as the stretching; this gives the user flexibility of choosing the role of each hand. The package assigns the “anchor” and “stretching” roles to objects before runtime.
    ///
    ///   Reset: The package implements a reset feature that allows the user to reset by flipping their hand over
    ///
    /// Original paper:
    /// Benko, H., and S. Feiner. 2007. “Balloon Selection: A Multi-Finger Technique for Accurate Low-Fatigue 3D Selection.” In 2007 IEEE Symposium on 3D User Interfaces. https://doi.org/10.1109/3DUI.2007.340778.
    /// </summary>
    public class BalloonSelection : MonoBehaviour
    {
        [SerializeField] private XRBaseInteractor m_interactor; //The interactor responsible for selecting the object of interest. The interactor's attach transform and sphere collider are modified based on the balloonPosition and balloonRadius properties. 
        [SerializeField] private Transform m_anchor; //The transform of the anchor game object. This corresponds to the "anchor finger" mentioned in the original paper.
        [SerializeField] private Transform m_stretching; //The transform of the stretching game object. This corresponds to the "stretching finger" mentioned in the original paper.
        [SerializeField] private float m_contactThreshold; //Threshold for initiating the balloon selection technique. When the distance between anchor and stretching is below this threshold, the technique can be initiated. This allows the user to bring together the two game objects (for example fingers) to initiate the technique.
        [SerializeField] private float m_minBalloonRadius; //The minimum balloon radius.
        [SerializeField] private float m_maxBalloonRadius; //The maximum balloon radius.

        private Vector3 m_balloonPosition; //The world position of the balloon.
        private float m_balloonRadius; //The radius of the balloon. This sets the radius of the interactor's sphere collider. For setting the balloon radius, use SetNormalizedRadius(Single).
        private BalloonState m_balloonState; //The current state of the technique.
        private float m_normalizedBalloonRadius; //The normalized balloon radius (between 0 to 1), which represents the balloon radius as a value between minBalloonRadius and maxBalloonRadius.
       
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// These are all my Properties
        
       /// <summary>
       /// The interactor responsible for selecting the object of interest. The interactor's attach transform and sphere collider are modified based on the balloonPosition and balloonRadius properties.
       /// </summary>
        public XRBaseInteractor interactor
        {
            get => m_interactor; 
            set => m_interactor = value;
        }
        
        /// <summary>
        /// The transform of the anchor game object. This corresponds to the "anchor finger" mentioned in the original paper.
        /// </summary>
        public Transform anchor
        {
            get => m_anchor;
            set => m_anchor = value;
        }
        
        /// <summary>
        /// The transform of the stretching game object. This corresponds to the "stretching finger" mentioned in the original paper.
        /// </summary>
        public Transform stretching
        {
            get => m_stretching;
            set => m_stretching = value;
        }
        
        /// <summary>
        /// Threshold for initiating the balloon selection technique. When the distance between anchor and stretching is below this threshold, the technique can be initiated. This allows the user to bring together the two game objects (for example fingers) to initiate the technique.
        /// </summary>
        public float contactThreshold
        {
            get => m_contactThreshold;
            set => m_contactThreshold = value;
        }
        
        /// <summary>
        /// The minimum balloon radius.
        /// </summary>
        public float minBalloonRadius
        {
            get => m_minBalloonRadius;
            set => m_minBalloonRadius = value;
        }
        
        /// <summary>
        /// The maximum balloon radius.
        /// </summary>
        public float maxBalloonRadius
        {
            get => m_maxBalloonRadius;
            set => m_maxBalloonRadius = value;
        }
        
        /// <summary>
        /// The world position of the balloon.
        /// </summary>
        public Vector3 balloonPosition
        {
            get => m_balloonPosition;
        }
        
        /// <summary>
        /// The radius of the balloon. This sets the radius of the interactor's sphere collider. For setting the balloon radius, use SetNormalizedRadius(Single).
        /// </summary>
        public float balloonRadius
        {
            get => m_balloonRadius;
        }
        
        /// <summary>
        /// The state of the selection technique. The field names are derived from the original paper. The sequence of states is as follows: Idle -> InUse .
        /// </summary>
        public enum BalloonState
        {
            Idle,
            InUse,
            Strecthing
        }
        /// <summary>
        /// The current state of the technique.
        /// </summary>
        public BalloonState balloonState
        {
            get => m_balloonState;
        }
        
        /// <summary>
        /// The normalized balloon radius (between 0 to 1), which represents the balloon radius as a value between minBalloonRadius and maxBalloonRadius.
        /// </summary>
        public float normalizedBalloonRadius
        {
            get => m_normalizedBalloonRadius;
        }
        
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// These are all the methods
        
        ///<summary>
        /// Sets the normalized radius of the balloon. The normalized radius determines the balloon radius by mapping between minBalloonRadius and maxBalloonRadius.
        /// </summary>
        
        // Start is called before the first frame update
        void Start()
        {
            // Ensure the interactor has a SphereCollider
            SetupInteractor();

            if (interactor != null)
            {
                interactor.onSelectEntered.AddListener(HandleSelectEntered);
            }
        }

        // Update is called once per frame
        void Update()
        {
            
            switch (m_balloonState)
            {
                case BalloonState.Idle:
                    CheckForInitiation();
                    break;
                case BalloonState.InUse:
                    CheckForReset();
                    // UpdateColliderPosition();
                    break;
            }
        }
        
        //This method checks when the interaction happens and plays the selection sound
        private void HandleSelectEntered(XRBaseInteractable interactable)
        {
            // Find the BalloonSelectionFeedback component and play the selection sound
            BalloonSelectionFeedback feedback = GetComponent<BalloonSelectionFeedback>();
            if (feedback != null)
            {
                feedback.PlaySelectionSound();
            }
        }
        
        //This method sets up the interactor to later interact with the cube
        void SetupInteractor()
        {
            SphereCollider collider = interactor.GetComponent<SphereCollider>();
            if (collider == null) {
                collider = interactor.gameObject.AddComponent<SphereCollider>();
                collider.isTrigger = true;
            }
        }

        //This method is the start of the reset logic
        void CheckForReset()
        {
            if (IsHandFlipped(anchor))
            {
                ResetSelection();
            }
        }
        
        //This is the method that calculates and determines if the hand was flipped to reset 
        bool IsHandFlipped(Transform handTransform)
        {
            // Threshold for considering the hand as flipped.
            // This value can be adjusted based on what feels most natural in VR.
            float threshold = -0.2f;

            // Calculate the dot product.
            float dotProduct = Vector3.Dot(handTransform.up, Vector3.down);

            // Check if the dot product exceeds the threshold.
            return dotProduct > threshold;
        }
        
        //This method is the execution of the reset after the code determined the hand was flipped
        void ResetSelection()
        {
            m_balloonState = BalloonState.Idle;
            SetNormalizedRadius(0f);
        }
        
        //This method checks if the two index fingers collide in order to start the code logic
        void CheckForInitiation()
        {
            if (Vector3.Distance(anchor.position, stretching.position) < contactThreshold)
            {
                m_balloonState = BalloonState.InUse;
            }
        }
        
        /// <summary>
        /// Sets the normalized radius of the balloon. The normalized radius determines the balloon radius by mapping between minBalloonRadius and maxBalloonRadius.
        /// </summary>
        public void SetNormalizedRadius(float normalizedRadius)
        {
            m_normalizedBalloonRadius = Mathf.Clamp(normalizedRadius, 0f, 1f);
            m_balloonRadius = Mathf.Lerp(minBalloonRadius, maxBalloonRadius, normalizedRadius);

            SphereCollider sphereCollider = interactor.GetComponent<SphereCollider>();
            if (sphereCollider != null)
            {
                sphereCollider.radius = m_balloonRadius;
                UpdateColliderPosition();

                BalloonSelectionFeedback feedback = GetComponent<BalloonSelectionFeedback>();
                if (feedback != null)
                {
                    feedback.UpdateBalloonVisualSize(m_balloonRadius);
                }
            }
        }
        
        //This method updates the collider and keeps it in the same size and position as the balloon 
        void UpdateColliderPosition()
        {
            if (interactor == null) return;
            BalloonSelectionFeedback feedback = GetComponent<BalloonSelectionFeedback>();
            SphereCollider sphereCollider = interactor.GetComponent<SphereCollider>();
            
            if (sphereCollider != null && feedback.GetBalloonVisual() != null) {
                
                // Convert balloonVisual's world position to the local space of the interactor (which holds the collider)
                Vector3 localBalloonPos = interactor.transform.InverseTransformPoint(feedback.GetBalloonVisual().transform.position);

                // Update collider's center to match balloon's position in local space
                sphereCollider.center = localBalloonPos;

                // Assuming balloonRadius has already been updated to reflect the current visual size of the balloon
                sphereCollider.radius = balloonRadius;
                
                // Convert localBalloonPos back to world space for setting the attachTransform position
                Vector3 worldBalloonPos = interactor.transform.TransformPoint(localBalloonPos);
                interactor.attachTransform.position = worldBalloonPos;
                interactor.attachTransform.rotation = interactor.transform.rotation;
            }

        }
        
    }
}