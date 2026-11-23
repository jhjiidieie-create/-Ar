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
        constructionButton.onClick.AddListener(ShowConstruction);
        decorationButton.onClick.AddListener(ShowDecoration);
        ShowConstruction();
    }

    void ShowConstruction()
    {
        constructionItems.SetActive(true);
        decorationItems.SetActive(false);
    }

    void ShowDecoration()
    {
        constructionItems.SetActive(false);
        decorationItems.SetActive(true);
    }
}