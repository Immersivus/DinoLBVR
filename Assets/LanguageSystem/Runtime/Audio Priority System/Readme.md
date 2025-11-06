### **Audio Priority System**  

The **Audio Priority System** manages the priority of audio playback.  

To play audio from a scene, an **Audio Launcher** component must be added to a GameObject. This component provides three functions to control audio behavior:  

1. **Play**: Considers the layer of the **Audio Object** before playing.  
2. **Play Ignoring Layer**: Ignores the layer and plays the audio directly.  
3. **Stop**: Stops the audio immediately.

When an audio with the same priority or higher is play, current audio will perform a fade out.