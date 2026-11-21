using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] GridManager grid;
    [SerializeField] GridSelector selector;
    [SerializeField] GameObject buildingPrefab;

    public void PlaceObject()
    {
        if (!selector || !selector.HasSelection || !buildingPrefab)
            return;

        Vector3 position = grid.GetCellCenter(selector.SelectedCell);
        Instantiate(buildingPrefab, position, Quaternion.identity);
    }
}
