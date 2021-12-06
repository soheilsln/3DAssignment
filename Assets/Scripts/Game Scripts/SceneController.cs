using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    GameManager gameManager;
    RandomMazeGenerator randomMazeGenerator;

    private void Start()
    {
        gameManager = GameManager.instance;
        randomMazeGenerator = RandomMazeGenerator.instance;
    }

    private void Update()
    {
        if (gameManager.GetWinner() == 1)
        {
            randomMazeGenerator.StartNextLevel();
            SceneManager.LoadScene(2, LoadSceneMode.Single);
        }
        else if (gameManager.GetWinner() == 2)
        {
            SceneManager.LoadScene(2, LoadSceneMode.Single);
        }
    }
}
