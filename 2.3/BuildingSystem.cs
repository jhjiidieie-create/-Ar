using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    public enum BuildLevel
    {
        Ground,
        Decoration,
        Building
    }

    class CellObjects
    {
        public GameObject[] levels = new GameObject[3];
    }

    readonly Dictionary<Vector2Int, CellObjects> cells =
        new Dictionary<Vector2Int, CellObjects>();

    public bool TryPlace(
        GridManager grid,
        Transform parent,
        Vector2Int cell,
        GameObject prefab,
        BuildLevel level)
    {
        if (!grid || !parent || !prefab)
            return false;

        CellObjects data = GetCell(cell);
        int index = (int)level;

        if (data.levels[index])
            return false;

        if (level != BuildLevel.Ground && !data.levels[(int)BuildLevel.Ground])
            return false;

        float height = 0f;

        for (int i = 0; i < index; i++)
            if (data.levels[i])
                height += data.levels[i].transform.localScale.y;

        Vector3 position = grid.GetCellCenter(cell);
        position.y += height + prefab.transform.localScale.y / 2f;

        GameObject placed = Instantiate(prefab, position, Quaternion.identity, parent);
        placed.name = prefab.name;
        data.levels[index] = placed;

        return true;
    }

    public bool RemoveTop(Vector2Int cell)
    {
        if (!cells.TryGetValue(cell, out CellObjects data))
            return false;

        for (int i = data.levels.Length - 1; i >= 0; i--)
        {
            if (!data.levels[i])
                continue;

            Destroy(data.levels[i]);
            data.levels[i] = null;

            if (IsEmpty(data))
                cells.Remove(cell);

            return true;
        }

        cells.Remove(cell);
        return false;
    }

    CellObjects GetCell(Vector2Int cell)
    {
        if (!cells.TryGetValue(cell, out CellObjects data))
        {
            data = new CellObjects();
            cells.Add(cell, data);
        }

        return data;
    }

    bool IsEmpty(CellObjects data)
    {
        foreach (GameObject level in data.levels)
            if (level)
                return false;

        return true;
    }
}