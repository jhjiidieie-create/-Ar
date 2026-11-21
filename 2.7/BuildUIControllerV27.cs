namespace ARBuildV27
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Events;

    public class BuildUIControllerV27 : MonoBehaviour
    {
        [Header("Кнопки категорий")]
        [SerializeField] GameObject categoryGround;
        [SerializeField] GameObject categoryDecoration;
        [SerializeField] GameObject categoryBuilding;

        [Header("Единый контейнер всех материалов")]
        [SerializeField] Transform itemsContainer;

        BuildItemButton[] itemButtons;

        void Awake()
        {
            if (itemsContainer)
                itemButtons = itemsContainer.GetComponentsInChildren<BuildItemButton>(true);
        }

        void Start()
        {
            AddButtonListener(categoryGround, ShowGround);
            AddButtonListener(categoryDecoration, ShowDecoration);
            AddButtonListener(categoryBuilding, ShowBuilding);
            ShowCategory(BuildingSystem.BuildLevel.Ground);
        }

        void AddButtonListener(GameObject target, UnityAction action)
        {
            if (!target) return;

            Button button = target.GetComponent<Button>();
            if (!button) button = target.GetComponentInChildren<Button>();

            if (button)
                button.onClick.AddListener(action);
        }

        void ShowGround() => ShowCategory(BuildingSystem.BuildLevel.Ground);
        void ShowDecoration() => ShowCategory(BuildingSystem.BuildLevel.Decoration);
        void ShowBuilding() => ShowCategory(BuildingSystem.BuildLevel.Building);

        void ShowCategory(BuildingSystem.BuildLevel level)
        {
            if (itemButtons == null) return;

            foreach (BuildItemButton item in itemButtons)
                if (item)
                    item.gameObject.SetActive(item.Level == level);
        }
    }
}