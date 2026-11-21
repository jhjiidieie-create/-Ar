using UnityEngine;

public class RemovalManager : MonoBehaviour
{
    [SerializeField] GridManager grid;
    [SerializeField] GridSelector selector;
    [SerializeField] PlacementManager placement;

    public void RemoveSelected()
    {
        if (!grid || !selector || !placement || !placement.PlacedObjects ||
            selector.SelectedCells.Count == 0)
            return;

        foreach (Transform building in placement.PlacedObjects)
        {
            if (grid.TryGetCell(building.position, out Vector2Int cell) &&
                selector.SelectedCells.Contains(cell))
                Destroy(building.gameObject);
        }

        selector.ClearSelection();
    }
}
