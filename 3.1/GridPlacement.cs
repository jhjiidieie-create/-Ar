using UnityEngine;

[RequireComponent(typeof(GridManager))]
public class GridPlacement : MonoBehaviour
{
    private GridManager grid;

    private void Awake()
    {
        grid = GetComponent<GridManager>();
    }

    public void ReplaceCell(int x, int z, GameObject prefab)
    {
        if (prefab == null || !grid.IsInsideGrid(x, z))
            return;

        Vector3 center = grid.GetCellCenter(x, z);

        foreach (Transform child in transform)
        {
            Vector3 position = child.position;

            if (Mathf.Abs(position.x - center.x) < 0.001f &&
                Mathf.Abs(position.z - center.z) < 0.001f)
            {
                Destroy(child.gameObject);
                break;
            }
        }

        center.y -= prefab.transform.localScale.y * 0.5f;
        Instantiate(prefab, center, Quaternion.identity, transform);
    }
}