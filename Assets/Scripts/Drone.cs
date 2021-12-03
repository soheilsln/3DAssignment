using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : Enemy
{
    

    public GameObject bullet;
    public Transform player;
    public Transform AI;
    public Transform spawnBulletPosition;
    [SerializeField]
    private float attackRange = 10f;
    [SerializeField]
    private LayerMask shootLayerMask = new LayerMask();
    [SerializeField]
    private float shootCooldownTime = 10f;
    private float shootTimeStamp = 0f;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (shootTimeStamp <= Time.time)
        {
            Attack();
            shootTimeStamp = Time.time + shootCooldownTime;
        }
    }

    private void Shoot(Vector3 target)
    {
        target = target + Vector3.up;
        if (Physics.Raycast(spawnBulletPosition.position, target - spawnBulletPosition.position, 
            out RaycastHit hit, 999f, shootLayerMask))
        {
            if (hit.transform.CompareTag("Player"))
            {
                Vector3 aimDir = (target - spawnBulletPosition.position).normalized;
                Instantiate(bullet, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
            }
        }
    }

    private void Attack()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        float distanceToAI = Vector3.Distance(AI.position, transform.position);

        //if player or AI is in Range
        if (distanceToPlayer <= attackRange || distanceToAI <= attackRange)
        {
            if (distanceToPlayer <= attackRange && distanceToAI <= attackRange)
            {
                int rnd = Random.Range(0, 2);
                if (rnd == 0)
                {
                    Shoot(player.position);
                }
                else
                {
                    Shoot(AI.position);
                }
            }
            else if (distanceToPlayer <= attackRange)
            {
                Shoot(player.position);
            }
            else
            {
                Shoot(AI.position);
            }
        }
    }

    public override void Die()
    {
        base.Die();
    }
}
