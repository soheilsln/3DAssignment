using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }
}
