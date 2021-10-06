using System.Collections;
using System.Collections.Generic;
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

        //
    }

    public void VisitNewCell(Cell[,] cells, int i, int j)
    {
        cells[i, j].isVisited = true;
        if(cells[i,j].hasLeftWall == false && cells[i, j].hasLeftWall == false && cells[i, j].hasLeftWall == false && cells[i, j].hasLeftWall == false)
        {
            return;
        }
        //To Do
    }

    public class Cell
    {
        public bool hasLeftWall = true;
        public bool hasDownWall = true;
        public bool hasRightWall = true;
        public bool hasUpWall = true;
        public bool isVisited = false;
    }

}
