using UnityEngine;
using UnityEngine.XR.ARFoundation;

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
        foreach (var image in args.added)
            CreateArea(image);

        foreach (var image in args.updated)
        {
            if (area != null)
                area.SetActive(image.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking);
        }
    }

    void CreateArea(ARTrackedImage image)
    {
        if (area != null)
            return;

        area = Instantiate(areaPrefab, image.transform);
        area.transform.localPosition = Vector3.zero;
        area.transform.localRotation = Quaternion.identity;
        area.transform.localScale = Vector3.one;
        area.SetActive(image.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking);
    }
}
