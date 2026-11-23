using UnityEngine;
using UnityEngine.UI;

public class BuildUIController : MonoBehaviour
{
    public Button constructionButton;
    public Button decorationButton;
    public GameObject constructionItems;
    public GameObject decorationItems;

    void Start()
    {
        constructionButton.onClick.AddListener(() => ShowItems(true));
        decorationButton.onClick.AddListener(() => ShowItems(false));
        ShowItems(true);
    }

    void ShowItems(bool construction)
    {
        constructionItems.SetActive(construction);
        decorationItems.SetActive(!construction);
    }
}