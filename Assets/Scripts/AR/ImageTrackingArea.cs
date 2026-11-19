using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTrackingArea : MonoBehaviour
{
    [SerializeField] GameObject areaPrefab;

    ARTrackedImageManager imageManager;
    GameObject area;

    void Awake()
    {
        imageManager = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        imageManager.trackedImagesChanged += OnImagesChanged;
    }

    void OnDisable()
    {
        imageManager.trackedImagesChanged -= OnImagesChanged;
    }

    void OnImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (ARTrackedImage image in args.added)
            CreateArea(image);

        foreach (ARTrackedImage image in args.updated)
        {
            if (area != null && image.transform == area.transform.parent)
                area.SetActive(image.trackingState == TrackingState.Tracking);
        }

        foreach (ARTrackedImage image in args.removed)
        {
            if (area != null && image.transform == area.transform.parent)
                area.SetActive(false);
        }
    }

    void CreateArea(ARTrackedImage image)
    {
        if (area != null || areaPrefab == null)
            return;

        area = Instantiate(areaPrefab, image.transform);
        area.name = "BuildingArea";
        area.transform.localPosition = Vector3.zero;

        // AR tracked images lie in local XY; the grid is generated in local XZ.
        area.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
        area.transform.localScale = Vector3.one;
        area.SetActive(image.trackingState == TrackingState.Tracking);
    }
}
