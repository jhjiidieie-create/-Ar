using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTrackingArea : MonoBehaviour
{
    [SerializeField] GameObject areaPrefab;
    ARTrackedImageManager imageManager;
    GameObject area;
    TrackableId trackedId;

    void Awake() => imageManager = GetComponent<ARTrackedImageManager>();
    void OnEnable() => imageManager.trackedImagesChanged += OnImagesChanged;
    void OnDisable() => imageManager.trackedImagesChanged -= OnImagesChanged;

    void OnImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var image in args.added) CreateArea(image);
        foreach (var image in args.updated)
            if (image.trackableId == trackedId && area != null)
                area.SetActive(image.trackingState == TrackingState.Tracking);
        foreach (var image in args.removed)
            if (image.trackableId == trackedId && area != null) area.SetActive(false);
    }

    void CreateArea(ARTrackedImage image)
    {
        if (area != null || areaPrefab == null) return;
        trackedId = image.trackableId;
        area = Instantiate(areaPrefab, image.transform);
        area.name = "BuildingArea";
        area.transform.localPosition = Vector3.zero;
        area.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
        area.transform.localScale = Vector3.one;
        area.SetActive(image.trackingState == TrackingState.Tracking);
    }
}
