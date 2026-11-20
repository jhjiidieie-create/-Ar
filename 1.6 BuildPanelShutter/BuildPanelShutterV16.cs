using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BuildPanelShutterV16 : MonoBehaviour
{
    [SerializeField] private RectTransform buildPanel;
    [SerializeField] private RectTransform categoryContainer;
    [SerializeField] private Button handleButton;
    [SerializeField] private float bottomPadding = 12f;
    [SerializeField] private float duration = 0.35f;

    private float openHeight;
    private float closedHeight;
    private bool isOpen = true;
    private Coroutine animationCoroutine;

    private void Start()
    {
        openHeight = buildPanel.rect.height;
        closedHeight = GetClosedHeight();

        handleButton.onClick.AddListener(ToggleShutter);
    }

    private float GetClosedHeight()
    {
        Vector3[] corners = new Vector3[4];
        categoryContainer.GetWorldCorners(corners);

        float categoryBottom = buildPanel.InverseTransformPoint(corners[0]).y;
        float panelBottom = -buildPanel.rect.height * buildPanel.pivot.y;

        return Mathf.Max(1f, categoryBottom - panelBottom + bottomPadding);
    }

    private void ToggleShutter()
    {
        isOpen = !isOpen;

        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        float target = isOpen ? openHeight : closedHeight;
        animationCoroutine = StartCoroutine(AnimateShutter(target));
    }

    private IEnumerator AnimateShutter(float target)
    {
        float start = buildPanel.rect.height;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float progress = Mathf.Clamp01(time / duration);
            progress = progress * progress * (3f - 2f * progress);

            buildPanel.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Vertical,
                Mathf.Lerp(start, target, progress)
            );

            yield return null;
        }

        buildPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, target);
        animationCoroutine = null;
    }
}