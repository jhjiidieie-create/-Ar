namespace ARBuildV27
{
    using System.Collections.Generic;
    using UnityEngine;

    public class BuildingSystem : MonoBehaviour
    {
        public enum BuildLevel { Ground, Decoration, Building }

        class CellObjects
        {
            public GameObject ground;
            public GameObject decoration;
            public readonly List<GameObject> buildings = new List<GameObject>();
        }

        readonly Dictionary<Vector2Int, CellObjects> cells =
            new Dictionary<Vector2Int, CellObjects>();

        public bool TryPlace(GridManager grid, Transform parent, Vector2Int cell,
            GameObject prefab, BuildLevel level)
        {
            if (!grid || !parent || !prefab) return false;

            CellObjects data = GetCell(cell);

            if (level != BuildLevel.Ground && !data.ground) return false;
            if (level == BuildLevel.Ground && data.ground) return false;
            if (level == BuildLevel.Decoration && (data.decoration || data.buildings.Count > 0)) return false;

            float heightBelow = GetHeight(data.ground) + GetHeight(data.decoration);
            if (level == BuildLevel.Building)
                foreach (GameObject building in data.buildings)
                    heightBelow += GetHeight(building);

            float objectHeight = GetHeight(prefab);
            Vector3 position = grid.GetCellCenter(cell);
            position.y += heightBelow + objectHeight / 2f;

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
            else
            {
                cells.Remove(cell);
                return false;
            }

            if (!data.ground && !data.decoration && data.buildings.Count == 0)
                cells.Remove(cell);

            return true;
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

        float GetHeight(GameObject obj)
        {
            if (!obj) return 0f;
            Renderer renderer = obj.GetComponentInChildren<Renderer>();
            return renderer ? renderer.bounds.size.y : obj.transform.localScale.y;
        }
    }
}