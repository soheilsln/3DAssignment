using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomMazeGenerator : MonoBehaviour
{
    public static RandomMazeGenerator instance;

    [SerializeField]
    private int width = 10;
    [SerializeField]
    private int height = 10;

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
    }

    void Start()
    {
        if (width == 0)
        {
            width = 1;
        }
        if (height == 0)
        {
            height = 1;
        }

        GenerateRandomMaze(width, height);
    }

    private void GenerateRandomMaze(int width, int height)
    {
        int ILocation = 0;
        int JLocation = 0;

        //Initialize the array
        Cell[,] cells = new Cell[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                cells[i, j] = new Cell();
            }
        }

        //Backtracking Generator Algorithm
        //Start from a random cell
        ILocation = Random.Range(0, width);
        JLocation = Random.Range(0, height);
        VisitNewCell(cells, ILocation, JLocation);
        //
    }

    public void VisitNewCell(Cell[,] cells, int i, int j)
    {
        Debug.Log((i + 1) + " " + (j + 1));
        cells[i, j].isVisited = true;
        if (!HaveAdjacentAvailableCell(cells, i, j))
        {
            return;
        }
        while(HaveAdjacentAvailableCell(cells, i, j))
        {
            bool wallFound = false;
            while (!wallFound)
            {
                int selectedWall = Random.Range(0, 4);
                if (cells[i, j].hasWalls[selectedWall])
                {
                    if (selectedWall == 0 && i > 0 && !cells[i - 1, j].isVisited)
                    {
                        wallFound = true;
                        cells[i, j].hasWalls[selectedWall] = false;
                        VisitNewCell(cells, i - 1, j);
                    }
                    else if (selectedWall == 1 && j > 0 && !cells[i, j - 1].isVisited)
                    {
                        wallFound = true;
                        cells[i, j].hasWalls[selectedWall] = false;
                        VisitNewCell(cells, i, j - 1);
                    }
                    else if (selectedWall == 2 && i + 1 < width && !cells[i + 1, j].isVisited)
                    {
                        wallFound = true;
                        cells[i, j].hasWalls[selectedWall] = false;
                        VisitNewCell(cells, i + 1, j);
                    }
                    else if (selectedWall == 3 && j + 1 < height && !cells[i, j + 1].isVisited)
                    {
                        wallFound = true;
                        cells[i, j].hasWalls[selectedWall] = false;
                        VisitNewCell(cells, i, j + 1);
                    }
                }
            }
        }
    }

    public bool HaveAdjacentAvailableCell(Cell[,] cells, int i, int j)
    {
        if (cells[i, j].hasWalls.All(p => !p))
            return false;
        if (i > 0 && !cells[i - 1, j].isVisited)
            return true;
        else if (j > 0 && !cells[i, j - 1].isVisited)
            return true;
        else if (i + 1 < width && !cells[i + 1, j].isVisited)
            return true;
        else if (j + 1 < height && !cells[i, j + 1].isVisited)
            return true;
        return false;
    }

    public class Cell
    {
        public bool[] hasWalls = new bool[4]; //Left,Down,Right,Up
        public bool isVisited;

        public Cell()
        {
            for (int i = 0; i < 4; i++)
            {
                hasWalls[i] = true;
            }
            isVisited = false;
        }
    }

}
