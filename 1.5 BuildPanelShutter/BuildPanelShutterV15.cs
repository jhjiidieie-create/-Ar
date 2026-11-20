using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BuildPanelShutterV15 : MonoBehaviour
{
    [SerializeField] private Button handleButton;
    [SerializeField] private RectTransform itemsContainer;
    [SerializeField] private float hideDistance = 300f;
    [SerializeField] private float duration = 0.35f;

    private Vector2 shownPosition;
    private Vector2 hiddenPosition;
    private bool isOpen = true;
    private Coroutine animationCoroutine;

    private void Start()
    {
        shownPosition = itemsContainer.anchoredPosition;
        hiddenPosition = shownPosition + Vector2.down * hideDistance;

        handleButton.onClick.AddListener(ToggleShutter);
    }

    private void ToggleShutter()
    {
        isOpen = !isOpen;

        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        Vector2 target = isOpen ? shownPosition : hiddenPosition;
        animationCoroutine = StartCoroutine(AnimateShutter(target));
    }

    private IEnumerator AnimateShutter(Vector2 target)
    {
        Vector2 start = itemsContainer.anchoredPosition;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float progress = Mathf.Clamp01(time / duration);
            progress = progress * progress * (3f - 2f * progress);

            itemsContainer.anchoredPosition = Vector2.Lerp(start, target, progress);
            yield return null;
        }

        itemsContainer.anchoredPosition = target;
        animationCoroutine = null;
    }
}