using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rgbody;
    public float speed = 10f;

    private void Awake()
    {
        rgbody = GetComponent<Rigidbody>();
        Physics.IgnoreCollision(GameManager.instance.AI.GetComponent<SphereCollider>(),GetComponent<Collider>());
    }

    private void Start()
    {
        rgbody.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);

        if(other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().Die();
        }
        if(other.gameObject.CompareTag("Player"))
        {
            if(other.GetComponent<PlayerController>() != null)
            {
                other.GetComponent<PlayerController>().SetIsWalking();
            }

            if(other.GetComponent<AIController>() != null)
            {
                other.GetComponent<AIController>().SetIsWalking();
            }
        }
    }
}
