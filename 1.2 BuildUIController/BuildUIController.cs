using UnityEngine;
using UnityEngine.UI;

public class BuildUIController : MonoBehaviour
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

    private void AddButtonListener(GameObject target, UnityEngine.Events.UnityAction action)
    {
        if (target == null) return;

        Button button = target.GetComponent<Button>();
        if (button != null) button.onClick.AddListener(action);
    }

    private void ShowGround()
    {
        if (itemGround != null) itemGround.SetActive(true);
        if (itemAsphalt != null) itemAsphalt.SetActive(false);
        if (itemRoad != null) itemRoad.SetActive(false);
        SelectedItem = "";
    }

    private void ShowRoad()
    {
        if (itemGround != null) itemGround.SetActive(false);
        if (itemAsphalt != null) itemAsphalt.SetActive(true);
        if (itemRoad != null) itemRoad.SetActive(true);
        SelectedItem = "";
    }

    private void ShowBuilding()
    {
        if (itemGround != null) itemGround.SetActive(false);
        if (itemAsphalt != null) itemAsphalt.SetActive(false);
        if (itemRoad != null) itemRoad.SetActive(false);
        SelectedItem = "";
    }

    private void SelectItem(string item)
    {
        SelectedItem = item;
    }
}