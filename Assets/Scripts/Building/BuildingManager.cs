using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] Camera arCamera;
    [SerializeField] GridManager grid;
    [SerializeField] Transform buildingsRoot;
    [SerializeField] LayerMask buildSurfaceMask = ~0;

    GameObject selectedPrefab;
    readonly Dictionary<Vector2Int, GameObject> placed = new Dictionary<Vector2Int, GameObject>();

    public void SelectPrefab(GameObject prefab)
    {
        selectedPrefab = prefab;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began &&
                (EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject(touch.fingerId)))
                PlaceAt(touch.position);
        }
#if UNITY_EDITOR
        else if (Input.GetMouseButtonDown(0) &&
                 (EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject()))
        {
            PlaceAt(Input.mousePosition);
        }
#endif
    }

    void PlaceAt(Vector2 screenPosition)
    {
        if (selectedPrefab == null || arCamera == null || grid == null)
            return;

        Ray ray = arCamera.ScreenPointToRay(screenPosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, 5f, buildSurfaceMask))
            return;

        if (!grid.TryGetCell(hit.point, out Vector2Int cell))
            return;

        if (placed.TryGetValue(cell, out GameObject oldBlock))
            Destroy(oldBlock);

        Vector3 position = grid.GetCellCenter(cell);
        GameObject block = Instantiate(selectedPrefab, position, grid.transform.rotation, buildingsRoot);
        placed[cell] = block;
    }
}
