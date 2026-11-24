using UnityEngine;
using UnityEngine.UI;

public class WeatherUIController : MonoBehaviour
{
    public Button dayButton;
    public Button nightButton;
    public GameObject dayItems;
    public GameObject nightItems;

    private void Start()
    {
        dayButton.onClick.AddListener(() => ShowItems(true));
        nightButton.onClick.AddListener(() => ShowItems(false));
        ShowItems(true);
    }

    private void ShowItems(bool day)
    {
        dayItems.SetActive(day);
        nightItems.SetActive(!day);
    }
}
