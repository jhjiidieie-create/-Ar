using UnityEngine;
using UnityEngine.EventSystems;

public class DeleteModeController : MonoBehaviour, IPointerClickHandler
{
    public bool deleteMode;

    public void ToggleDeleteMode()
    {
        deleteMode = !deleteMode;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!deleteMode)
            return;

        GameObject clicked = eventData.pointerCurrentRaycast.gameObject;
        if (clicked == null)
            return;

        Transform target = clicked.transform;

        while (target.parent != transform && target.parent != null)
            target = target.parent;

        if (target.parent != transform)
            return;

        if (target.CompareTag("Ground") ||
            target.CompareTag("Road") ||
            target.CompareTag("Decoration") ||
            target.CompareTag("Building"))
            Destroy(target.gameObject);
    }
}