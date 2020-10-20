using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalFlyEnemy : MonoBehaviour
{
    // 移動速度
    public float moveSpeed;
    //移動域
    private float right_limitPos;
    private float left_limitPos;
    private float basePos;
    public float x_LimitPos;
    public float delay;
    private bool isRetrun;
    private float moveTime;
    private bool change;

    //現在のステート
    public EnemyState currentState;
    //以前のステート保存
    private EnemyState previousState;
    private float waitTime;
    // Start is called before the first frame update
    void Start()
    {
        isRetrun = false;
        currentState = EnemyState.Move;
        basePos = transform.position.x;
        right_limitPos = transform.position.x + x_LimitPos;
        left_limitPos = transform.position.x - x_LimitPos;
    }

    // Update is called once per frame
    void Update()
    {
       
        XMove();
        if (currentState == EnemyState.Idle)
        {
            WaitTrun();
        }
    }
  
    void XMove()
    {
        if(currentState != EnemyState.Move)
        {
            return;
        }
        moveTime += Time.deltaTime;

        if(moveTime > moveSpeed)
        {
            if (change)
            {
                currentState = EnemyState.Idle;
                isRetrun = !isRetrun;
            }
            change = !change;     
            moveTime = 0;
        }
        if (!isRetrun)
        {
            if (!change)
            {
                var pos = left_limitPos + (basePos - left_limitPos) * ((moveTime + 1) * moveTime / 2.0f) / ((moveSpeed + 1) * moveSpeed / 2.0f);
                transform.position = new Vector3(pos, transform.position.y, 0);
            }
            else
            {
                var pos = basePos + (right_limitPos - basePos) * ((moveSpeed * 2 - moveTime + 1) * moveTime / 2.0f) / ((moveSpeed + 1) * moveSpeed / 2.0f);
                transform.position = new Vector3(pos, transform.position.y, 0);        
            }
        }
        else
        {
            if (!change)
            {
                var pos = right_limitPos + (basePos - right_limitPos) * ((moveTime + 1) * moveTime / 2.0f) / ((moveSpeed + 1) * moveSpeed / 2.0f);
                transform.position = new Vector3(pos, transform.position.y, 0);
            }
            else
            {
                var pos = basePos + (left_limitPos - basePos) * ((moveSpeed * 2 - moveTime + 1) * moveTime / 2.0f) / ((moveSpeed + 1) * moveSpeed / 2.0f);
                transform.position = new Vector3(pos, transform.position.y, 0);
            }

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
            currentState = EnemyState.Move;
            waitTime = 0;
        }
    }
}
