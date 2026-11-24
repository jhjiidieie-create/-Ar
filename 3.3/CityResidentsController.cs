using System.Collections.Generic;
using UnityEngine;

public class CityResidentsController : MonoBehaviour
{
    public GameObject residentPrefab;
    public float moveSpeed = 0.03f;
    public float heightOffset;

    private GridManager grid;
    private GameObject[] roads;
    private readonly List<GameObject> residents = new List<GameObject>();
    private readonly List<Transform> currentRoads = new List<Transform>();
    private readonly List<Transform> targetRoads = new List<Transform>();
    private readonly List<Transform> previousRoads = new List<Transform>();

    private void Awake()
    {
        grid = GetComponent<GridManager>();
    }

    public void StartResidents()
    {
        StopResidents();

        if (residentPrefab == null || grid == null)
            return;

        roads = GameObject.FindGameObjectsWithTag("Road");
        if (roads.Length == 0)
            return;

        int residentCount = GameObject.FindGameObjectsWithTag("Building").Length;

        for (int i = 0; i < residentCount; i++)
        {
            Transform road = roads[Random.Range(0, roads.Length)].transform;
            Vector3 position = road.position + Vector3.up * heightOffset;

            residents.Add(Instantiate(residentPrefab, position, Quaternion.identity, transform));
            currentRoads.Add(road);
            targetRoads.Add(GetNextRoad(road, null));
            previousRoads.Add(null);
        }
    }

    private void Update()
    {
        for (int i = 0; i < residents.Count; i++)
        {
            if (residents[i] == null || targetRoads[i] == null)
                continue;

            Vector3 target = targetRoads[i].position + Vector3.up * heightOffset;
            Transform resident = residents[i].transform;

            resident.position = Vector3.MoveTowards(resident.position, target, moveSpeed * Time.deltaTime);

            Vector3 direction = target - resident.position;
            direction.y = 0;

            if (direction.sqrMagnitude > 0.000001f)
                resident.rotation = Quaternion.LookRotation(direction);

            if (Vector3.Distance(resident.position, target) < 0.001f)
            {
                previousRoads[i] = currentRoads[i];
                currentRoads[i] = targetRoads[i];
                targetRoads[i] = GetNextRoad(currentRoads[i], previousRoads[i]);
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
        previousRoads.Clear();
    }

    private Transform GetNextRoad(Transform road, Transform previousRoad)
    {
        List<Transform> neighbours = new List<Transform>();

        foreach (GameObject item in roads)
        {
            if (item == null)
                continue;

            Transform candidate = item.transform;
            float distance = Vector2.Distance(
                new Vector2(road.position.x, road.position.z),
                new Vector2(candidate.position.x, candidate.position.z));

            if (candidate != road && Mathf.Abs(distance - grid.cellSize) < grid.cellSize * 0.15f)
                neighbours.Add(candidate);
        }

        if (neighbours.Count > 1)
            neighbours.Remove(previousRoad);

        return neighbours.Count == 0 ? road : neighbours[Random.Range(0, neighbours.Count)];
    }
}