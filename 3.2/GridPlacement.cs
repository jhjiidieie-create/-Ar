using UnityEngine;

[RequireComponent(typeof(GridManager))]
public class GridPlacement : MonoBehaviour
{
    public GameObject placementEffect;
    public AudioSource placementAudio;

    private GridManager grid;

    private void Awake()
    {
        grid = GetComponent<GridManager>();
    }

    public bool CanPlaceDecoration(int x, int z)
    {
        if (!grid.IsInsideGrid(x, z) ||
            !HasObjectAt(x, z, "Ground") ||
            HasObjectAt(x, z, "Road") ||
            HasObjectAt(x, z, "Decoration"))
            return false;

        return HasObjectAt(x + 1, z, "Road") ||
               HasObjectAt(x - 1, z, "Road") ||
               HasObjectAt(x, z + 1, "Road") ||
               HasObjectAt(x, z - 1, "Road");
    }

    public void PlaceDecoration(int x, int z, GameObject prefab)
    {
        if (prefab == null || !CanPlaceDecoration(x, z))
            return;

        Vector3 position = grid.GetCellCenter(x, z);
        position.y += prefab.transform.localScale.y * 0.5f;
        Spawn(prefab, position, x, z);
    }

    public bool CanPlaceBuildingBlock(int x, int z)
    {
        return grid.IsInsideGrid(x, z) &&
               HasObjectAt(x, z, "Ground") &&
               !HasObjectAt(x, z, "Road") &&
               !HasObjectAt(x, z, "Decoration");
    }

    public float GetBuildingPlacementY(int x, int z, GameObject prefab)
    {
        Vector3 center = grid.GetCellCenter(x, z);
        float top = center.y;

        foreach (Transform child in transform)
        {
            Vector3 position = child.position;

            if (child.CompareTag("Building") &&
                Mathf.Abs(position.x - center.x) < 0.001f &&
                Mathf.Abs(position.z - center.z) < 0.001f)
                top = Mathf.Max(top, position.y + child.localScale.y * 0.5f);
        }

        return top + prefab.transform.localScale.y * 0.5f;
    }

    public void PlaceBuildingBlock(int x, int z, GameObject prefab)
    {
        if (prefab == null || !CanPlaceBuildingBlock(x, z))
            return;

        Vector3 position = grid.GetCellCenter(x, z);
        position.y = GetBuildingPlacementY(x, z, prefab);
        Spawn(prefab, position, x, z);
    }

    public void ReplaceCell(int x, int z, GameObject prefab)
    {
        if (prefab == null || !grid.IsInsideGrid(x, z))
            return;

        Vector3 position = grid.GetCellCenter(x, z);

        foreach (Transform child in transform)
        {
            Vector3 childPosition = child.position;

            if (Mathf.Abs(childPosition.x - position.x) < 0.001f &&
                Mathf.Abs(childPosition.z - position.z) < 0.001f)
            {
                Destroy(child.gameObject);
                break;
            }
        }

        position.y -= prefab.transform.localScale.y * 0.5f;
        Spawn(prefab, position, x, z);
    }

    private bool HasObjectAt(int x, int z, string objectTag)
    {
        if (!grid.IsInsideGrid(x, z))
            return false;

        Vector3 center = grid.GetCellCenter(x, z);

        foreach (Transform child in transform)
        {
            Vector3 position = child.position;

            if (Mathf.Abs(position.x - center.x) < 0.001f &&
                Mathf.Abs(position.z - center.z) < 0.001f &&
                child.CompareTag(objectTag))
                return true;
        }

        return false;
    }

    private void Spawn(GameObject prefab, Vector3 position, int x, int z)
    {
        Instantiate(prefab, position, Quaternion.identity, transform);
        PlayPlacementEffects(x, z);
    }

    private void PlayPlacementEffects(int x, int z)
    {
        if (placementEffect != null)
        {
            Vector3 position = grid.GetCellCenter(x, z);
            position.y += 0.001f;
            Instantiate(placementEffect, position, Quaternion.identity);
        }

        if (placementAudio != null)
            placementAudio.Play();
    }
}