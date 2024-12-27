using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cs5678_2024sp.h_prism.g02
{
    /// <summary>
    /// This component handles user input and updates the toggling feature of the object selection.
    /// </summary>
    public class PrismInput : MonoBehaviour
    {
        [SerializeField] private bool m_ObjectSelected;

        /// <summary>
        /// Bool variable that represents if the object has been selected.
        /// </summary>
        public bool objectSelected
        {
            get { return m_ObjectSelected; }
        }
        
        
        // Start is called before the first frame update.
        void Start()
        {
            m_ObjectSelected = false;
        }
        
        /// <summary>
        /// This method toggles the bool variable that represents if the object has been selected or not. 
        /// </summary>
        public void ToggleSelected()
        {
            m_ObjectSelected = !m_ObjectSelected;
        }
    }
}
