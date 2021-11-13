using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public RandomMazeGenerator.Cell[,] cells;

    private int[] initialLocation;
    private List<int[]> path;
    private List<int[]> visitedCells;
    private int[] exitLocation;

    void Start()
    {
        cells = RandomMazeGenerator.instance.cells;
        initialLocation = GameManager.instance.SetInitialLocation();
        path = new List<int[]>();
        visitedCells = new List<int[]>();
        path.Add(initialLocation);
        visitedCells.Add(initialLocation);
        ChoosePath(initialLocation);
        //Debug.Log(initialLocation[0] + " " + initialLocation[1]);
    }

    void Update()
    {

    }

    public void ChoosePath(int[] location)
    {
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

        foreach (int[] cell in availableCells)
        {

        }

    }

    public void chooseAction()
    {

    }

}
