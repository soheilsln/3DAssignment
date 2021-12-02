using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : Enemy
{
    public GameObject bullet;

    protected override void Start()
    {
        base.Start();
        Shoot(Vector3.zero);
    }

    private void Move()
    {
        
    }

    private void Shoot(Vector3 target)
    {
        Instantiate(bullet, transform.position, Quaternion.identity);
    }
}
