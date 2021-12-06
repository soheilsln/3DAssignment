using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<PlayerController>() && 
            !GameManager.instance.AI.GetComponent<AIController>().isGameFinished)
        {
            if (other.transform.GetComponent<PlayerController>().GetKeyCollected())
            {
                other.transform.GetComponent<PlayerController>().WonGame();
            }
            else
            {
                other.transform.GetComponent<PlayerController>().KeyRequired();
            }
        }
    }
}
