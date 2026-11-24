using UnityEngine;

public class CityEnvironmentController : MonoBehaviour
{
    public GameObject sunPrefab;
    public GameObject moonPrefab;
    public GameObject cloudPrefab;
    public GameObject rainPrefab;
    public GameObject snowPrefab;

    public Vector3 skyOffset = new Vector3(0, 0.3f, 0);
    public Vector3 weatherOffset = new Vector3(0, 0.1f, 0);

    private GameObject timeObject;
    private GameObject weatherObject;

    public void SetDay() => SetTime(sunPrefab);
    public void SetNight() => SetTime(moonPrefab);

    public void SetClouds() => SetWeather(cloudPrefab);
    public void SetRain() => SetWeather(rainPrefab);
    public void SetSnow() => SetWeather(snowPrefab);

    private void SetTime(GameObject prefab)
    {
        if (timeObject != null)
            Destroy(timeObject);

        if (prefab != null)
            timeObject = Instantiate(prefab, transform.position + skyOffset, Quaternion.identity, transform);
    }

    private void SetWeather(GameObject prefab)
    {
        if (weatherObject != null)
            Destroy(weatherObject);

        if (prefab != null)
            weatherObject = Instantiate(prefab, transform.position + weatherOffset, Quaternion.identity, transform);
    }
}
