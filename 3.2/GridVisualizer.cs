using UnityEngine;

[RequireComponent(typeof(GridManager))]
public class GridVisualizer : MonoBehaviour
{
    public float lineHeight = 0.01f;
    public Color lineColor = Color.gray;

    private GridManager grid;

    private void Awake()
    {
        grid = GetComponent<GridManager>();
    }

    private void OnDrawGizmos()
    {
        if (grid == null)
            grid = GetComponent<GridManager>();

        if (grid == null)
            return;

        Gizmos.color = lineColor;

        float width = grid.columns * grid.cellSize;
        float depth = grid.rows * grid.cellSize;
        Vector3 origin = transform.position + Vector3.up * lineHeight;

        for (int x = 0; x <= grid.columns; x++)
        {
            Vector3 start = origin + Vector3.right * (x * grid.cellSize);
            Vector3 end = start + Vector3.forward * depth;
            Gizmos.DrawLine(start, end);
        }

        for (int z = 0; z <= grid.rows; z++)
        {
            Vector3 start = origin + Vector3.forward * (z * grid.cellSize);
            Vector3 end = start + Vector3.right * width;
            Gizmos.DrawLine(start, end);
        }
    }
}