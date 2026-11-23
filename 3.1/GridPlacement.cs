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

        Vector3 center = grid.GetCellCenter(x, z);
        center.y += prefab.transform.localScale.y * 0.5f;
        Instantiate(prefab, center, Quaternion.identity, transform);

        PlayPlacementEffects(x, z);
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

        PlayPlacementEffects(x, z);
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

    private void PlayPlacementEffects(int x, int z)
    {
        if (placementEffect != null)
        {
            Vector3 effectPosition = grid.GetCellCenter(x, z);
            effectPosition.y += 0.001f;
            Instantiate(placementEffect, effectPosition, Quaternion.identity);
        }

        if (placementAudio != null)
            placementAudio.Play();
    }
}