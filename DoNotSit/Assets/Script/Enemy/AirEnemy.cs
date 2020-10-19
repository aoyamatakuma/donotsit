using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirEnemy : MonoBehaviour
{
    Rigidbody _rb;
    // 移動速度
    public float moveSpeed;
    //移動域
    private float up_limitPos;
    private float down_limitPos;
    public float limitPos;
    public float delay;

    //現在のステート
    public EnemyState currentState;
    //以前のステート保存
    private EnemyState previousState;
    private float waitTime;
    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.UP;
        _rb = GetComponent<Rigidbody>();
        up_limitPos = transform.position.y + limitPos;
        down_limitPos = transform.position.y - limitPos;
    }

    // Update is called once per frame
    void Update()
    {
        StateChange();
        FlyMove();
        if (currentState == EnemyState.Idle)
        {
            WaitTrun();
        }
    }



    void FlyMove()
    {
        if (currentState != EnemyState.UP && currentState != EnemyState.DOWN)
        {
            return;
        }
        Vector3 moveVector = Vector3.zero;
        if (currentState == EnemyState.UP)
        {
            moveVector.y = moveSpeed;
        }
        else
        {
            moveVector.y = -moveSpeed;
        }
        _rb.AddForce(moveVector);

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
        previousState = currentState;
        currentState = EnemyState.Idle;
    }

    void WaitTrun()
    {
        _rb.velocity = Vector3.zero;
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
