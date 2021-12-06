using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : Enemy
{
    private Animator animator;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        animator.SetFloat("MotionSpeed", 1f);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void Die()
    {
        base.Die();
    }

    protected override IEnumerator MoveToLocation(Vector3 location, float duration)
    {
        float time = 0f;
        Vector3 startPosition = transform.position;

        transform.LookAt(location);
        animator.SetFloat("Speed", 5.335f);

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, location, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = location;
        reachedDestination = true;
        animator.SetFloat("Speed", 0f);
    }
}
