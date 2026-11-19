# AR City Builder — Stage 1

Unity **2022.3.62f3**, Android, AR Foundation.

## Prototype scope

- Track a printed A4 image with AR Foundation.
- Attach a construction area to the tracked image.
- Generate a 21 x 23 grid (1 cm cells) over the supplied poster image, printed centered on A4 portrait paper.
- Tap the surface to place the selected prefab into the nearest grid cell.
- One construction level only.

## Marker image

The marker image must be a high-detail, non-repeating image. Print it on A4 portrait paper at 100% scale. In Unity, add the exact image to an **XR Reference Image Library** and set its physical width to **0.210 m**.

The grid is 0.210 x 0.290 m. An A4 portrait sheet is 0.210 x 0.297 m, so the 29 cm grid leaves 7 mm total along the long edge.

## Unity setup

1. Open this repository folder in Unity Hub using Unity 2022.3.62f3.
2. Let Package Manager install the dependencies from `Packages/manifest.json`.
3. Install Android Build Support, Android SDK & NDK Tools, and OpenJDK through Unity Hub.
4. In **Project Settings > XR Plug-in Management > Android**, enable **ARCore**.
5. Create an **XR Reference Image Library** (Project window > Create > XR > Reference Image Library). Add the printed marker image and set its physical width to 0.210.
6. Create a scene with an **AR Session** and an **XR Origin (Mobile AR)**. Add **AR Tracked Image Manager** to XR Origin and assign the reference image library.
7. Create an area prefab with a root object at the marker center. Add **GridManager** to the root.
8. Add **ImageTrackingArea** to the object with AR Tracked Image Manager and assign the area prefab to its `areaPrefab` field.
9. Add **BuildingManager** to a scene object and assign the AR Camera. It will find the runtime GridManager when the tracked image creates the area.
10. Create block prefabs with their pivot at the bottom center. For this initial grid, each cell is 0.01 x 0.01 Unity units (1 cm), so use suitably small test blocks. Connect UI buttons to `BuildingManager.SelectPrefab(GameObject)`.
11. Set Android orientation to Portrait and build to a supported ARCore device.

## Scripts

- `Assets/Scripts/AR/ImageTrackingArea.cs` — creates the build area under the tracked image.
- `Assets/Scripts/Building/GridManager.cs` — creates the surface and grid; converts world positions to cell coordinates.
- `Assets/Scripts/Building/BuildingManager.cs` — raycasts from the AR camera and places the selected prefab.

## Current limitations

This is the **first-stage source scaffold**, not a tested APK or a fully configured Unity scene. The marker image, XR Reference Image Library, scene objects, area prefab, and block prefabs still need to be assigned in the Unity Editor. The project has not been compiled in Unity in this environment.


## Full Unity source layout

The Unity project source files are mirrored in [`UnityProject/`](./tree/main/UnityProject). The complete ZIP distributed in the ChatGPT conversation also includes the supplied marker image (`Assets/ReferenceImages/DetroitMarker.jpg`). The GitHub source mirror omits that binary image; use the ZIP for a complete ready-to-open project.
