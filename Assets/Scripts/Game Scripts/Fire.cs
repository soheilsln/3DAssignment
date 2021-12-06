using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public float fireDuration = 5f;
    private void Start()
    {
        StartCoroutine(DestroyFire(fireDuration));
        Physics.IgnoreCollision(GameManager.instance.AI.GetComponent<SphereCollider>(), GetComponent<BoxCollider>());
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
            if (other.GetComponent<PlayerController>() != null)
            {
                other.GetComponent<PlayerController>().SetIsWalking();
            }
            else if (other.GetComponent<AIController>() != null)
            {
                other.GetComponent<AIController>().SetIsWalking();
            }
        }
    }

}
