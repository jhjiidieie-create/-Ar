using UnityEngine;
using UnityEngine.EventSystems;

public class DeleteModeController : MonoBehaviour
{
    public bool deleteMode;

    public void ToggleDeleteMode()
    {
        deleteMode = !deleteMode;
    }

    private void Update()
    {
        if (!deleteMode)
            return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began &&
                (EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject(touch.fingerId)))
                DeleteAt(touch.position);
        }
        else if (Input.GetMouseButtonDown(0) &&
                 (EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject()))
            DeleteAt(Input.mousePosition);
    }

    private void DeleteAt(Vector2 screenPosition)
    {
        if (Camera.main == null ||
            !Physics.Raycast(Camera.main.ScreenPointToRay(screenPosition), out RaycastHit hit))
            return;

        Transform target = hit.transform;

        while (target.parent != transform && target.parent != null)
            target = target.parent;

        if (target.parent == transform &&
            (target.CompareTag("Road") ||
             target.CompareTag("Decoration") ||
             target.CompareTag("Building")))
            Destroy(target.gameObject);
    }
}