using UnityEngine;

[RequireComponent(typeof(GridManager))]
public class GroundGenerator : MonoBehaviour
{
    public GameObject groundPrefab;

    private void Start()
    {
        GridManager grid = GetComponent<GridManager>();

        if (groundPrefab == null)
            return;

        float halfHeight = groundPrefab.transform.localScale.y * 0.5f;

        for (int x = 0; x < grid.columns; x++)
        {
            for (int z = 0; z < grid.rows; z++)
            {
                Vector3 position = grid.GetCellCenter(x, z);
                position.y -= halfHeight;

                Instantiate(groundPrefab, position, Quaternion.identity, transform);
            }
        }
    }
}