using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField]
    protected const float maxHealth = 100f;
    protected float health = 100f;

    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        health = maxHealth;
    }

    protected virtual void Update()
    {
        
    }
}
