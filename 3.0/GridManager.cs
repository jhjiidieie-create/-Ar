using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] int columns = 21;
    [SerializeField] int rows = 23;
    [SerializeField] float cellSize = 0.01f;

    float CellX => cellSize / Mathf.Abs(transform.lossyScale.x);
    float CellZ => cellSize / Mathf.Abs(transform.lossyScale.z);

    void Start() => CreateGrid();

    void CreateGrid()
    {
        float width = columns * CellX;
        float depth = rows * CellZ;
        float y = 0.001f / Mathf.Abs(transform.lossyScale.y);
        float xThickness = 0.0003f / Mathf.Abs(transform.lossyScale.x);
        float zThickness = 0.0003f / Mathf.Abs(transform.lossyScale.z);

        for (int x = 0; x <= columns; x++)
            CreateLine(new Vector3(x * CellX - width / 2, y, 0),
                new Vector3(xThickness, y, depth));

        for (int z = 0; z <= rows; z++)
            CreateLine(new Vector3(0, y, z * CellZ - depth / 2),
                new Vector3(width, y, zThickness));
    }

    void CreateLine(Vector3 position, Vector3 scale)
    {
        GameObject line = GameObject.CreatePrimitive(PrimitiveType.Cube);
        line.name = "GridLine";
        line.transform.SetParent(transform, false);
        line.transform.localPosition = position;
        line.transform.localScale = scale;
        Destroy(line.GetComponent<Collider>());
    }

    public bool TryGetCell(Vector3 point, out Vector2Int cell)
    {
        Vector3 local = transform.InverseTransformPoint(point);
        int x = Mathf.FloorToInt((local.x + columns * CellX / 2) / CellX);
        int z = Mathf.FloorToInt((local.z + rows * CellZ / 2) / CellZ);

        cell = new Vector2Int(x, z);
        return x >= 0 && x < columns && z >= 0 && z < rows;
    }

    public Vector3 GetCellCenter(Vector2Int cell)
    {
        return transform.TransformPoint(new Vector3(
            (cell.x + 0.5f) * CellX - columns * CellX / 2,
            0.001f / Mathf.Abs(transform.lossyScale.y),
            (cell.y + 0.5f) * CellZ - rows * CellZ / 2));
    }
}