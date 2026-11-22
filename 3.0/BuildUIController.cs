using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BuildUIController : MonoBehaviour
{
    [SerializeField] GameObject categoryGround;
    [SerializeField] GameObject categoryDecoration;
    [SerializeField] GameObject categoryBuilding;
    [SerializeField] Transform itemsContainer;

    BuildItemButton[] itemButtons;
    BuildItemButton selectedItem;

    void Awake()
    {
        if (itemsContainer)
            itemButtons = itemsContainer.GetComponentsInChildren<BuildItemButton>(true);
    }

    void Start()
    {
        AddButtonListener(categoryGround, () => ShowCategory(BuildingSystem.BuildLevel.Ground));
        AddButtonListener(categoryDecoration, () => ShowCategory(BuildingSystem.BuildLevel.Decoration));
        AddButtonListener(categoryBuilding, () => ShowCategory(BuildingSystem.BuildLevel.Building));
        ShowCategory(BuildingSystem.BuildLevel.Ground);
    }

    void AddButtonListener(GameObject target, UnityAction action)
    {
        if (!target) return;
        Button button = target.GetComponent<Button>();
        if (!button) button = target.GetComponentInChildren<Button>();
        if (button) button.onClick.AddListener(action);
    }

    void ShowCategory(BuildingSystem.BuildLevel level)
    {
        SetOutline(categoryGround, level == BuildingSystem.BuildLevel.Ground);
        SetOutline(categoryDecoration, level == BuildingSystem.BuildLevel.Decoration);
        SetOutline(categoryBuilding, level == BuildingSystem.BuildLevel.Building);

        if (selectedItem) selectedItem.SetSelected(false);
        selectedItem = null;

        if (itemButtons == null) return;

        foreach (BuildItemButton item in itemButtons)
        {
            if (!item) continue;
            item.SetSelected(false);
            item.gameObject.SetActive(item.Level == level);
        }
    }

    public void SelectItem(BuildItemButton item)
    {
        if (selectedItem) selectedItem.SetSelected(false);
        selectedItem = item;
        if (selectedItem) selectedItem.SetSelected(true);
    }

    void SetOutline(GameObject target, bool active)
    {
        if (!target) return;
        Outline outline = target.GetComponent<Outline>();
        if (!outline) outline = target.GetComponentInChildren<Outline>(true);
        if (outline) outline.enabled = active;
    }
}