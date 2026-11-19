using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("A4 portrait: 21 x 29 cells, 1 cm each")]
    [SerializeField] int columns = 21;
    [SerializeField] int rows = 29;
    [SerializeField] float cellSize = 0.01f;
    [SerializeField] float lineThickness = 0.00035f;
    [SerializeField] float surfaceThickness = 0.001f;

    public int Columns => columns;
    public int Rows => rows;
    public float CellSize => cellSize;

    void Start()
    {
        CreateSurface();
        CreateGridLines();
    }

    public bool TryGetCell(Vector3 worldPoint, out Vector2Int cell)
    {
        Vector3 local = transform.InverseTransformPoint(worldPoint);
        float x = local.x + columns * cellSize * 0.5f;
        float z = local.z + rows * cellSize * 0.5f;

        int cx = Mathf.FloorToInt(x / cellSize);
        int cz = Mathf.FloorToInt(z / cellSize);

        cell = new Vector2Int(cx, cz);
        return cx >= 0 && cx < columns && cz >= 0 && cz < rows;
    }

    public Vector3 GetCellCenter(Vector2Int cell)
    {
        return transform.TransformPoint(new Vector3(
            (cell.x + 0.5f) * cellSize - columns * cellSize * 0.5f,
            0f,
            (cell.y + 0.5f) * cellSize - rows * cellSize * 0.5f));
    }

    void CreateSurface()
    {
        GameObject surface = GameObject.CreatePrimitive(PrimitiveType.Cube);
        surface.name = "BuildSurface";
        surface.transform.SetParent(transform, false);
        surface.transform.localPosition = new Vector3(0f, -surfaceThickness * 0.5f, 0f);
        surface.transform.localScale = new Vector3(columns * cellSize, surfaceThickness, rows * cellSize);
    }

    void CreateGridLines()
    {
        Transform root = new GameObject("GridLines").transform;
        root.SetParent(transform, false);

        float width = columns * cellSize;
        float depth = rows * cellSize;
        float y = 0.0002f;

        for (int x = 0; x <= columns; x++)
            MakeLine(root, new Vector3(x * cellSize - width * 0.5f, y, -depth * 0.5f),
                new Vector3(x * cellSize - width * 0.5f, y, depth * 0.5f));

        for (int z = 0; z <= rows; z++)
            MakeLine(root, new Vector3(-width * 0.5f, y, z * cellSize - depth * 0.5f),
                new Vector3(width * 0.5f, y, z * cellSize - depth * 0.5f));
    }

    void MakeLine(Transform parent, Vector3 start, Vector3 end)
    {
        GameObject line = GameObject.CreatePrimitive(PrimitiveType.Cube);
        line.name = "GridLine";
        line.transform.SetParent(parent, false);
        line.transform.localPosition = (start + end) * 0.5f;
        Vector3 direction = end - start;
        line.transform.localRotation = Quaternion.FromToRotation(Vector3.up, direction.normalized);
        line.transform.localScale = new Vector3(lineThickness, direction.magnitude, lineThickness);
        Destroy(line.GetComponent<Collider>());
    }
}
