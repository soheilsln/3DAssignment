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
        //Debug.Log(initialLocation[0] + " " + initialLocation[1]);
    }

    void Update()
    {
        
    }

    public void choosePath(int iLocation, int jLocation)
    {
        
    }

    public void chooseAction()
    {

    }

}
