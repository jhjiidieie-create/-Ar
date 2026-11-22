using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    public enum BuildLevel { Ground, Decoration, Building }

    class CellObjects
    {
        public GameObject ground, decoration;
        public List<GameObject> buildings = new List<GameObject>();
    }

    readonly Dictionary<Vector2Int, CellObjects> cells = new Dictionary<Vector2Int, CellObjects>();

    public bool TryPlace(GridManager grid, Transform parent, Vector2Int cell,
        GameObject prefab, BuildLevel level)
    {
        if (!grid || !parent || !prefab) return false;

        cells.TryGetValue(cell, out CellObjects data);

        if (level != BuildLevel.Ground && (data == null || !data.ground)) return false;
        if (level == BuildLevel.Ground && data != null && data.ground) return false;
        if (level == BuildLevel.Decoration && (data.decoration || data.buildings.Count > 0)) return false;

        if (data == null)
        {
            data = new CellObjects();
            cells.Add(cell, data);
        }

        float height = GetHeight(data.ground);

        if (level == BuildLevel.Building)
        {
            height += GetHeight(data.decoration);
            foreach (GameObject building in data.buildings)
                height += GetHeight(building);
        }

        float objectHeight = GetHeight(prefab);
        Vector3 position = grid.GetCellCenter(cell);
        position.y += height + objectHeight / 2;

        GameObject placed = Instantiate(prefab, position, Quaternion.identity, parent);
        placed.name = prefab.name;

        if (level == BuildLevel.Ground) data.ground = placed;
        else if (level == BuildLevel.Decoration) data.decoration = placed;
        else data.buildings.Add(placed);

        return true;
    }

    public bool RemoveTop(Vector2Int cell)
    {
        if (!cells.TryGetValue(cell, out CellObjects data)) return false;

        if (data.buildings.Count > 0)
        {
            int last = data.buildings.Count - 1;
            Destroy(data.buildings[last]);
            data.buildings.RemoveAt(last);
        }
        else if (data.decoration)
        {
            Destroy(data.decoration);
            data.decoration = null;
        }
        else if (data.ground)
        {
            Destroy(data.ground);
            data.ground = null;
        }
        else return false;

        if (!data.ground && !data.decoration && data.buildings.Count == 0)
            cells.Remove(cell);

        return true;
    }

    float GetHeight(GameObject obj)
    {
        if (!obj) return 0;
        Renderer renderer = obj.GetComponentInChildren<Renderer>();
        return renderer ? renderer.bounds.size.y : obj.transform.localScale.y;
    }
}