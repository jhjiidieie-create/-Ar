using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BuildUIControllerV13 : MonoBehaviour
{
    [Header("Категории")]
    [SerializeField] private GameObject categoryGround;
    [SerializeField] private GameObject categoryRoad;
    [SerializeField] private GameObject categoryBuilding;

    [Header("Материалы")]
    [SerializeField] private GameObject itemGround;
    [SerializeField] private GameObject itemAsphalt;
    [SerializeField] private GameObject itemRoad;

    public string SelectedItem { get; private set; }

    private void Awake()
    {
        ShowGround();
    }

    private void Start()
    {
        AddButtonListener(categoryGround, ShowGround);
        AddButtonListener(categoryRoad, ShowRoad);
        AddButtonListener(categoryBuilding, ShowBuilding);

        AddButtonListener(itemGround, () => SelectItem("Ground"));
        AddButtonListener(itemAsphalt, () => SelectItem("Asphalt"));
        AddButtonListener(itemRoad, () => SelectItem("Road"));
    }

    private void AddButtonListener(GameObject target, UnityAction action)
    {
        if (target == null) return;

        Button button = target.GetComponent<Button>();
        if (button != null) button.onClick.AddListener(action);
    }

    private void ShowGround()
    {
        SetCategory(categoryGround);
        SetItems(true, false, false);
    }

    private void ShowRoad()
    {
        SetCategory(categoryRoad);
        SetItems(false, true, true);
    }

    private void ShowBuilding()
    {
        SetCategory(categoryBuilding);
        SetItems(false, false, false);
    }

    private void SetCategory(GameObject selectedCategory)
    {
        SetOutline(categoryGround, selectedCategory == categoryGround);
        SetOutline(categoryRoad, selectedCategory == categoryRoad);
        SetOutline(categoryBuilding, selectedCategory == categoryBuilding);
        SelectedItem = "";
    }

    private void SetOutline(GameObject category, bool active)
    {
        if (category == null) return;

        Outline outline = category.GetComponent<Outline>();
        if (outline != null) outline.enabled = active;
    }

    private void SetItems(bool ground, bool asphalt, bool road)
    {
        if (itemGround != null) itemGround.SetActive(ground);
        if (itemAsphalt != null) itemAsphalt.SetActive(asphalt);
        if (itemRoad != null) itemRoad.SetActive(road);
    }

    private void SelectItem(string item)
    {
        SelectedItem = item;
    }
}