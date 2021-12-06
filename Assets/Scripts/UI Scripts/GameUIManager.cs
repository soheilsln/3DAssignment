using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameUIManager : MonoBehaviour
{
    public Text shootCoolDown;
    public Text bombCoolDown;
    public Text punchCoolDown;

    public GameObject UIPanel;
    // id : 1
    public GameObject keyRequiredPanel;
    public GameObject keyRequiredPanelFirstSelected;
    // id : 2
    public GameObject continuePanel;
    public GameObject continuePanelFirstSelected;
    // id : 3
    public GameObject failedPanel;
    public GameObject failedPanelFirstSelected;
    // id : 4
    public GameObject pausePanel;
    public GameObject pausePanelFirstSelected;

    private ThirdPersonShooterController player;
    private StarterAssetsInputs starterAssetsInputs;
    private EventSystem eventSystem;
    private float shootTime;
    private float bombTime;
    private float punchTime;

    private void Start()
    {
        player = GameManager.instance.player.GetComponent<ThirdPersonShooterController>();
        starterAssetsInputs = player.GetComponent<StarterAssetsInputs>();
        eventSystem = EventSystem.current;
    }

    private void Update()
    {
        if (starterAssetsInputs.pause)
        {
            if (!pausePanel.activeSelf && !UIPanel.activeSelf)
            {
                ActiveUIPanelObjects(4);
            }
            else if (pausePanel.activeSelf)
            {
                pausePanel.SetActive(false);
                UIPanel.SetActive(false);
            }
            starterAssetsInputs.pause = false;
        }

        UpdateCooldowns();

    }

    private void UpdateCooldowns()
    {
        shootTime = (player.shootTimeStamp - Time.time) > 0 ? player.shootTimeStamp - Time.time : 0f;
        bombTime = (player.bombTimeStamp - Time.time) > 0 ? player.bombTimeStamp - Time.time : 0f;
        punchTime = (player.punchTimeStamp - Time.time) > 0 ? player.punchTimeStamp - Time.time : 0f;

        shootCoolDown.text = "Shoot in: " + shootTime.ToString("0.0") + " Seconds";
        bombCoolDown.text = "Drop Bomb in: " + bombTime.ToString("0.0") + " Seconds";
        punchCoolDown.text = "Punch in: " + punchTime.ToString("0.0") + " Seconds";

        if (shootTime == 0)
            shootCoolDown.color = Color.green;
        else
            shootCoolDown.color = Color.red;

        if (bombTime == 0)
            bombCoolDown.color = Color.green;
        else
            bombCoolDown.color = Color.red;

        if (punchTime == 0)
            punchCoolDown.color = Color.green;
        else
            punchCoolDown.color = Color.red;
    }

    private void ActiveUIPanelObjects(int id)
    {
        UIPanel.SetActive(true);
        switch (id)
        {
            case 1:
                keyRequiredPanel.SetActive(true);
                continuePanel.SetActive(false);
                failedPanel.SetActive(false);
                pausePanel.SetActive(false);
                eventSystem.SetSelectedGameObject(keyRequiredPanelFirstSelected, new BaseEventData(eventSystem));
                break;
            case 2:
                keyRequiredPanel.SetActive(false);
                continuePanel.SetActive(true);
                failedPanel.SetActive(false);
                pausePanel.SetActive(false);
                eventSystem.SetSelectedGameObject(continuePanelFirstSelected, new BaseEventData(eventSystem));
                break;
            case 3:
                keyRequiredPanel.SetActive(false);
                continuePanel.SetActive(false);
                failedPanel.SetActive(true);
                pausePanel.SetActive(false);
                eventSystem.SetSelectedGameObject(failedPanelFirstSelected, new BaseEventData(eventSystem));
                break;
            case 4:
                keyRequiredPanel.SetActive(false);
                continuePanel.SetActive(false);
                failedPanel.SetActive(false);
                pausePanel.SetActive(true);
                eventSystem.SetSelectedGameObject(pausePanelFirstSelected, new BaseEventData(eventSystem));
                break;
            default:
                break;
        }
    }
}
