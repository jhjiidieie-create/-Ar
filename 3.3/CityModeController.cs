using UnityEngine;

public class CityModeController : MonoBehaviour
{
    public GameObject buildModePanel;
    public GameObject cityModePanel;
    public CityResidentsController residents;

    public void StartCity()
    {
        buildModePanel.SetActive(false);
        cityModePanel.SetActive(true);
        residents.StartResidents();
    }

    public void EditCity()
    {
        cityModePanel.SetActive(false);
        buildModePanel.SetActive(true);
        residents.StopResidents();
    }
}
