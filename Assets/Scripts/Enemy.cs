using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public RandomMazeGenerator.Cell[,] cells;
    private RandomMazeGenerator randomMazeGenerator;
    public float moveDuration = 10f;
    private bool reachedDestination = true;
    private int[] currentLocation;


    protected virtual void Awake()
    {
        randomMazeGenerator = RandomMazeGenerator.instance;
    }

    protected virtual void Start()
    {
        cells = randomMazeGenerator.cells;
        SetCurrentLocation(GameManager.instance.initialLocation);//GameManager Job
    }

    protected virtual void Update()
    {
        if (reachedDestination)
        {
            ChoosePath(currentLocation);
        }
    }

    private IEnumerator MoveToLocation(Vector3 location, float duration)
    {
        float time = 0f;
        Vector3 startPosition = transform.position;

        transform.LookAt(location);

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, location, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = location;
        reachedDestination = true;
    }

    private void Move(int[] cell)
    {
        StartCoroutine(MoveToLocation(ConvertCellToLocation(cell), moveDuration));
        currentLocation = cell;
    }

    private Vector3 ConvertCellToLocation(int[] cell)
    {
        int scale = randomMazeGenerator.GetScale();
        return new Vector3((cell[0] + 0.5f) * scale, transform.position.y, (cell[1] + 0.5f) * scale);
    }

    public void ChoosePath(int[] location)
    {
        reachedDestination = false;
        List<int> availablePaths = new List<int>();
        List<int[]> availableCells = new List<int[]>();

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

        int rnd = Random.Range(0, availableCells.Count);
        Move(availableCells[rnd]);

    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public void SetCurrentLocation(int[] newCurrentLocation)
    {
        currentLocation = newCurrentLocation;
    }
}
