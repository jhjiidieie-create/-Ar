namespace ARBuildV27
{
    using UnityEngine;

    public class RemovalManager : MonoBehaviour
    {
        [SerializeField] GridSelector selector;
        [SerializeField] BuildingSystem buildingSystem;

        void Awake()
        {
            if (!buildingSystem)
                buildingSystem = FindObjectOfType<BuildingSystem>();
        }

        public void RemoveSelected()
        {
            if (!selector || !buildingSystem || selector.SelectedCells.Count == 0) return;

            foreach (Vector2Int cell in selector.SelectedCells)
                buildingSystem.RemoveTop(cell);

            selector.ClearSelection();
        }
    }
}