using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] GridManager grid;
    [SerializeField] GridSelector selector;
    [SerializeField] BuildingSystem buildingSystem;
    [SerializeField] GameObject buildingPrefab;
    [SerializeField] BuildingSystem.BuildLevel selectedLevel;

    public Transform PlacedObjects { get; private set; }

    void Awake()
    {
        if (!grid)
            return;

        GameObject container = new GameObject("PlacedObjects");
        PlacedObjects = container.transform;
        PlacedObjects.SetParent(grid.transform, false);
        PlacedObjects.localScale = new Vector3(
            1f / grid.transform.lossyScale.x,
            1f / grid.transform.lossyScale.y,
            1f / grid.transform.lossyScale.z);

        if (!buildingSystem)
            buildingSystem = GetComponent<BuildingSystem>();
    }

    public void PlaceObject()
    {
        if (!grid || !selector || !buildingSystem || !buildingPrefab ||
            !PlacedObjects || selector.SelectedCells.Count == 0)
            return;

        foreach (Vector2Int cell in selector.SelectedCells)
            buildingSystem.TryPlace(
                grid, PlacedObjects, cell, buildingPrefab, selectedLevel);

        selector.ClearSelection();
    }

    public void SetLevel(BuildingSystem.BuildLevel level)
    {
        selectedLevel = level;
    }
}