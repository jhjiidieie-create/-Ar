using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BuildUIControllerV25 : MonoBehaviour
{
    [Header("Система строительства")]
    [SerializeField] private PlacementManager placementManager;

    [Header("Категории")]
    [SerializeField] private GameObject categoryGround;
    [SerializeField] private GameObject categoryDecoration;
    [SerializeField] private GameObject categoryBuilding;

    [Header("Карточки объектов каталога")]
    [SerializeField] private List<GameObject> itemButtons = new List<GameObject>();

    public string SelectedItem { get; private set; }

    private void Start()
    {
        AddButtonListener(categoryGround, ShowGround);
        AddButtonListener(categoryDecoration, ShowDecoration);
        AddButtonListener(categoryBuilding, ShowBuilding);

        for (int i = 0; i < itemButtons.Count; i++)
        {
            int index = i;
            AddButtonListener(itemButtons[i], () => SelectItem(index));
        }

        ShowGround();
    }

    private void AddButtonListener(GameObject target, UnityAction action)
    {
        if (!target) return;

        Button button = target.GetComponent<Button>();
        if (!button) button = target.GetComponentInChildren<Button>();
        if (button) button.onClick.AddListener(action);
    }

    private void ShowGround()
    {
        ShowCategory(categoryGround, BuildingSystem.BuildLevel.Ground);
    }

    private void ShowDecoration()
    {
        ShowCategory(categoryDecoration, BuildingSystem.BuildLevel.Decoration);
    }

    private void ShowBuilding()
    {
        ShowCategory(categoryBuilding, BuildingSystem.BuildLevel.Building);
    }

    private void ShowCategory(GameObject selectedCategory, BuildingSystem.BuildLevel level)
    {
        if (!placementManager) return;

        placementManager.SetLevel(level);
        SelectedItem = "";

        SetOutline(categoryGround, selectedCategory == categoryGround);
        SetOutline(categoryDecoration, selectedCategory == categoryDecoration);
        SetOutline(categoryBuilding, selectedCategory == categoryBuilding);

        for (int i = 0; i < itemButtons.Count; i++)
        {
            GameObject item = itemButtons[i];
            if (!item) continue;

            bool available = i < placementManager.GetItemCount();
            item.SetActive(available);
            SetOutline(item, false);

            if (available)
            {
                Text label = item.GetComponentInChildren<Text>();
                if (label) label.text = placementManager.GetItemName(i);
            }
        }
    }

    private void SelectItem(int index)
    {
        if (!placementManager || index >= placementManager.GetItemCount()) return;

        placementManager.SelectItem(index);
        SelectedItem = placementManager.GetItemName(index);

        for (int i = 0; i < itemButtons.Count; i++)
            SetOutline(itemButtons[i], i == index);
    }

    private void SetOutline(GameObject target, bool active)
    {
        if (!target) return;

        Outline outline = target.GetComponent<Outline>();
        if (outline) outline.enabled = active;
    }
}