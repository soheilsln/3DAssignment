using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIManager : MonoBehaviour
{

    public RandomMazeGenerator.Cell[,] cells;
    private RandomMazeGenerator randomMazeGenerator;
    private GameManager gameManager;

    private int[] currentLocation;
    private List<int[]> path;
    private List<int[]> visitedCells;
    private Stack<int[]> exitPath;
    private bool exitFound = false;
    private bool keyFound = false;
    private bool reachedDestination = true;
    private int[] exitLocation;
    private int[] keyLocation1;
    private int[] keyLocation2;
    private bool isWalking = false;

    private const float runMoveDuration = 1.5f;
    private const float walkMoveDuration = 1.5f;
    private const float runSpeedAnimation = 5.335f;
    private const float walkSpeedAnimation = 2f;

    private Animator animator;
    

    private void Awake()
    {
        randomMazeGenerator = RandomMazeGenerator.instance;
        gameManager = GameManager.instance;
        animator = GetComponent<Animator>();
        animator.SetFloat("MotionSpeed", 1f);
    }

    void Start()
    {
        cells = randomMazeGenerator.cells;
        currentLocation = GameManager.instance.initialLocation;
        exitLocation = GameManager.instance.exitLocation;
        keyLocation1 = GameManager.instance.keyLocation1;
        keyLocation2 = GameManager.instance.keyLocation2;
        path = new List<int[]>();
        visitedCells = new List<int[]>();
        exitPath = new Stack<int[]>();
        exitPath.Push(exitLocation);
        path.Add(currentLocation);
        visitedCells.Add(currentLocation);

    }

    void Update()
    {
        if (reachedDestination)
        {
            ChooseAction();
        }
    }

    public void ChoosePath(int[] location)
    {
        reachedDestination = false;
        List<int> availablePaths = new List<int>();
        List<int[]> availableCells = new List<int[]>();
        List<int[]> unvisitedAvailableCells = new List<int[]>();

        for (int i = 0; i < 4; i++)
        {
            if (!cells[location[0], location[1]].hasWalls[i])
            {
                availablePaths.Add(i);
            }
        }

        foreach (int path in availablePaths)
        {
            if (path == 0)
            {
                availableCells.Add(new int[] { location[0] - 1, location[1] });
            }
            else if (path == 1)
            {
                availableCells.Add(new int[] { location[0], location[1] - 1 });
            }
            else if (path == 2)
            {
                availableCells.Add(new int[] { location[0] + 1, location[1] });
            }
            else
            {
                availableCells.Add(new int[] { location[0], location[1] + 1 });
            }
        }

        foreach (int[] cell in availableCells)
        {
            if (!visitedCells.Any(p => p.SequenceEqual(cell)))
            {
                unvisitedAvailableCells.Add(cell);
            }
        }

        if (unvisitedAvailableCells.Count == 0)
        {
            MoveBackward(path[path.Count - 2]);
        }
        else
        {
            int selectedCellIndex = Random.Range(0, unvisitedAvailableCells.Count);
            MoveForward(unvisitedAvailableCells[selectedCellIndex]);
        }
    }

    public void ChoosePathToExit()
    {
        reachedDestination = false;
        currentLocation = exitPath.Pop();
        StartCoroutine(MoveToLocation(gameManager.ConvertCellToLocation(currentLocation,transform.position.y), 
            runMoveDuration));
    }

    private void MoveForward(int[] cell)
    {
        visitedCells.Add(cell);
        path.Add(cell);
        StartCoroutine(MoveToLocation(gameManager.ConvertCellToLocation(cell, transform.position.y), 
            runMoveDuration));
        currentLocation = cell;
    }

    private void MoveBackward(int[] cell)
    {
        path.Remove(path[path.Count - 1]);
        StartCoroutine(MoveToLocation(gameManager.ConvertCellToLocation(cell, transform.position.y), 
            runMoveDuration));
        currentLocation = cell;
    }

    private IEnumerator MoveToLocation(Vector3 location, float duration)
    {
        float time = 0f;
        Vector3 startPosition = transform.position;

        transform.LookAt(location);
        animator.SetFloat("Speed", 5.335f);

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, location, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = location;
        reachedDestination = true;
        animator.SetFloat("Speed", 0f);
    }

    private void Move()
    {
        if (!currentLocation.SequenceEqual(keyLocation1) && !currentLocation.SequenceEqual(keyLocation2) && !keyFound)
        {
            if (currentLocation.SequenceEqual(exitLocation))
            {
                exitFound = true;
            }

            ChoosePath(currentLocation);
        }
        else
        {
            keyFound = true;
            if (!currentLocation.SequenceEqual(exitLocation) && !exitFound)
            {
                ChoosePath(currentLocation);
            }
            else
            {
                exitFound = true;
            }
        }

        if (exitFound && !keyFound)
        {
            if (!exitPath.Contains(currentLocation))
            {
                exitPath.Push(currentLocation);
            }
            else
            {
                //Pop the wrong paths
                exitPath.Pop();
            }
        }

        if (exitFound && keyFound)
        {
            //Pop the current keyLocation
            if(currentLocation.SequenceEqual(keyLocation1) || currentLocation.SequenceEqual(keyLocation2))
            {
                exitPath.Pop();
            }

            if (!currentLocation.SequenceEqual(exitLocation))
            {
                ChoosePathToExit();
            }
        }

    }

    public void ChooseAction()
    {
        Move();
    }

    public void SetIsWalking(bool isWalking)
    {
        this.isWalking = isWalking;
    }

}
