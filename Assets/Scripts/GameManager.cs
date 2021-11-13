using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public RandomMazeGenerator.Cell[,] cells;

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

    public int[] SetInitialLocation()
    {
        int width = cells.GetLength(0);
        int lenght = cells.GetLength(1);

        int locationIndex = UnityEngine.Random.Range(0, 4);

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
    }

    void Start()
    {
        cells = RandomMazeGenerator.instance.cells;
    }

    void Update()
    {

    }

}
