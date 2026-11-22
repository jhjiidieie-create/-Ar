    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Events;

    public class BuildUIController : MonoBehaviour
    {
        [Header("Кнопки категорий")]
        [SerializeField] GameObject categoryGround;
        [SerializeField] GameObject categoryDecoration;
        [SerializeField] GameObject categoryBuilding;

        [Header("Единый контейнер материалов")]
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
            SetOutline(categoryGround, level == BuildingSystem.BuildLevel.Ground);
            SetOutline(categoryDecoration, level == BuildingSystem.BuildLevel.Decoration);
            SetOutline(categoryBuilding, level == BuildingSystem.BuildLevel.Building);

            if (selectedItem)
                selectedItem.SetSelected(false);
            selectedItem = null;

            if (itemButtons == null) return;

            foreach (BuildItemButton item in itemButtons)
                if (item)
                {
                    item.SetSelected(false);
                    item.gameObject.SetActive(item.Level == level);
                }
        }

        public void SelectItem(BuildItemButton item)
        {
            if (selectedItem)
                selectedItem.SetSelected(false);

            selectedItem = item;

            if (selectedItem)
                selectedItem.SetSelected(true);
        }

        void SetOutline(GameObject target, bool active)
        {
            if (!target) return;

            Outline outline = target.GetComponent<Outline>();
            if (!outline) outline = target.GetComponentInChildren<Outline>(true);
            if (outline) outline.enabled = active;
        }
    }
