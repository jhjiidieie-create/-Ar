using System.Collections.Generic;
using UnityEngine;

public class GridSelector : MonoBehaviour
{
    [SerializeField] GridManager grid;
    [SerializeField] Camera arCamera;
    [SerializeField] float cellSize = 0.05f;
    [SerializeField] Color highlightColor = Color.green;

    readonly Dictionary<Vector2Int, GameObject> highlights = new Dictionary<Vector2Int, GameObject>();
    public List<Vector2Int> SelectedCells { get; } = new List<Vector2Int>();

    void Start()
    {
        if (!arCamera) arCamera = Camera.main;
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
        if (!grid || !arCamera) return;

        Ray ray = arCamera.ScreenPointToRay(screenPosition);
        if (!Physics.Raycast(ray, out RaycastHit hit)) return;
        if (hit.transform != grid.transform && !hit.transform.IsChildOf(grid.transform)) return;
        if (!grid.TryGetCell(hit.point, out Vector2Int cell)) return;

        if (highlights.TryGetValue(cell, out GameObject selectedHighlight))
        {
            Destroy(selectedHighlight);
            highlights.Remove(cell);
            SelectedCells.Remove(cell);
            return;
        }

        GameObject highlight = GameObject.CreatePrimitive(PrimitiveType.Cube);
        highlight.name = "SelectedCell";
        highlight.transform.SetParent(grid.transform, false);
        Destroy(highlight.GetComponent<Collider>());
        highlight.transform.localScale = new Vector3(
            cellSize / Mathf.Abs(grid.transform.lossyScale.x),
            0.001f / Mathf.Abs(grid.transform.lossyScale.y),
            cellSize / Mathf.Abs(grid.transform.lossyScale.z));
        highlight.transform.localPosition = grid.transform.InverseTransformPoint(grid.GetCellCenter(cell));
        highlight.GetComponent<Renderer>().material.color = highlightColor;
        highlights.Add(cell, highlight);
        SelectedCells.Add(cell);
    }

    public void ClearSelection()
    {
        foreach (GameObject highlight in highlights.Values)
            Destroy(highlight);
        highlights.Clear();
        SelectedCells.Clear();
    }
}