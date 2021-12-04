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

    private int[] SetInitialLocation()
    {
        /*
        int width = cells.GetLength(0);
        int lenght = cells.GetLength(1);

        int locationIndex = Random.Range(0, 4);

        switch (locationIndex)
        {
            case 0:
                return new int[] { 0, 0 };
            case 1:
                return new int[] { 0, width - 1 };
            case 2:
                return new int[] { 0, lenght - 1 };
            default:
                return new int[] { width - 1, lenght - 1 };
        }
        */

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

    public Vector3 ConvertCellToLocation(int[] cell, float y)
    {
        int scale = randomMazeGenerator.GetScale();
        return new Vector3((cell[0] + 0.5f) * scale, y, (cell[1] + 0.5f) * scale);
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

    void Start()
    {
        cells = RandomMazeGenerator.instance.cells;
        initialLocation = SetInitialLocation();
        exitLocation = SetExitLocation();
        keyLocation1 = SetKeysLocations(1);
        keyLocation2 = SetKeysLocations(2);
    }

    void Update()
    {

    }

}
