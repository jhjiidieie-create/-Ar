using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] GridManager grid;
    [SerializeField] GridSelector selector;
    [SerializeField] GameObject buildingPrefab;

    public void PlaceObject()
    {
        if (!grid || !selector || selector.SelectedCells.Count == 0 || !buildingPrefab)
            return;

        foreach (Vector2Int cell in selector.SelectedCells)
        {
            Vector3 position = grid.GetCellCenter(cell);
            position.y += buildingPrefab.transform.localScale.y / 2f;

            Instantiate(buildingPrefab, position, Quaternion.identity);
        }
    }
}
