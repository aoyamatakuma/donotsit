using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : NormalEnemyAI
{
    public float speed;
    bool isDead;
    public int damage;
    public GameObject effect;
    void Start()
    {
        isDead = false;
        enemyState = EnemyState.Move;
        isReturn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            base.Move(speed);
        }

    }


    void OnCollisionEnter(Collision col)
    {
        // base.ReturnBoolCollision(col);

      
    }

    void Hit()
    {
      
        Instantiate(effect, transform.position, transform.rotation);
    }

    void OnTriggerEnter(Collider col)
    {
        base.ReturnBoolTrigger(col);
        if (col.gameObject.tag == "Player" && !isDead)
        {
            PlayerControl player = col.gameObject.GetComponent<PlayerControl>();
            if (player.currentPlayerState == PlayerState.Attack)
            {
                gameObject.tag = "BlowAway";
                Hit();
                isDead = true;
            }
            else
            {
                player.Damage(damage); ;
            }

        }

        if(col.gameObject.tag == "Bomb")
        {
            gameObject.tag = "BlowAway";
            Hit();
            isDead = true;
        }
    }


}
