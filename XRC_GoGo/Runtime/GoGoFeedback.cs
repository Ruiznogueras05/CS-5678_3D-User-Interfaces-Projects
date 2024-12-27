using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace cs5678_2024sp.h_go_go.ruiz.ger83
{
    
    /// <summary>
    /// This component is responsible for providing feedback for the GoGo interaction technique. This includes rendering a circle for visualizing the threshold parameter of the technique. This component also visualizes the virtual hand, providing feedback on its position and orientation.
    /// </summary>
    public class GoGoFeedback : MonoBehaviour
    {
        
        
        [Header("Go-Go Feedback Properties")] 
        [SerializeField] private Color m_color = Color.yellow; // the color applied to the circle 
        [SerializeField] private GameObject m_virtualHandPrefab; // 3D model to visualize the virtual hand

        private Material m_yellowCircleMaterial;
        private GoGo m_goGoScript;
        private LineRenderer m_thresholdCircleRenderer; // Fields for the yellow circle
        private const float m_lineThickness = 0.02f;
        private Transform m_playerTransform;
        private MeshRenderer[] m_meshRenderers;
        public GameObject virtualHand;


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// These are all my Properties

        /// <summary>
        /// The color applied to the circle used for threshold feedback. This color is also applied to the material of the game object instantiated from the virtualHandPrefab.
        /// </summary>

        public Color color
        {
            get => m_color;
            set
            {
                m_color = value;
                UpdateMaterialProperties();
            }
        }

        /// <summary>
        /// Prefab used for visualizing the virtual hand, such as a 3D model of a VR controller. The game object instantiated from this prefab provides feedback for virtualHandPosition, a property from GoGo. The alpha value of the game object's material color is controlled by the distance to the real hand position, resulting in a fade-in / fade-out behaviour of the virtual hand feedback object.
        /// </summary>

        public GameObject virtualHandPrefab
        {
            get => m_virtualHandPrefab;
            set => m_virtualHandPrefab = value;
        }

        // Start is called before the first frame update
        private void Awake()
        {
            try
            {
                m_goGoScript = GetComponent<GoGo>();

                m_yellowCircleMaterial = new Material(Shader.Find("Unlit/Color")) { color = m_color };

                if (m_virtualHandPrefab != null)
                {
                    //Instantiate the virtual hand prefab and store the reference
                    virtualHand = Instantiate(m_virtualHandPrefab, transform.parent.parent);

                    m_meshRenderers = virtualHand.GetComponentsInChildren<MeshRenderer>();
                    foreach (MeshRenderer renderer in m_meshRenderers)
                    {
                        renderer.material.color = m_color;
                    }

                    virtualHand.transform.position = m_goGoScript.realHandPosition;

                    virtualHand.SetActive(true);

                }

                CreateThresholdCircle(m_goGoScript.Threshold);
            }
            catch (Exception e)
            {
                Debug.LogError("Exception in Awake" + e);
            }

        }

        // Update is called once per frame
        private void Update()
        {
            try
            {
                if (m_goGoScript != null)
                {
                    bool isRunning = m_goGoScript.isRunning;
                    virtualHand.SetActive(isRunning);
                    m_thresholdCircleRenderer.gameObject.SetActive(isRunning);

                    if (isRunning)
                    {
                        virtualHand.transform.position = m_goGoScript.virtualHandPosition;
                        virtualHand.transform.rotation = m_goGoScript.virtualHandRotation;

                        UpdateThresholdCircle(m_goGoScript.Threshold);
                        UpdateAlpha();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Exception in Update: " + e);
            }
        }

        private void CreateThresholdCircle(float threshold)
        {
            try
            {
                GameObject circleObject = new GameObject("ThresholdCircle");
                circleObject.transform.SetParent(this.transform, false);

                m_thresholdCircleRenderer = circleObject.AddComponent<LineRenderer>();
                m_thresholdCircleRenderer.material = m_yellowCircleMaterial;
                m_thresholdCircleRenderer.startWidth = m_lineThickness;
                m_thresholdCircleRenderer.endWidth = m_lineThickness;
                m_thresholdCircleRenderer.startColor = m_color;
                m_thresholdCircleRenderer.endColor = m_color;
                m_thresholdCircleRenderer.loop = true;
                m_thresholdCircleRenderer.useWorldSpace = true;

                UpdateThresholdCircle(threshold);
            }
            catch (Exception e)
            {
                Debug.LogError("Exception in CreateThresholdCircle: " + e);
            }

        }

        void UpdateThresholdCircle(float threshold)
        {
            try
            {

                int segments = 360;
                m_thresholdCircleRenderer.positionCount = segments;
                float deltaTheta = (2f * Mathf.PI) / segments;
                float theta = 0f;

                Vector3[] circlePoints = new Vector3[segments];
                for (int i = 0; i < segments; i++)
                {
                    float x = m_goGoScript.Origin.x + threshold * Mathf.Cos(theta);
                    float z = m_goGoScript.Origin.z + threshold * Mathf.Sin(theta);
                    circlePoints[i] = new Vector3(x, m_goGoScript.Origin.y, z);
                    theta += deltaTheta;
                }

                m_thresholdCircleRenderer.SetPositions(circlePoints);
            }
            catch (Exception e)
            {
                Debug.LogError("Exception in UpdateThresholdCircle: " + e);
            }
        }

        private void UpdateAlpha()
        {
            float distance = Vector3.Distance(m_goGoScript.realHandPosition, m_goGoScript.virtualHandPosition);
            float alpha = Mathf.Clamp01(distance * 2f);
            Color fadeColor = m_color;
            fadeColor.a = alpha;

            foreach (MeshRenderer renderer in m_meshRenderers)
            {
                renderer.material.color = fadeColor;
            }
        }

        private void UpdateMaterialProperties()
        {
            if (m_thresholdCircleRenderer != null)
            {
                m_thresholdCircleRenderer.startColor = m_color;
                m_thresholdCircleRenderer.endColor = m_color;
            }

            if (virtualHand != null)
            {
                Renderer handRenderer = virtualHand.GetComponent<Renderer>();
                if (handRenderer != null)
                {
                    if (handRenderer.material != null)
                    {
                        handRenderer.material.color = m_color;
                    }
                    else
                    {
                        Debug.LogWarning("Material not found on the renderer!");
                    }
                }
            }
        }
    }
}