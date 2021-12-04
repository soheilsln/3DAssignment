using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(DestroyFire(GameManager.instance.GetFireDuration()));
    }

    private IEnumerator DestroyFire(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.GetComponent<ThirdPersonShooterController>() != null)
            {
                other.GetComponent<ThirdPersonShooterController>().SetIsWalking();
            }
            else if (other.GetComponent<AIManager>() != null)
            {
                other.GetComponent<AIManager>().SetIsWalking();
            }
        }
    }

}
