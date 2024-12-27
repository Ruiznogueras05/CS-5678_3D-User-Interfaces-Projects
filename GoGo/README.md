# XRC Go-Go

![XRC Go-Go Demo](media/XRC_GoGo.gif)

**XRC Go-Go** is an immersive virtual reality (VR) interaction technique that allows users to interact with objects beyond their physical reach by extending their virtual hand using the **Go-Go technique**. This project was developed as part of **CS 5678: 3D User Interfaces** at **Cornell Tech** during Spring 2024. Utilizing **Unity** and the **XR Interaction Toolkit**, the system blends advanced VR interaction mechanics with intuitive hand-tracking capabilities.

---

## Project Description

The Go-Go interaction technique extends the user’s physical reach into virtual environments, enabling precise object manipulation across distances. This is achieved through a **non-linear mapping** between real-world hand motion and the corresponding virtual hand movements. The system provides real-time feedback and visual cues, ensuring a seamless and intuitive experience.

### Key Features
- **Virtual Hand Extension**: Implements the Go-Go technique to dynamically extend the user’s virtual hand for distant interactions.
- **Collision Detection**: Utilizes sphere colliders to enable accurate object interaction.
- **Visual Feedback**: Highlights interaction thresholds and displays virtual hand positions for clarity.
- **Interaction Manager**: Handles the lifecycle of interactions, including hover, select, and activate states.
- **XR Toolkit Integration**: Seamlessly integrates with Unity's XR Interaction Toolkit for enhanced VR functionality.

---

## System Architecture

### Components
1. **Virtual Hand Prefab**: Represents the user’s hand in the virtual environment.
2. **Interactor**: XR component managing interactions with virtual objects.
3. **Sphere Collider**: Enables collision detection for interaction.
4. **Interaction Manager**: Manages hover, select, and activation states of interactions.
5. **Go-Go Script**: Implements the Go-Go technique for virtual hand movement and rotation.
6. **Go-Go Feedback Script**: Provides visual indicators for interaction thresholds.
7. **Go-Go Input Script**: Toggles the Go-Go system on or off.

---

## Challenges and Solutions

### Synchronization of Virtual and Real Hands
- **Challenge**: Aligning the movement and rotation of the virtual hand with the user’s real hand.
- **Solution**: Ensured accurate synchronization by initializing the virtual hand at the real hand’s position when toggling the system on.

### Collider Interaction
- **Challenge**: Making the sphere collider follow the virtual hand precisely.
- **Solution**: Debugged collider positioning and activation issues to achieve accurate object interaction.

### Performance and Optimization
- **Challenge**: Avoiding lag or glitches during interaction.
- **Solution**: Optimized scripts and reduced computational overhead to enhance responsiveness.

### User Feedback and Visual Indicators
- **Challenge**: Providing intuitive feedback for interaction thresholds.
- **Solution**: Integrated clear visual cues for hand positions and interaction zones.

---

## Getting Started

### Prerequisites
- Unity Editor 2021.3.0 or later.
- XR Interaction Toolkit configured in Unity.
- VR headset with hand tracking enabled.

### Steps to Use
1. **Set Up the Environment**:
   - Import the Unity package provided in this repository.
   - Open the `XRC_GoGo.unity` scene in Unity.
2. **Run the Scene**:
   - Connect your VR headset to the computer.
   - Enter Play mode in Unity to interact with the virtual environment using the Go-Go technique.
3. **Enable Go-Go Interaction**:
   - Activate the Go-Go system and use hand gestures to extend your virtual hand and interact with objects.

---

## Results

### Demo
Watch the **XRC Go-Go** interaction technique in action:
- [**Full Demo Video**](https://drive.google.com/file/d/1pNjekIiLoGJuTaHlLJ9E0QOyblcQBclC/view?usp=sharing)

### Documentation
For a detailed explanation of the project, visit the project’s documentation website:
- [**XRC Go-Go Documentation**](https://cs5678-2024sp.github.io/h-go-go-Ruiznogueras05CT/index.html)

---

## Reflections and Future Enhancements

### Achievements
- Successfully implemented a non-linear virtual hand extension technique for immersive interactions.
- Delivered real-time feedback mechanisms to enhance user experience.
- Tackled challenges in synchronization, collider interaction, and performance optimization.

### Future Enhancements
- **Adaptive Mapping**: Implement user-specific customization for hand movement mapping.
- **Haptic Feedback**: Integrate haptic feedback for more immersive interactions.

---

## Acknowledgments

This project was developed as part of **CS 5678: 3D User Interfaces** at **Cornell Tech**, instructed by Harald Haraldsson, Director of the **XR Collaboratory**. For more information about this course, visit the [**XR Collaboratory Website**](https://xrcollaboratory.tech.cornell.edu/courses).
