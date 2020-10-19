using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemy : MonoBehaviour
{
    Rigidbody _rb;
    // 移動速度
    public float moveSpeed;
    //移動域
    private float up_limitPos;
    private float down_limitPos;
    private float right_limitPos;
    private float left_limitPos;
    public float y_LimitPos;
    public float x_LimitPos;
    public float delay;
    private bool isRetrun;

    //現在のステート
    public EnemyState currentState;
    //以前のステート保存
    private EnemyState previousState;
    private float waitTime;
    // Start is called before the first frame update
    void Start()
    {
        isRetrun = false;
        currentState = EnemyState.UP;
        _rb = GetComponent<Rigidbody>();
        up_limitPos = transform.position.y + y_LimitPos;
        down_limitPos = transform.position.y - y_LimitPos;
        right_limitPos = transform.position.x + x_LimitPos;
        left_limitPos = transform.position.x - x_LimitPos;
    }

    // Update is called once per frame
    void Update()
    {
        StateChange();
        YMove();
        XMove();
        if(currentState == EnemyState.Idle)
        {
            WaitTrun();
        }
    }
   


    void YMove()
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
        
        float y_Pos = transform.position.y;
        float x_Pos = transform.position.x;

        if (y_Pos >= up_limitPos && currentState == EnemyState.UP)
        {
            //ステートセーブ
            previousState = currentState;
            currentState = EnemyState.Idle;
        }

        if (y_Pos <= down_limitPos && currentState == EnemyState.DOWN) 
        {
            //ステートセーブ
            previousState = currentState;
            currentState = EnemyState.Idle;
        }

        if(x_Pos >= right_limitPos || x_Pos <= left_limitPos)
        {
            isRetrun = !isRetrun;
        }
        
    }
    void XMove()
    {
      
        Vector3 pos = transform.position;
        if (!isRetrun)
        {
            pos.x += moveSpeed * Time.deltaTime;
        }
        else
        {
            pos.x -= moveSpeed * Time.deltaTime;
        }
        pos.z = 0;
        transform.position = pos;
    }


    void OnCollisionEnter(Collision col)
    {
        previousState = currentState;
        currentState = EnemyState.Idle;
        isRetrun = !isRetrun;
    }

    void WaitTrun()
    {
        _rb.velocity = Vector3.zero;
        waitTime += Time.deltaTime;
        if(waitTime > delay)
        {
            if(previousState == EnemyState.UP)
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
