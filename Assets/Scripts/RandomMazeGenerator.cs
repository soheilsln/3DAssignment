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
    [HideInInspector]
    public Cell[,] cells;

    public GameObject floorPrefab;
    public GameObject wallPrefab;

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
        if (width <= 0)
        {
            width = 1;
        }
        if (height <= 0)
        {
            height = 1;
        }

        CreateFloor();
        GenerateRandomMazeMatrix(width, height);
        CreateMaze(cells);
    }


    private void CreateFloor()
    {
        GameObject floor = Instantiate(floorPrefab, new Vector3(width / 2f, 0, height / 2f), Quaternion.identity);
        floor.transform.localScale = new Vector3(width, floor.transform.localScale.y, height);
        floor.name = "Floor";
    }

    private void GenerateRandomMazeMatrix(int width, int height)
    {
        int ILocation = 0;
        int JLocation = 0;

        //Initialize the array
        cells = new Cell[width, height];
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
    }

    public void VisitNewCell(Cell[,] cells, int i, int j)
    {
        //Debug.Log((i + 1) + " " + (j + 1));
        cells[i, j].isVisited = true;
        if (!HaveAdjacentAvailableCell(cells, i, j))
        {
            return;
        }
        while (HaveAdjacentAvailableCell(cells, i, j))
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
                        cells[i, j].hasWalls[0] = false;
                        cells[i - 1, j].hasWalls[2] = false;
                        VisitNewCell(cells, i - 1, j);
                    }
                    else if (selectedWall == 1 && j > 0 && !cells[i, j - 1].isVisited)
                    {
                        wallFound = true;
                        cells[i, j].hasWalls[1] = false;
                        cells[i, j - 1].hasWalls[3] = false;
                        VisitNewCell(cells, i, j - 1);
                    }
                    else if (selectedWall == 2 && i + 1 < width && !cells[i + 1, j].isVisited)
                    {
                        wallFound = true;
                        cells[i, j].hasWalls[2] = false;
                        cells[i + 1, j].hasWalls[0] = false;
                        VisitNewCell(cells, i + 1, j);
                    }
                    else if (selectedWall == 3 && j + 1 < height && !cells[i, j + 1].isVisited)
                    {
                        wallFound = true;
                        cells[i, j].hasWalls[3] = false;
                        cells[i, j + 1].hasWalls[1] = false;
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

    private void CreateMaze(Cell[,] cells)
    {
        GameObject walls = new GameObject();
        walls.name = "Walls";
        bool[] currentWalls;
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
            {
                currentWalls = cells[i, j].hasWalls;
                for (int k = 0; k < 4; k++)
                {
                    if (currentWalls[k])
                    {
                        if (k == 1 || k == 3)
                        {
                            GameObject wall = Instantiate(wallPrefab, walls.transform);
                            wall.transform.position = new Vector3(0.5f + i, wall.transform.position.y, j);
                        }
                        else
                        {
                            GameObject wall = Instantiate(wallPrefab, walls.transform);
                            wall.transform.position = new Vector3(i, wall.transform.position.y, j + 0.5f);
                            wall.transform.rotation = Quaternion.Euler(wall.transform.rotation.x, 90, wall.transform.rotation.z);
                        }
                    }
                }
            }
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
