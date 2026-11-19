# AR City Builder — Unity 2022.3.62f3

Android prototype using AR Foundation 5.1.5 and ARCore.

## Included
- Source marker image is included in the downloadable ZIP at `Assets/ReferenceImages/DetroitMarker.jpg`.
- Editor bootstrap creates MainScene, XR Reference Image Library and BuildingArea prefab.
- Image tracking attaches the 21 x 23 (1 cm) grid to the tracked poster.
- Grid and touch-to-place scripts are included.

## Setup
1. Extract the ZIP and add the folder in Unity Hub.
2. Open with Unity 2022.3.62f3 and wait for packages/scripts to import.
3. The editor bootstrap generates and opens MainScene. If needed use AR City Builder > Setup or Rebuild First Scene.
4. Open Project Settings > XR Plug-in Management > Android and enable ARCore.
5. Install Android Build Support, SDK/NDK and OpenJDK in Unity Hub.
6. Build and run on an ARCore-supported Android phone.

The reference image physical width is set to 0.210 m. Print the supplied poster centered on A4 without stretching. Grid size is 0.21 x 0.23 m.

## Important
The project is a first-stage scaffold and has not been compiled in Unity in this environment. The side-panel UI, sample block prefabs, placement preview and other building tools are future work. The marker JPG is inside the ZIP; the GitHub mirror stores the source scripts and setup files.
