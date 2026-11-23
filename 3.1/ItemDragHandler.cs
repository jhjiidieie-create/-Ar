using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject prefab;
    public GridPlacement placement;
    public GameObject highlightPrefab;
    public Material validHighlightMaterial;
    public Material invalidHighlightMaterial;

    private GameObject preview;
    private GameObject highlight;
    private Renderer highlightRenderer;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (prefab == null || placement == null || highlightPrefab == null)
            return;

        preview = Instantiate(prefab);
        highlight = Instantiate(highlightPrefab);
        highlightRenderer = highlight.GetComponent<Renderer>();

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

            if (prefab.CompareTag("Decoration"))
                placement.PlaceDecoration(x, z, prefab);
            else
                placement.ReplaceCell(x, z, prefab);
        }

        if (preview != null)
            Destroy(preview);

        if (highlight != null)
            Destroy(highlight);
    }

    private void UpdatePreview(Vector2 screenPosition)
    {
        if (preview == null || !TryGetPoint(screenPosition, out Vector3 point))
            return;

        point.y += prefab.transform.localScale.y * 0.5f + 0.001f;
        preview.transform.position = point;

        GridManager grid = placement.GetComponent<GridManager>();
        int x = Mathf.FloorToInt((point.x - grid.transform.position.x) / grid.cellSize);
        int z = Mathf.FloorToInt((point.z - grid.transform.position.z) / grid.cellSize);

        bool valid = prefab.CompareTag("Decoration")
            ? placement.CanPlaceDecoration(x, z)
            : grid.IsInsideGrid(x, z);

        Vector3 cellCenter = grid.GetCellCenter(x, z);
        cellCenter.y += 0.006f;

        highlight.transform.position = cellCenter;
        highlight.SetActive(true);

        if (highlightRenderer != null)
        {
            Material material = valid ? validHighlightMaterial : invalidHighlightMaterial;

            if (material != null)
                highlightRenderer.sharedMaterial = material;
        }
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