using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : Enemy
{
    private Transform player;
    private Transform AI;
    public GameObject bombPrefab;
    [SerializeField]
    private Transform bombSpawnPoint;
    [SerializeField]
    private float bombCooldownTime = 10f;
    private float bombTimeStamp = 0f;
    [SerializeField]
    private float attackRange = 10f;

    protected override void Awake()
    {
        base.Awake();
        player = GameManager.instance.player.transform;
        AI = GameManager.instance.AI.transform;
    }

    protected override void Start()
    {
        base.Start();
        bombTimeStamp = Time.time + bombCooldownTime;
    }

    protected override void Update()
    {
        base.Update();
        if (bombTimeStamp <= Time.time)
        {
            DropBomb();
        }
    }

    public override void Die()
    {
        base.Die();
    }

    private void DropBomb()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        float distanceToAI = Vector3.Distance(AI.position, transform.position);

        //if player or AI is in Range
        if (distanceToPlayer <= attackRange || distanceToAI <= attackRange)
        {
            Instantiate(bombPrefab, bombSpawnPoint.position, Quaternion.identity);
            bombTimeStamp = Time.time + bombCooldownTime;
        }
    }

    
}
