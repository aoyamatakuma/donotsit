using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingEnemy : MonoBehaviour
{
    // 移動速度
    public float moveSpeed;
    //移動域
    private float up_limitPos;
    public float limitPos;
    public float delay;
    private float down_limitPos;
    //現在のステート
    public EnemyState currentState;
    //以前のステート保存
    private EnemyState previousState;
    private float waitTime;
    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.UP;
        up_limitPos = transform.position.y + limitPos;
        down_limitPos = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        StateChange();
        Move();
        if (currentState == EnemyState.Idle)
        {
            WaitTrun();
        }
    }



    void Move()
    {
        if (currentState != EnemyState.UP && currentState != EnemyState.DOWN)
        {
            return;
        }
        Vector3 pos = transform.position;
        if (currentState == EnemyState.UP)
        {
            pos.y += moveSpeed * Time.deltaTime;
        }
        else
        {
            pos.y -= moveSpeed * Time.deltaTime;
        }
        pos.z = 0;
        transform.position = pos;

    }
    void StateChange()
    {

        float pos = transform.position.y;

        if (pos >= up_limitPos && currentState == EnemyState.UP)
        {
            //ステートセーブ
            previousState = currentState;
            currentState = EnemyState.Idle;
        }

        if (pos <= down_limitPos && currentState == EnemyState.DOWN)
        {
            //ステートセーブ
            previousState = currentState;
            currentState = EnemyState.Idle;
        }


    }


    void OnCollisionEnter(Collision col)
    {
        
    }

    void WaitTrun()
    {
        waitTime += Time.deltaTime;
        if (waitTime > delay)
        {
            if (previousState == EnemyState.UP)
            {
                currentState = EnemyState.DOWN;
            }
            else
            {
                currentState = EnemyState.UP;
            }
            waitTime = 0;
        }
    }
}
