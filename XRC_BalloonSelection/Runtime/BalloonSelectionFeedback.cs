using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cs5678_2024sp.h_balloon_selection.ruiz.ger83
{
    /// <summary>
    /// This component provides visual and auditory feedback for the BalloonSelection technique.
    /// </summary>
    public class BalloonSelectionFeedback : MonoBehaviour
    {
        [SerializeField] private Material m_balloonMaterial; //Material applied to the balloon and spheres representing the anchor and stretching game objects.
        [SerializeField] private Material m_lineMaterial; //Material for the line renderer used to draw the balloon string.
        [SerializeField] private float m_ratchetThreshold;
        [SerializeField] private AudioClip m_ratchetAudio; //Audio clip to be played for ratchet auditory feedback, as demonstrated in the original paper.
        [SerializeField] private AudioClip m_selectionSound; //Audio clip to be played for selection sound. 

        private BalloonSelection m_balloonSelection;
        private GameObject m_anchorVisual;
        private GameObject m_stretchingVisual;
        private GameObject m_balloonVisual;
        private LineRenderer m_lineRenderer;
        private LineRenderer m_balloonLineRenderer;
        private AudioSource m_audioSource;
        private bool m_balloonActive = false;
        private bool m_balloonCreated = false;
        private float m_initialStretchingDistance = 0f;
        private float m_lastStretchingDistance = 0f;
        private float m_accumulatedDistanceChange = 0f;
        private float m_maxRiseHeight = 1.5f;
      
        
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// These are all my Properties

        ///<summary>
        /// Material applied to the balloon and spheres representing the anchor and stretching game objects.
        /// </summary>
        public Material balloonMaterial
        {
            get => m_balloonMaterial;
        }

        /// <summary>
        /// Material for the line renderer used to draw the balloon string.
        /// </summary>
        public Material lineMaterial
        {
            get => m_lineMaterial;
        }

        /// <summary>
        /// Audio clip to be played for ratchet auditory feedback, as demonstrated in the original paper.
        /// </summary>
        public AudioClip ratchetAudio
        {
            get => m_ratchetAudio;
        }

        /// <summary>
        /// Threshold that defines the change in distance between anchor and stretching needed to trigger playback of the ratchetAudio clip.
        /// </summary>
        public float ratchetThreshold
        {
            get => m_ratchetThreshold;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// These are all the methods

        // Start is called before the first frame update
        void Start()
        {
            m_balloonSelection = GetComponent<BalloonSelection>();
            if (m_balloonSelection == null)
            {
                Debug.LogError("BalloonSelection component not found on this GameObject.");
                return;
            }

            m_audioSource = gameObject.AddComponent<AudioSource>();
            CreateVisuals();

        }

        // Update is called once per frame
        void Update()
        {
            UpdateVisuals();

            if (m_balloonSelection.balloonState == BalloonSelection.BalloonState.InUse)
            {
                if (!m_balloonActive)
                {
                    ActivateBalloonAndLine();
                }
                // Update the stretching line between anchor and stretching points
                m_lineRenderer.enabled = true;
                m_lineRenderer.SetPosition(0, m_balloonSelection.stretching.position);
                m_lineRenderer.SetPosition(1, m_balloonSelection.anchor.position);
                

                // Handle auditory feedback and check for balloon rise conditions
                float currentStretchingDistance = Vector3.Distance(m_balloonSelection.anchor.position, m_balloonSelection.stretching.position);
                HandleStretchingAndSound(currentStretchingDistance);

                // If the balloon is created, update its rise and the line from the anchor to the balloon
                if (m_balloonCreated)
                {
                    RiseBalloon(currentStretchingDistance);
                    m_lineRenderer.positionCount = 3;
                    m_lineRenderer.SetPosition(2, m_balloonVisual.transform.position);

                }
            }
            else if (m_balloonSelection.balloonState == BalloonSelection.BalloonState.Idle && m_balloonActive)
            {
                DeactivateBalloonAndLine();
            }
        }
        
        /// <summary>
        /// This method plays the selection sound downloaded for the project 
        /// </summary>
        public void PlaySelectionSound()
        {
            if (m_selectionSound != null)
            {
                m_audioSource.PlayOneShot(m_selectionSound);
            }
        }
        
        //This is the method that creates the balloon and line renderers 
        private void ActivateBalloonAndLine()
        {
            m_balloonActive = true;
            PlaySelectionSound();
            if (!m_balloonCreated)
            {
                m_balloonVisual = CreateBalloonVisual();
                m_balloonCreated = true;
            }
            m_balloonVisual.SetActive(true); // Ensure the visual is active
            m_balloonVisual.transform.position = m_balloonSelection.anchor.position; // Reset position to anchor
    
            m_lineRenderer.enabled = true;
            // Recalculate or reset initial and last stretching distances here
            float currentStretchingDistance = Vector3.Distance(m_balloonSelection.anchor.position, m_balloonSelection.stretching.position);
            m_initialStretchingDistance = m_lastStretchingDistance = currentStretchingDistance;
        }
        
        //This is the method that deletes all the drawn objects when the reset logic is activated in BalloonSelection.cs
        private void DeactivateBalloonAndLine()
        {
            if (m_balloonCreated)
            {
                m_balloonVisual.SetActive(false); // This makes the balloon disappear visually
            }
            m_lineRenderer.enabled = false;
            m_balloonActive = false; // This deactivates the balloon until it's reactivated
        }
        
        //This method creates the line renderers and the two small spheres in your index fingers
        private void CreateVisuals()
        {
            m_anchorVisual = CreateIndexVisual(m_balloonSelection.anchor, "AnchorVisual");
            m_stretchingVisual = CreateIndexVisual(m_balloonSelection.stretching, "StretchingVisual");
            m_lineRenderer = CreateLineRenderer();
        }

        //This is the method that calculates and determines the size of the spheres in the index fingers
        private GameObject CreateIndexVisual(Transform indexTransform, string objectName)
        {
            var indexVisual = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            indexVisual.name = objectName;
            indexVisual.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
            indexVisual.GetComponent<Renderer>().material = balloonMaterial;
            Destroy(indexVisual.GetComponent<Collider>());
            return indexVisual;
        }
        
        //This is the method that calculates and determines the size and color of the line renderers
        private LineRenderer CreateLineRenderer()
        {
            LineRenderer renderer = gameObject.AddComponent<LineRenderer>();
            renderer.material = lineMaterial;
            renderer.startWidth = 0.005f;
            renderer.endWidth = 0.005f;
            renderer.positionCount = 2;
            renderer.enabled = false;
            return renderer;

        }
        
        //This method calculates and draws the balloon object 
        private GameObject CreateBalloonVisual()
        {
            var balloon = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            balloon.GetComponent<Renderer>().material = balloonMaterial;
            balloon.transform.position = m_balloonSelection.anchor.position;
            balloon.SetActive(true);
            return balloon;
        }
        
        
        /// <summary>
        /// This method is meant to send the balloon's information to BalloonSelection.cs
        /// </summary>
        public GameObject GetBalloonVisual()
        {
            return m_balloonVisual;
        }

        //This method updates the position of the spheres in the index fingers
        private void UpdateVisuals()
        {
            m_anchorVisual.transform.position = m_balloonSelection.anchor.position;
            m_stretchingVisual.transform.position = m_balloonSelection.stretching.position;
            
        }
        
        /// <summary>
        /// This method is what applies the calculated radius to the balloon
        /// </summary>
        public void UpdateBalloonVisualSize(float normalizedRadius)
        {
            if (m_balloonVisual != null)
            {
                m_balloonVisual.transform.localScale = Vector3.one * normalizedRadius;
            }
        }
        
        //This method handles the line renderer stretching out and the sound it plays as its being stretched/compressed 
        private void HandleStretchingAndSound(float currentStretchingDistance)
        {
            // Calculate the difference in stretching distance to determine if ratchet audio should play
            float distanceChange = currentStretchingDistance - m_lastStretchingDistance;
            m_accumulatedDistanceChange += Mathf.Abs(distanceChange);
            if (m_accumulatedDistanceChange > ratchetThreshold)
            {
                m_audioSource.PlayOneShot(ratchetAudio);
                m_accumulatedDistanceChange = 0;
            }
            m_lastStretchingDistance = currentStretchingDistance;
            
        }

        //This is the method that calculates how much the balloon
        private void RiseBalloon(float currentStretchingDistance)
        {
            // Calculate the difference in stretching distance
            float distanceChange = currentStretchingDistance - m_initialStretchingDistance;

            // Use distance change to move the balloon up or down inversely to the stretching
            Vector3 newBalloonPosition = m_balloonVisual.transform.position;
            newBalloonPosition.y -= distanceChange; // Inverting the direction of change

            // Clamp the balloon's Y position to prevent it from going below the anchor or above a max height
            float minHeight = m_balloonSelection.anchor.position.y;
            float maxHeight = minHeight + m_maxRiseHeight;
            newBalloonPosition.y = Mathf.Clamp(newBalloonPosition.y, minHeight, maxHeight);

            // Ensure the X and Z positions are aligned with the anchor to follow its tracking
            newBalloonPosition.x = m_balloonSelection.anchor.position.x;
            newBalloonPosition.z = m_balloonSelection.anchor.position.z;

            // Apply the new position
            m_balloonVisual.transform.position = newBalloonPosition;

            // Update the last stretching distance for the next frame
            m_initialStretchingDistance = currentStretchingDistance;
            
        }
    }
}
      

        