using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public float moveDuration = 3f;

    public RandomMazeGenerator.Cell[,] cells;
    private RandomMazeGenerator randomMazeGenerator;

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

    //private Animator animator;
    //// animation IDs
    //private int _animIDSpeed;
    //private int _animIDGrounded;
    //private int _animIDMotionSpeed;

    private void Awake()
    {
        randomMazeGenerator = RandomMazeGenerator.instance;
    }

    void Start()
    {
        cells = RandomMazeGenerator.instance.cells;
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

        //_animIDSpeed = Animator.StringToHash("Speed");
        //_animIDGrounded = Animator.StringToHash("Grounded");
        //_animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        //animator.SetBool(_animIDGrounded, true);
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
        StartCoroutine(MoveToLocation(ConvertCellToLocation(currentLocation), moveDuration));
    }

    private void MoveForward(int[] cell)
    {
        visitedCells.Add(cell);
        path.Add(cell);
        StartCoroutine(MoveToLocation(ConvertCellToLocation(cell), moveDuration));
        currentLocation = cell;
    }

    private void MoveBackward(int[] cell)
    {
        path.Remove(path[path.Count - 1]);
        StartCoroutine(MoveToLocation(ConvertCellToLocation(cell), moveDuration));
        currentLocation = cell;
    }

    private Vector3 ConvertCellToLocation(int[] cell)
    {
        int scale = randomMazeGenerator.GetScale();
        return new Vector3((cell[0] + 0.5f) * scale, 0f, (cell[1] + 0.5f) * scale);
    }

    private IEnumerator MoveToLocation(Vector3 location, float duration)
    {
        float time = 0f;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, location, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = location;
        reachedDestination = true;
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

}
