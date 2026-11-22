using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] GridManager grid;
    [SerializeField] GridSelector selector;
    [SerializeField] BuildingSystem buildingSystem;

    public Transform PlacedObjects { get; private set; }

    void Awake()
    {
        if (!grid) return;

        PlacedObjects = new GameObject("PlacedObjects").transform;
        PlacedObjects.SetParent(grid.transform, false);
        PlacedObjects.localScale = new Vector3(
            1f / grid.transform.lossyScale.x,
            1f / grid.transform.lossyScale.y,
            1f / grid.transform.lossyScale.z);

        if (!buildingSystem) buildingSystem = GetComponent<BuildingSystem>();
    }

    public void PlaceObject(GameObject prefab, BuildingSystem.BuildLevel level)
    {
        if (!grid || !selector || !buildingSystem || !prefab ||
            !PlacedObjects || selector.SelectedCells.Count == 0) return;

        foreach (Vector2Int cell in selector.SelectedCells)
            buildingSystem.TryPlace(grid, PlacedObjects, cell, prefab, level);

        selector.ClearSelection();
    }
}