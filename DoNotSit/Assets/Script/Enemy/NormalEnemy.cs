﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : NormalEnemyAI
{
    public float speed;

    void Start()
    {
        enemyState = EnemyState.Move;
        isReturn = false;
    }

    // Update is called once per frame
    void Update()
    {
        base.Move(speed);
    }


    void OnCollisionEnter(Collision col)
    {
        base.ReturnBool(col);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
