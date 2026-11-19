using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] int columns = 21;
    [SerializeField] int rows = 23;
    [SerializeField] float cellSize = 0.01f;
    [SerializeField] float lineThickness = 0.00025f;
    [SerializeField] float surfaceThickness = 0.0005f;

    public bool TryGetCell(Vector3 point, out Vector2Int cell)
    {
        Vector3 local = transform.InverseTransformPoint(point);
        int x = Mathf.FloorToInt((local.x + columns * cellSize * 0.5f) / cellSize);
        int z = Mathf.FloorToInt((local.z + rows * cellSize * 0.5f) / cellSize);
        cell = new Vector2Int(x, z);
        return x >= 0 && x < columns && z >= 0 && z < rows;
    }

    public Vector3 GetCellCenter(Vector2Int cell) =>
        transform.TransformPoint(new Vector3(
            (cell.x + 0.5f) * cellSize - columns * cellSize * 0.5f,
            0f,
            (cell.y + 0.5f) * cellSize - rows * cellSize * 0.5f));

    void Start()
    {
        GameObject surface = GameObject.CreatePrimitive(PrimitiveType.Cube);
        surface.name = "BuildSurface";
        surface.transform.SetParent(transform, false);
        surface.transform.localPosition = new Vector3(0f, -surfaceThickness * 0.5f, 0f);
        surface.transform.localScale = new Vector3(columns * cellSize, surfaceThickness, rows * cellSize);

        Transform lines = new GameObject("GridLines").transform;
        lines.SetParent(transform, false);
        float width = columns * cellSize, depth = rows * cellSize;
        for (int x = 0; x <= columns; x++)
            MakeLine(lines, new Vector3(x * cellSize - width * .5f, .0002f, -depth * .5f),
                new Vector3(x * cellSize - width * .5f, .0002f, depth * .5f));
        for (int z = 0; z <= rows; z++)
            MakeLine(lines, new Vector3(-width * .5f, .0002f, z * cellSize - depth * .5f),
                new Vector3(width * .5f, .0002f, z * cellSize - depth * .5f));
    }

    void MakeLine(Transform parent, Vector3 a, Vector3 b)
    {
        GameObject line = GameObject.CreatePrimitive(PrimitiveType.Cube);
        line.name = "GridLine";
        line.transform.SetParent(parent, false);
        line.transform.localPosition = (a + b) * .5f;
        Vector3 direction = b - a;
        line.transform.localRotation = Quaternion.FromToRotation(Vector3.up, direction.normalized);
        line.transform.localScale = new Vector3(lineThickness, direction.magnitude, lineThickness);
        Destroy(line.GetComponent<Collider>());
    }
}
