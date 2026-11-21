using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BuildUIControllerV26 : MonoBehaviour
{
    [Header("Кнопки категорий")]
    [SerializeField] GameObject categoryGround;
    [SerializeField] GameObject categoryDecoration;
    [SerializeField] GameObject categoryBuilding;

    [Header("Панели с кнопками материалов")]
    [SerializeField] GameObject groundPanel;
    [SerializeField] GameObject decorationPanel;
    [SerializeField] GameObject buildingPanel;

    void Start()
    {
        AddButtonListener(categoryGround, ShowGround);
        AddButtonListener(categoryDecoration, ShowDecoration);
        AddButtonListener(categoryBuilding, ShowBuilding);

        ShowGround();
    }

    void AddButtonListener(GameObject target, UnityAction action)
    {
        if (!target) return;

        Button button = target.GetComponent<Button>();
        if (!button) button = target.GetComponentInChildren<Button>();

        if (button)
            button.onClick.AddListener(action);
    }

    void ShowGround()
    {
        if (groundPanel) groundPanel.SetActive(true);
        if (decorationPanel) decorationPanel.SetActive(false);
        if (buildingPanel) buildingPanel.SetActive(false);
    }

    void ShowDecoration()
    {
        if (groundPanel) groundPanel.SetActive(false);
        if (decorationPanel) decorationPanel.SetActive(true);
        if (buildingPanel) buildingPanel.SetActive(false);
    }

    void ShowBuilding()
    {
        if (groundPanel) groundPanel.SetActive(false);
        if (decorationPanel) decorationPanel.SetActive(false);
        if (buildingPanel) buildingPanel.SetActive(true);
    }
}