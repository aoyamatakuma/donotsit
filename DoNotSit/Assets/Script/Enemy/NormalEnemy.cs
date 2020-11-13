using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : NormalEnemyAI
{
    public float speed;
    public GameObject rayPos;
    bool isDead;
    public int damage;
    public GameObject effect;
    void Start()
    {
        isDead = false;
        enemyState = EnemyState.Move;
        isReturn = false;
        isTouch = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            base.Move(speed);
            base.TouchGround(rayPos.transform);
            base.Return_Ground();
        }

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
                player.Damage(damage); 
            }

        }

        if( col.gameObject.tag=="BlowAway")
        { 
            gameObject.tag = "BlowAway";
            Hit();
            isDead = true;
        }

        if(col.gameObject.tag == "Bomb")
        {
            Effect effect = col.GetComponent<Effect>();
            if (!effect.isChaseHit)
            {
                gameObject.tag = "BlowAway";
                Hit();
                isDead = true;
            }
        }
    }

    //void OnTriggerStay(Collision col)
    //{
    //    if(col.gameObject.tag == "Wall")
    //    {
    //        isTouch = true;
    //    }
    //}

    //void OnTriggerExit(Collision col)
    //{
    //    if (col.gameObject.tag == "Wall")
    //    {
    //        isTouch = false;
    //    }
    //}


}
