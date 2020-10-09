using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineEnemy : NormalEnemyAI
{
    public float speed;

    private PlayerControl player;

    void Start()
    {
        enemyState = EnemyState.Move;
        isReturn = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        base.Move(speed);
    }

    void OnCollisionEnter(Collision col)
    {
        base.ReturnBool(col);

        if(col.gameObject.CompareTag("Player") && player.currentPlayerState == PlayerState.Attack)
        {
            Destroy(gameObject);
        }
    }

   
}
