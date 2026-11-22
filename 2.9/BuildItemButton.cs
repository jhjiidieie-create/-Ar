    using UnityEngine;
    using UnityEngine.UI;

    public class BuildItemButton : MonoBehaviour
    {
        [SerializeField] PlacementManager placementManager;
        [SerializeField] BuildUIController uiController;
        [SerializeField] GameObject prefab;
        [SerializeField] BuildingSystem.BuildLevel level;

        public BuildingSystem.BuildLevel Level => level;

        void Start()
        {
            Button button = GetComponent<Button>();
            if (!button) button = GetComponentInChildren<Button>();

            if (button)
                button.onClick.AddListener(SelectAndPlace);
        }

        void SelectAndPlace()
        {
            if (uiController)
                uiController.SelectItem(this);

            if (placementManager && prefab)
                placementManager.PlaceObject(prefab, level);
        }

        public void SetSelected(bool selected)
        {
            Outline outline = GetComponent<Outline>();
            if (!outline) outline = GetComponentInChildren<Outline>(true);
            if (outline) outline.enabled = selected;
        }
    }
