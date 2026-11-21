namespace ARBuildV27
{
    using UnityEngine;
    using UnityEngine.UI;

    public class BuildItemButton : MonoBehaviour
    {
        [SerializeField] PlacementManager placementManager;
        [SerializeField] GameObject prefab;
        [SerializeField] BuildingSystem.BuildLevel level;

        public BuildingSystem.BuildLevel Level => level;

        void Start()
        {
            Button button = GetComponent<Button>();
            if (!button) button = GetComponentInChildren<Button>();

            if (button)
                button.onClick.AddListener(Place);
        }

        void Place()
        {
            if (placementManager && prefab)
                placementManager.PlaceObject(prefab, level);
        }
    }
}