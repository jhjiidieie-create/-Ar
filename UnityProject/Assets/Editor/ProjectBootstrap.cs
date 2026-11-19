using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.XR.ARSubsystems;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.XR.CoreUtils;
using System.IO;

[InitializeOnLoad]
public static class ProjectBootstrap
{
    const string ScenePath = "Assets/Scenes/MainScene.unity";
    const string LibraryPath = "Assets/ReferenceImages/ARReferenceImageLibrary.asset";
    const string AreaPath = "Assets/Prefabs/BuildingArea.prefab";
    const string MarkerPath = "Assets/ReferenceImages/DetroitMarker.jpg";

    static ProjectBootstrap() => EditorApplication.delayCall += CreateIfNeeded;

    static void CreateIfNeeded()
    {
        if (!EditorApplication.isPlayingOrWillChangePlaymode && !File.Exists(ScenePath) && File.Exists(MarkerPath))
            SetupProject();
    }

    [MenuItem("AR City Builder/Setup or Rebuild First Scene")]
    public static void SetupProject()
    {
        TextureImporter importer = AssetImporter.GetAtPath(MarkerPath) as TextureImporter;
        if (importer != null && !importer.isReadable) { importer.isReadable = true; importer.SaveAndReimport(); }
        Texture2D marker = AssetDatabase.LoadAssetAtPath<Texture2D>(MarkerPath);
        if (marker == null) { Debug.LogError("Marker image missing: " + MarkerPath); return; }

        XRReferenceImageLibrary library = AssetDatabase.LoadAssetAtPath<XRReferenceImageLibrary>(LibraryPath);
        if (library == null)
        {
            library = ScriptableObject.CreateInstance<XRReferenceImageLibrary>();
            AssetDatabase.CreateAsset(library, LibraryPath);
        }
        while (library.count > 0) library.RemoveAt(library.count - 1);
        library.Add();
        library.SetTexture(0, marker, true);
        library.SetName(0, "DetroitMarker");
        library.SetSpecifySize(0, true);
        library.SetSize(0, new Vector2(0.21f, 0.2329f));
        EditorUtility.SetDirty(library);

        GameObject old = AssetDatabase.LoadAssetAtPath<GameObject>(AreaPath);
        if (old != null) AssetDatabase.DeleteAsset(AreaPath);
        GameObject area = new GameObject("BuildingArea");
        area.AddComponent<GridManager>();
        GameObject areaPrefab = PrefabUtility.SaveAsPrefabAsset(area, AreaPath);
        Object.DestroyImmediate(area);

        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        new GameObject("AR Session").AddComponent<ARSession>();
        GameObject originObject = new GameObject("XR Origin");
        XROrigin origin = originObject.AddComponent<XROrigin>();
        GameObject cameraObject = new GameObject("AR Camera");
        cameraObject.tag = "MainCamera";
        cameraObject.transform.SetParent(originObject.transform, false);
        Camera camera = cameraObject.AddComponent<Camera>();
        camera.nearClipPlane = 0.01f;
        cameraObject.AddComponent<ARCameraManager>();
        cameraObject.AddComponent<ARCameraBackground>();
        origin.Camera = camera;

        ARTrackedImageManager imageManager = originObject.AddComponent<ARTrackedImageManager>();
        imageManager.referenceLibrary = library;
        imageManager.requestedMaxNumberOfMovingImages = 1;
        ImageTrackingArea tracker = originObject.AddComponent<ImageTrackingArea>();
        SerializedObject so = new SerializedObject(tracker);
        so.FindProperty("areaPrefab").objectReferenceValue = areaPrefab;
        so.ApplyModifiedPropertiesWithoutUndo();

        GameObject bmObject = new GameObject("Building Manager");
        BuildingManager bm = bmObject.AddComponent<BuildingManager>();
        SerializedObject bmData = new SerializedObject(bm);
        bmData.FindProperty("arCamera").objectReferenceValue = camera;
        bmData.ApplyModifiedPropertiesWithoutUndo();

        EditorSceneManager.SaveScene(scene, ScenePath);
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel24;
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.citybuilder.ar");
        AssetDatabase.SaveAssets();
        EditorSceneManager.OpenScene(ScenePath);
        Debug.Log("First AR scene generated. Enable ARCore in XR Plug-in Management for Android before building.");
    }
}
