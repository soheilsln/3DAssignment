using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public RandomMazeGenerator.Cell[,] cells;

    private int[] currentLocation;
    private List<int[]> path;
    private List<int[]> visitedCells;
    private List<int[]> exitPath;
    private bool exitFound = false;
    private bool keyFound = false;
    private int[] exitLocation;
    private int[] keyLocation1;
    private int[] keyLocation2;

    void Start()
    {
        cells = RandomMazeGenerator.instance.cells;
        currentLocation = GameManager.instance.initialLocation;
        exitLocation = GameManager.instance.exitLocation;
        keyLocation1 = GameManager.instance.keyLocation1;
        keyLocation2 = GameManager.instance.keyLocation2;
        path = new List<int[]>();
        visitedCells = new List<int[]>();
        exitPath = new List<int[]>();
        path.Add(currentLocation);
        visitedCells.Add(currentLocation);


        while (!currentLocation.SequenceEqual(keyLocation1) && !currentLocation.SequenceEqual(keyLocation2))
        {
            if(currentLocation.SequenceEqual(exitLocation))
            {
                //exitPath.Add(currentLocation);
                exitFound = true;
            }
            ChoosePath(currentLocation);
        }
        keyFound = true;
        while (!currentLocation.SequenceEqual(exitLocation) && !exitFound)
        {
            ChoosePath(currentLocation);
        }
        exitFound = true;


    }

    void Update()
    {

    }

    public void ChoosePath(int[] location)
    {
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

    private void MoveForward(int[] cell)
    {
        //Debug.Log("moved to " + (cell[0] + 1) + "," + (cell[1] + 1));
        visitedCells.Add(cell);
        path.Add(cell);
        currentLocation = cell;
    }

    private void MoveBackward(int[] cell)
    {
        //Debug.Log("moved to " + (cell[0] + 1) + "," + (cell[1] + 1));
        path.Remove(path[path.Count - 1]);
        currentLocation = cell;
    }

    public void chooseAction()
    {

    }

}
