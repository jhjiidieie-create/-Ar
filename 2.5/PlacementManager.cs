using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuildItem
{
    public string itemName;
    public GameObject prefab;
    public BuildingSystem.BuildLevel level;
}

public class PlacementManager : MonoBehaviour
{
    [SerializeField] GridManager grid;
    [SerializeField] GridSelector selector;
    [SerializeField] BuildingSystem buildingSystem;
    [SerializeField] List<BuildItem> items = new List<BuildItem>();

    [SerializeField] BuildingSystem.BuildLevel selectedLevel;
    GameObject selectedPrefab;

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

        if (!buildingSystem)
            buildingSystem = GetComponent<BuildingSystem>();
    }

    public void SetLevel(BuildingSystem.BuildLevel level)
    {
        selectedLevel = level;
        selectedPrefab = null;
    }

    // Индекс считается только среди объектов выбранной категории.
    public void SelectItem(int index)
    {
        int current = 0;
        foreach (BuildItem item in items)
        {
            if (item.level != selectedLevel || !item.prefab) continue;
            if (current == index)
            {
                selectedPrefab = item.prefab;
                return;
            }
            current++;
        }
    }

    public int GetItemCount()
    {
        int count = 0;
        foreach (BuildItem item in items)
            if (item.level == selectedLevel && item.prefab) count++;
        return count;
    }

    public string GetItemName(int index)
    {
        int current = 0;
        foreach (BuildItem item in items)
        {
            if (item.level != selectedLevel || !item.prefab) continue;
            if (current == index)
                return string.IsNullOrEmpty(item.itemName) ? item.prefab.name : item.itemName;
            current++;
        }
        return "";
    }

    public void PlaceObject()
    {
        if (!grid || !selector || !buildingSystem || !selectedPrefab ||
            !PlacedObjects || selector.SelectedCells.Count == 0) return;

        foreach (Vector2Int cell in selector.SelectedCells)
            buildingSystem.TryPlace(grid, PlacedObjects, cell, selectedPrefab, selectedLevel);

        selector.ClearSelection();
    }
}