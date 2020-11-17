using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// トゲの敵
/// </summary>
public class SpineEnemy : NormalEnemyAI
{
    public float speed;

    private PlayerControl player;

    void Start()
    {
        enemyState = EnemyState.Move;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        base.Move(speed);
    }

    void OnCollisionEnter(Collision col)
    {
        base.ReturnBoolCollision(col);

        if(col.gameObject.CompareTag("Player") && player.currentPlayerState == PlayerState.Attack)
        {
            Destroy(gameObject);
        }
    }

   
}
