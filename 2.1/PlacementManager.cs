using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] GridManager grid;
    [SerializeField] GridSelector selector;
    [SerializeField] GameObject buildingPrefab;

    public Transform PlacedObjects { get; private set; }

    void Awake()
    {
        if (grid)
        {
            GameObject container = new GameObject("PlacedObjects");
            PlacedObjects = container.transform;
            PlacedObjects.SetParent(grid.transform, false);
            PlacedObjects.localScale = new Vector3(
                1f / grid.transform.lossyScale.x,
                1f / grid.transform.lossyScale.y,
                1f / grid.transform.lossyScale.z);
        }
    }

    public void PlaceObject()
    {
        if (!grid || !selector || selector.SelectedCells.Count == 0 ||
            !buildingPrefab || !PlacedObjects)
            return;

        foreach (Vector2Int cell in selector.SelectedCells)
        {
            Vector3 position = grid.GetCellCenter(cell);
            position.y += buildingPrefab.transform.localScale.y / 2f;

            Instantiate(buildingPrefab, position, Quaternion.identity, PlacedObjects);
        }
    }
}
