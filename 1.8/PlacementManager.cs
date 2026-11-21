using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] GridManager grid;
    [SerializeField] GridSelector selector;
    [SerializeField] GameObject buildingPrefab;

    public void PlaceObject()
    {
        if (!grid || !selector || !selector.HasSelection || !buildingPrefab)
            return;

        Vector3 position = grid.GetCellCenter(selector.SelectedCell);
        position.y += buildingPrefab.transform.localScale.y / 2f;

        Instantiate(buildingPrefab, position, Quaternion.identity);
    }
}
