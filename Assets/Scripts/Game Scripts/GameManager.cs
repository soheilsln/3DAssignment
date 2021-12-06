using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public RandomMazeGenerator.Cell[,] cells;
    private RandomMazeGenerator randomMazeGenerator;
    private int winner = 0;

    [HideInInspector]
    public int[] initialLocation;
    [HideInInspector]
    public int[] exitLocation;
    [HideInInspector]
    public int[] keyLocation1;
    [HideInInspector]
    public int[] keyLocation2;
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public GameObject AI;
    [HideInInspector]
    public List<int[]> enemiesLocations;
    public GameObject[] enemiesPrefabs;
    public GameObject keyPrefab;
    public GameObject doorPrefab;
    [HideInInspector]
    public GameUIManager uIManager;


    public float fireDuration;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        player = FindObjectOfType<PlayerController>().gameObject;
        AI = FindObjectOfType<AIController>().gameObject;
        uIManager = FindObjectOfType<GameUIManager>();
        randomMazeGenerator = RandomMazeGenerator.instance;
        Physics.IgnoreCollision(player.GetComponent<Collider>(), AI.GetComponent<Collider>());
        winner = 0;
    }
    void Start()
    {
        cells = RandomMazeGenerator.instance.cells;
        initialLocation = SetInitialLocation();
        InstantiateKeys();
        InstantiateDoor();
        InstantiateEnemies();
    }

    void Update()
    {

    }

    private int[] SetInitialLocation()
    {
        return new int[] { 0, 0 };
    }

    private int[] SetExitLocation()
    {
        return new int[] { cells.GetLength(0) - 1, cells.GetLength(1) - 1 };
    }

    private int[] SetKeysLocations(int number)
    {
        if (number == 1)
        {
            return new int[] { 0, cells.GetLength(1) - 1 };
        }
        else
        {
            return new int[] { cells.GetLength(0) - 1, 0 };
        }
    }

    private List<int[]> SetEnemiesLocations()
    {
        List<int[]> locations = new List<int[]>();

        int numberOfEnemiesWidth = cells.GetLength(0) / 3;
        int numberOfEnemiesLenght = cells.GetLength(1) / 3;

        for (int i = 0; i < numberOfEnemiesWidth; i++)
        {
            for (int j = 0; j < numberOfEnemiesLenght; j++)
            {
                if (i != 0 || j != 0)
                {
                    int rndWidth = Random.Range(i * 3, i * 3 + 3);
                    int rndLenght = Random.Range(j * 3, j * 3 + 3);
                    locations.Add(new int[] { rndWidth, rndLenght });
                }
            }
        }

        return locations;
    }

    private void InstantiateKeys()
    {
        keyLocation1 = SetKeysLocations(1);
        keyLocation2 = SetKeysLocations(2);

        GameObject key1 = Instantiate(keyPrefab, ConvertCellToLocation(keyLocation1,
                keyPrefab.transform.position.y), keyPrefab.transform.rotation);
        key1.name = "Key 1";

        GameObject key2 = Instantiate(keyPrefab, ConvertCellToLocation(keyLocation2,
                keyPrefab.transform.position.y), keyPrefab.transform.rotation);
        key2.name = "Key 2";
    }

    private void InstantiateDoor()
    {
        exitLocation = SetExitLocation();
        GameObject door = Instantiate(doorPrefab, ConvertCellToLocation(exitLocation,
                doorPrefab.transform.position.y), doorPrefab.transform.rotation);
        door.transform.position = new Vector3(door.transform.position.x + randomMazeGenerator.GetScale() / 4f,
            door.transform.position.y, door.transform.position.x + randomMazeGenerator.GetScale() / 4f);
        door.name = "Door";
    }

    private void InstantiateEnemies()
    {
        enemiesLocations = SetEnemiesLocations();
        GameObject enemies = new GameObject();
        enemies.name = "Enemies";
        foreach (int[] enemyLocation in enemiesLocations)
        {
            int rnd = Random.Range(0, enemiesPrefabs.Length);
            Instantiate(enemiesPrefabs[rnd], ConvertCellToLocation(enemyLocation,
                enemiesPrefabs[rnd].transform.position.y), Quaternion.identity, enemies.transform);
        }
    }

    public Vector3 ConvertCellToLocation(int[] cell, float distanceToFloor)
    {
        int scale = randomMazeGenerator.GetScale();
        return new Vector3((cell[0] + 0.5f) * scale, distanceToFloor, (cell[1] + 0.5f) * scale);
    }

    public int[] ConvertLocationToCell(Vector3 location)
    {
        int scale = randomMazeGenerator.GetScale();
        int[] cell = new int[2];
        cell[0] = Mathf.FloorToInt((location.x / (float)scale));
        cell[1] = Mathf.FloorToInt((location.z / (float)scale));
        return cell;
    }

    public float GetFireDuration()
    {
        return fireDuration;
    }

    public void SetWinner(int winnerID)
    {
        winner = winnerID;
    }

    public int GetWinner()
    {
        return winner;
    }
}
