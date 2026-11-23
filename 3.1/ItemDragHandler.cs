using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject prefab;
    public GridPlacement placement;

    private GameObject preview;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (prefab == null || placement == null)
            return;

        preview = Instantiate(prefab);

        foreach (Collider col in preview.GetComponentsInChildren<Collider>())
            col.enabled = false;

        UpdatePreview(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdatePreview(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (TryGetPoint(eventData.position, out Vector3 point))
        {
            GridManager grid = placement.GetComponent<GridManager>();

            int x = Mathf.FloorToInt((point.x - grid.transform.position.x) / grid.cellSize);
            int z = Mathf.FloorToInt((point.z - grid.transform.position.z) / grid.cellSize);

            placement.ReplaceCell(x, z, prefab);
        }

        if (preview != null)
            Destroy(preview);
    }

    private void UpdatePreview(Vector2 screenPosition)
    {
        if (preview == null || !TryGetPoint(screenPosition, out Vector3 point))
            return;

        point.y -= prefab.transform.localScale.y * 0.5f;
        preview.transform.position = point;
    }

    private bool TryGetPoint(Vector2 screenPosition, out Vector3 point)
    {
        point = Vector3.zero;

        if (placement == null || Camera.main == null)
            return false;

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane plane = new Plane(Vector3.up, placement.transform.position);

        if (!plane.Raycast(ray, out float distance))
            return false;

        point = ray.GetPoint(distance);
        return true;
    }
}