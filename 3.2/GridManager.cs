using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int columns = 21;
    public int rows = 23;
    public float cellSize = 1f;

    public Vector3 GetCellCenter(int x, int z)
    {
        return transform.position + new Vector3(
            (x + 0.5f) * cellSize,
            0,
            (z + 0.5f) * cellSize
        );
    }

    public bool IsInsideGrid(int x, int z)
    {
        return x >= 0 && x < columns &&
               z >= 0 && z < rows;
    }
}