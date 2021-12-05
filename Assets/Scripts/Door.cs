using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<ThirdPersonShooterController>())
        {
            if (other.transform.GetComponent<ThirdPersonShooterController>().GetKeyCollected())
            {
                Debug.Log("Player Won");
            }
            else
            {
                Debug.Log("First Collect Key");
            }
        }
    }
}
