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

        if(other.GetComponent<Enemy>() != null)
        {
            other.GetComponent<Enemy>().Die();
        }
    }
}
