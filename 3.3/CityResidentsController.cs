using System.Collections.Generic;
using UnityEngine;

public class CityResidentsController : MonoBehaviour
{
    public GameObject residentPrefab;
    public int residentCount = 5;
    public float moveSpeed = 0.03f;
    public float heightOffset;

    private GridManager grid;
    private readonly List<GameObject> residents = new();
    private readonly List<Transform> currentRoads = new();
    private readonly List<Transform> targetRoads = new();

    private void Awake()
    {
        grid = GetComponent<GridManager>();
    }

    public void StartResidents()
    {
        StopResidents();

        if (residentPrefab == null || grid == null)
            return;

        GameObject[] roads = GameObject.FindGameObjectsWithTag("Road");
        if (roads.Length == 0)
            return;

        for (int i = 0; i < residentCount; i++)
        {
            Transform road = roads[Random.Range(0, roads.Length)].transform;
            Vector3 position = road.position + Vector3.up * heightOffset;

            GameObject resident = Instantiate(residentPrefab, position, Quaternion.identity, transform);
            residents.Add(resident);
            currentRoads.Add(road);
            targetRoads.Add(GetNextRoad(road, null, roads));
        }
    }

    private void Update()
    {
        for (int i = 0; i < residents.Count; i++)
        {
            if (residents[i] == null || targetRoads[i] == null)
                continue;

            Vector3 target = targetRoads[i].position + Vector3.up * heightOffset;
            residents[i].transform.position = Vector3.MoveTowards(
                residents[i].transform.position, target, moveSpeed * Time.deltaTime);

            Vector3 direction = target - residents[i].transform.position;
            direction.y = 0;

            if (direction.sqrMagnitude > 0.000001f)
                residents[i].transform.rotation = Quaternion.LookRotation(direction);

            if (Vector3.Distance(residents[i].transform.position, target) < 0.001f)
            {
                currentRoads[i] = targetRoads[i];
                GameObject[] roads = GameObject.FindGameObjectsWithTag("Road");
                targetRoads[i] = GetNextRoad(currentRoads[i], null, roads);
            }
        }
    }

    public void StopResidents()
    {
        foreach (GameObject resident in residents)
            if (resident != null)
                Destroy(resident);

        residents.Clear();
        currentRoads.Clear();
        targetRoads.Clear();
    }

    private Transform GetNextRoad(Transform road, Transform previousRoad, GameObject[] roads)
    {
        List<Transform> neighbours = new();

        foreach (GameObject item in roads)
        {
            Transform candidate = item.transform;
            float distance = Vector2.Distance(
                new Vector2(road.position.x, road.position.z),
                new Vector2(candidate.position.x, candidate.position.z));

            if (candidate != road && Mathf.Abs(distance - grid.cellSize) < grid.cellSize * 0.15f)
                neighbours.Add(candidate);
        }

        if (neighbours.Count == 0)
            return road;

        return neighbours[Random.Range(0, neighbours.Count)];
    }
}