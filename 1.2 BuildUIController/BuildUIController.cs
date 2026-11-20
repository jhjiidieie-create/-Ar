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

    private void Start()
    {
        categoryGround.GetComponent<Button>().onClick.AddListener(ShowGround);
        categoryRoad.GetComponent<Button>().onClick.AddListener(ShowRoad);
        categoryBuilding.GetComponent<Button>().onClick.AddListener(ShowBuilding);

        itemGround.GetComponent<Button>().onClick.AddListener(() => SelectItem("Ground"));
        itemAsphalt.GetComponent<Button>().onClick.AddListener(() => SelectItem("Asphalt"));
        itemRoad.GetComponent<Button>().onClick.AddListener(() => SelectItem("Road"));

        ShowGround();
    }

    private void ShowGround()
    {
        itemGround.SetActive(true);
        itemAsphalt.SetActive(false);
        itemRoad.SetActive(false);
        SelectedItem = "";
    }

    private void ShowRoad()
    {
        itemGround.SetActive(false);
        itemAsphalt.SetActive(true);
        itemRoad.SetActive(true);
        SelectedItem = "";
    }

    private void ShowBuilding()
    {
        itemGround.SetActive(false);
        itemAsphalt.SetActive(false);
        itemRoad.SetActive(false);
        SelectedItem = "";
    }

    private void SelectItem(string item)
    {
        SelectedItem = item;
    }
}
