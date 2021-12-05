using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public RandomMazeGenerator.Cell[,] cells;
    private RandomMazeGenerator randomMazeGenerator;

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


    public float fireDuration;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        if (instance == this)
        {
            DontDestroyOnLoad(this.gameObject);
        }

        player = FindObjectOfType<ThirdPersonShooterController>().gameObject;
        AI = FindObjectOfType<AIController>().gameObject;
        randomMazeGenerator = RandomMazeGenerator.instance;
        Physics.IgnoreCollision(player.GetComponent<Collider>(), AI.GetComponent<Collider>());
    }
    void Start()
    {
        cells = RandomMazeGenerator.instance.cells;
        initialLocation = SetInitialLocation();
        exitLocation = SetExitLocation();
        keyLocation1 = SetKeysLocations(1);
        keyLocation2 = SetKeysLocations(2);
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

    private void InstantiateEnemies()
    {
        enemiesLocations = SetEnemiesLocations();
        GameObject enemies = Instantiate(new GameObject());
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


}
