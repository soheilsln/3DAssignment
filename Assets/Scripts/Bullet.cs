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
            if(other.GetComponent<ThirdPersonShooterController>() != null)
            {
                other.GetComponent<ThirdPersonShooterController>().SetIsWalking();
            }

            if(other.GetComponent<AIManager>() != null)
            {
                other.GetComponent<AIManager>().SetIsWalking();
            }
        }
    }
}
