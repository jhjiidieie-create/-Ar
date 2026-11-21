using UnityEngine;

public class GridSelector : MonoBehaviour
{
    [SerializeField] GridManager grid;
    [SerializeField] Camera arCamera;
    [SerializeField] float cellSize = 0.05f;
    [SerializeField] Color highlightColor = Color.green;

    GameObject highlight;

    public Vector2Int SelectedCell { get; private set; }
    public bool HasSelection { get; private set; }

    void Start()
    {
        if (!arCamera) arCamera = Camera.main;

        highlight = GameObject.CreatePrimitive(PrimitiveType.Cube);
        highlight.name = "SelectedCell";
        highlight.transform.SetParent(grid.transform, false);
        Destroy(highlight.GetComponent<Collider>());

        highlight.transform.localScale = new Vector3(
            cellSize / Mathf.Abs(grid.transform.lossyScale.x),
            0.001f / Mathf.Abs(grid.transform.lossyScale.y),
            cellSize / Mathf.Abs(grid.transform.lossyScale.z));

        highlight.GetComponent<Renderer>().material.color = highlightColor;
        highlight.SetActive(false);
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
                SelectCell(Input.GetTouch(0).position);
        }
        else if (Input.GetMouseButtonDown(0))
            SelectCell(Input.mousePosition);
    }

    void SelectCell(Vector2 screenPosition)
    {
        Ray ray = arCamera.ScreenPointToRay(screenPosition);

        if (!Physics.Raycast(ray, out RaycastHit hit) ||
            hit.transform != grid.transform)
            return;

        if (!grid.TryGetCell(hit.point, out Vector2Int cell))
            return;

        SelectedCell = cell;
        HasSelection = true;

        highlight.transform.localPosition = grid.transform.InverseTransformPoint(
            grid.GetCellCenter(cell));
        highlight.SetActive(true);
    }
}
