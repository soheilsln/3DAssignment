using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public Text percentage;

    private void Start()
    {
        Cursor.visible = false;
        StartCoroutine(LoadAsync());
    }

    private IEnumerator LoadAsync()
    {
        AsyncOperation asyncOperation =  SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        while(asyncOperation.progress < 1)
        {
            percentage.text = (asyncOperation.progress * 100) + "%";
            yield return new WaitForEndOfFrame();
        }
    }
}
