# XRC Balloon Selection

![XRC Balloon Selection Demo](media/H-BalloonSelection.gif)

The **XRC Balloon Selection** project adapts the Balloon Selection interaction technique from 2D touch interfaces to the realm of virtual reality (VR), enabling intuitive object selection in 3D environments. Developed during **CS 5678: 3D User Interfaces** at **Cornell Tech**, this project explores innovative interaction models using hand tracking and VR technologies.

---

## Project Description

The **Balloon Selection** technique allows users to interact with objects in 3D space by manipulating a virtual balloon. The balloon's size dynamically adjusts based on the distance between two points, typically the user's fingers. This method, inspired by Benko and Feiner's 2007 work, was implemented to provide an intuitive, low-fatigue selection method for VR applications.

### Key Features
- **Dynamic Balloon Interaction**: Manipulate a virtual balloon to select objects, with size and position determined by user input.
- **Visual and Auditory Feedback**: Real-time visual cues and sound effects enhance user interaction and immersion.
- **Robust Input Processing**: Translates hand tracking gestures into precise balloon movements and object selection.

### Core Components
1. **BalloonSelection**: 
   - Core script for handling the balloon's state (initiation, updates, and resets). 
   - Calculates the balloon's radius and position for accurate interaction.
2. **BalloonSelectionFeedback**: 
   - Provides visual and auditory cues to represent the balloon's status and interactions.
   - Includes sound effects for stretching, selecting, and other actions.
3. **BalloonSelectionInput**: 
   - Captures and processes user input from VR controllers.
   - Adjusts the balloon's size and behavior based on finger positions and gestures.

---

## System Architecture

### Challenges and Solutions
1. **Adapting 2D to 3D**:
   - Transitioning the balloon selection technique from touch surfaces to 3D VR required redefining the interaction model for depth and hand tracking.
   - Addressed complexities like accurate hand position tracking and gesture recognition.
2. **Feedback Mechanisms**:
   - Designed intuitive feedback systems to help users visualize balloon size, interaction thresholds, and object selection.
3. **Interaction Detection**:
   - Implemented precise collision detection and interaction handling for objects in the VR environment.
4. **Performance Optimization**:
   - Ensured smooth performance by optimizing real-time updates of the balloon's properties.
5. **User Input Processing**:
   - Developed a robust system to translate VR controller inputs into meaningful actions for balloon manipulation.

---

## Getting Started

### Prerequisites
- **Hardware**:
  - VR headset (e.g., Oculus Quest 2) with hand tracking enabled.
- **Software**:
  - Unity 2021.3.0 or later.
  - XR Interaction Toolkit installed.

### Steps to Use
1. **Set Up the Environment**:
   - Import the Unity package provided in this repository.
   - Open the `BalloonSelection.unity` scene in Unity.
2. **Run the Scene**:
   - Connect your VR headset and enable Play mode in Unity.
3. **Interact with Objects**:
   - Use finger gestures to manipulate the virtual balloon for selecting objects in the 3D environment.

---

## Results

### Demo
Watch the full demo video showcasing the Balloon Selection technique in VR:
- [**XRC Balloon Selection Demo Video**](https://drive.google.com/file/d/1eEmDpCyMlEAGK2QRO0yxQwZIJsBDsLUE/view?usp=sharing)

### Documentation
Comprehensive details about the project can be found here:
- [**XRC Balloon Selection Documentation**](https://cs5678-2024sp.github.io/h-balloon-selection-Ruiznogueras05CT/index.html)

---

## Reflections and Future Enhancements

### Achievements
- Successfully transitioned the Balloon Selection technique to a VR environment.
- Developed robust feedback mechanisms for enhanced user interaction.
- Delivered a smooth and responsive VR experience.

### Future Enhancements
- **Expanded Interaction Capabilities**: Introduce multi-object selection and advanced balloon manipulation techniques.
- **Enhanced Feedback**: Improve visual and auditory cues for even more intuitive interactions.
- **Cross-Platform Optimization**: Optimize the system for various VR hardware platforms.

---

## Acknowledgments

This project was completed as part of **CS 5678: 3D User Interfaces** at **Cornell Tech** under the guidance of **Harald Haraldsson**. Special thanks to the XR Collaboratory at Cornell Tech for providing the resources and support for this project.
