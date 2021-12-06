using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private void Start()
    {
        Physics.IgnoreCollision(GameManager.instance.AI.GetComponent<SphereCollider>(), GetComponent<Collider>());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerController>() && 
                !other.GetComponent<PlayerController>().GetKeyCollected())
            {
                other.GetComponent<PlayerController>().CollectKey();
                Destroy(gameObject);
            }
            else if (other.GetComponent<AIController>() &&
                !other.GetComponent<AIController>().GetKeyCollected())
            {
                other.GetComponent<AIController>().CollectKey();
                Destroy(gameObject);
            }
        }
    }
}
