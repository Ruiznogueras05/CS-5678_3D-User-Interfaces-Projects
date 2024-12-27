using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace cs5678_2024sp.h_prism.g02
{
    /// <summary>
    /// PrismState is the enum variable that determines whether the interactable is NoMotion, InMotion or FastMotion.
    /// </summary>
    public enum PrismState
    {
        /// <summary>
        /// NoMotion is the state when as you've grabbed the interactable but moved your hand at a velocity lower than MinV which causes the interactable to remain still.
        /// </summary>
        NoMotion,
        
        /// <summary>
        /// ScaledMotion is the state when as you've grabbed the interactable and are moving at a velocity higher than MinV but lower than SC, the movement of the interactable is scaled to move slower than the hand.
        /// </summary>
        ScaledMotion,
        
        /// <summary>
        /// OneToOneMotion is the state when as you've grabbed the interactable and are moving at a velocity higher than SC but lower than MaxV, the movement of the interactable is one to one with that of the hand.
        /// </summary>
        OneToOneMotion,
        
        /// <summary>
        /// FastMotion is the state when as you've grabbed the interactable and are moving your hand at a velocity greater than MaxV then the interactable in any current position is immediately jumped to the hand's current position.
        /// </summary>
        FastMotion
    }
    
    /// <summary>
    /// This component implements the logic of the Prism interaction technique as presented in the original paper.
    ///
    /// Original Paper:
    ///
    /// Frees, Scott, and G. Drew Kessler. "Precise and rapid interaction through scaled manipulation in immersive virtual environments." IEEE Proceedings. VR 2005. Virtual Reality, 2005.. IEEE, 2005.
    ///
    /// https://ieeexplore.ieee.org/abstract/document/1492759
    /// 
    /// </summary>
    public class Prism : MonoBehaviour
    {
        [SerializeField] private float m_SC;
        [SerializeField] private XRBaseInteractor m_Interactor;
        [SerializeField] private float m_MinV;
        [SerializeField] private float m_MaxV;
        
        private PrismState m_PrismStateX;
        private PrismState m_PrismStateY;
        private PrismState m_PrismStateZ;
        private GameObject m_StateIndicator;
        private PrismInput m_PrismInput;

        private Vector3 m_PrevPosition;
        private Vector3 m_CurrPosition;

        private Vector3 m_PrevAttachPosition;
        
        /// <summary>
        /// The float representing the scaling constant of the PRISM technique.
        /// </summary>
        public float sc
        {
            get { return m_SC; }
            set { m_SC = value; }
        }
        
        /// <summary>
        /// The interactor responsible for selecting the object of interest.
        /// </summary>
        public XRBaseInteractor interactor
        {
            get { return m_Interactor; }
        }
        
        /// <summary>
        /// The float representing the minimum velocity to move the interactable.
        /// </summary>
        public float minV
        {
            get { return m_MinV; }
            set { m_MinV = value; }
        }
        
        /// <summary>
        /// The float representing the maximum velocity to recover the interactable to the current hand position.
        /// </summary>
        public float maxV
        {
            get { return m_MaxV; }
            set { m_MaxV = value; }
        }
        
        /// <summary>
        /// The current state of the motion in the X dimension.
        /// </summary>
        public PrismState prismStateX
        {
            get { return m_PrismStateX; }
            set { m_PrismStateX = value; }
        }
        
        /// <summary>
        /// The current state of the motion in the Y dimension.
        /// </summary>
        public PrismState prismStateY
        {
            get { return m_PrismStateY; }
            set { m_PrismStateY = value; }
        }
        
        /// <summary>
        /// The current state of the motion in the Z dimension.
        /// </summary>
        public PrismState prismStateZ
        {
            get { return m_PrismStateZ; }
            set { m_PrismStateZ = value; }
        }
        
        // Start is called before the first frame update
        void Start()
        {
            m_PrismInput = GetComponent<PrismInput>();

            m_PrevPosition = m_Interactor.transform.position;
        }

        /// <summary>
        /// This function implements the main formula referenced in the original paper to calculate Delta in for all the x, y and z axis
        /// </summary>
        private float DeltaObject(float vHand, float dHand, String dim)
        {
            var vScaled = Mathf.Min(vHand / m_SC, 1f);
            PrismState state = vScaled < 1f ? PrismState.ScaledMotion : PrismState.OneToOneMotion;
            if (dim == "x")
            {
                m_PrismStateX = state;
            }
            else if (dim == "y")
            {
                m_PrismStateY = state;
            }
            else if (dim == "z")
            {
                m_PrismStateZ = state;
            }
            return vScaled * dHand;
        }
        
        // Update is called once per frame
        void Update()
        {
            m_CurrPosition = m_Interactor.transform.position;
            
            if (m_PrismInput.objectSelected)
            {
                Vector3 delta = m_CurrPosition - m_PrevPosition;

                float deltaTime = Time.deltaTime;
                Vector3 velocity = new Vector3(delta.x / deltaTime,
                                               delta.y / deltaTime,
                                               delta.z / deltaTime);
                Vector3 absVelocity = new Vector3(MathF.Abs(velocity.x),
                                                  MathF.Abs(velocity.y),
                                                  MathF.Abs(velocity.z));


                Vector3 attachPosition = new Vector3();
                
                if (absVelocity.x > m_MaxV || absVelocity.y > m_MaxV || absVelocity.z > m_MaxV)
                {
                    m_PrismStateX = PrismState.FastMotion;
                    m_PrismStateY = PrismState.FastMotion;
                    m_PrismStateZ = PrismState.FastMotion;
                    attachPosition = m_CurrPosition;
                    m_PrevAttachPosition = m_CurrPosition;
                }
                else
                {
                    if (absVelocity.x < m_MinV )
                    {
                        m_PrismStateX = PrismState.NoMotion;
                        attachPosition.x = m_PrevAttachPosition.x;
                    }
                    else
                    {
                        float attachXDelta = DeltaObject(absVelocity.x, delta.x, "x");
                        attachPosition.x = m_PrevAttachPosition.x + attachXDelta;
                        m_PrevAttachPosition.x = attachPosition.x;
                    }
                
                    if (absVelocity.y < m_MinV )
                    {
                        m_PrismStateY = PrismState.NoMotion;
                        attachPosition.y = m_PrevAttachPosition.y;
                    }
                    else
                    {
                        float attachYDelta = DeltaObject(absVelocity.y, delta.y, "y");
                        attachPosition.y = m_PrevAttachPosition.y + attachYDelta;
                        m_PrevAttachPosition.y = attachPosition.y;
                    }
                
                    if (absVelocity.z < m_MinV )
                    {
                        m_PrismStateZ = PrismState.NoMotion;
                        attachPosition.z = m_PrevAttachPosition.z;
                    }
                    else
                    {
                        float attachZDelta = DeltaObject(absVelocity.z, delta.z, "z");
                        attachPosition.z = m_PrevAttachPosition.z + attachZDelta;
                        m_PrevAttachPosition.z = attachPosition.z;
                    }
                }
                

                m_Interactor.attachTransform.position = attachPosition;


            }
            else
            {
                m_PrevAttachPosition = m_CurrPosition;
                m_Interactor.attachTransform.position = m_CurrPosition;
            }

            m_PrevPosition = m_CurrPosition;
        }
    }
}