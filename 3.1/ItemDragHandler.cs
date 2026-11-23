using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject prefab;
    public GridPlacement placement;

    public void OnBeginDrag(PointerEventData eventData) { }

    public void OnDrag(PointerEventData eventData) { }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (prefab == null || placement == null || Camera.main == null)
            return;

        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        Plane plane = new Plane(Vector3.up, placement.transform.position);

        if (!plane.Raycast(ray, out float distance))
            return;

        Vector3 point = ray.GetPoint(distance);
        GridManager grid = placement.GetComponent<GridManager>();

        int x = Mathf.FloorToInt((point.x - grid.transform.position.x) / grid.cellSize);
        int z = Mathf.FloorToInt((point.z - grid.transform.position.z) / grid.cellSize);

        placement.ReplaceCell(x, z, prefab);
    }
}