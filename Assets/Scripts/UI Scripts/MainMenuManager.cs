using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    public Button newGameButton;
    public Button continueButton;

    private void Start()
    {
        Cursor.visible = false;
        if(!PlayerPrefs.HasKey("width") || !PlayerPrefs.HasKey("lenght"))
        {
            continueButton.interactable = false;
        }
        else
        {
            continueButton.interactable = true;
        }
    }

    public void OnNewGameClicked()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

    public void OnContinueClicked()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }
}
