using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 壁を歩く敵
/// </summary>
public class WallWalkEnemy : MonoBehaviour
{
    public EnemyState currentState;
    //速度
    public float speed;
    //rayの長さ
    public float rayLength;
    //左回転ならfalse右回転ならtrue
    private bool isReturn;
    //回転中か
    private bool isRotate;
    //地面に触れているか
    private bool isTouch;
    //transform.upが正か負か
    private bool isUp;
    Vector3 hitPoint;
    [SerializeField]
    Transform CenterOfBalance;  // 重心
    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.Left;
        if (transform.up.y < 0)
        {
            isReturn = true;
        }
        else
        {
            isReturn = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        TouchGround();
        YMoveDirection();
        XMove();
        YMove();
        Rotate();
    }

    //地面に触れてるかrayを飛ばして判定
    void TouchGround()
    {
        if (isRotate)
        {
            return;
        }
        RaycastHit hit;
        if (Physics.Raycast(
                    CenterOfBalance.position,
                    -transform.up,
                    out hit,
                    rayLength))
        {
            isTouch = true;
            hitPoint = hit.point;
        }
        else
        {
            isTouch = false;

        }

        Debug.DrawRay(CenterOfBalance.position, -transform.up, new Color(255, 0, 0), rayLength);
    }
    //地面がない場所で回転
    void Rotate()
    {
        //左回転か右回転か
        if (!isReturn)
        {
            if (!isTouch && currentState == EnemyState.UP && !isRotate)
            {
                StartCoroutine(RotateCoroutine(1.0f));
                transform.RotateAround(hitPoint, Vector3.forward, 90);
                currentState = EnemyState.Left;
            }

            if (!isTouch && currentState == EnemyState.Left && !isRotate)
            {
                StartCoroutine(RotateCoroutine(1.0f));
                transform.RotateAround(hitPoint, Vector3.forward, 90);
                currentState = EnemyState.DOWN;
            }

            if (!isTouch && currentState == EnemyState.DOWN && !isRotate)
            {
                StartCoroutine(RotateCoroutine(1.0f));
                transform.RotateAround(hitPoint, Vector3.forward, 90);
                currentState = EnemyState.Right;
            }

            if (!isTouch && currentState == EnemyState.Right && !isRotate)
            {
                StartCoroutine(RotateCoroutine(1.0f));
                transform.RotateAround(hitPoint, Vector3.forward, 90);
                currentState = EnemyState.UP;
            }
        }
        else
        {
            if (!isTouch && currentState == EnemyState.UP && !isRotate)
            {
                StartCoroutine(RotateCoroutine(1.0f));
                transform.RotateAround(hitPoint, Vector3.forward, -90);
                currentState = EnemyState.Right;
            }

            if (!isTouch && currentState == EnemyState.Left && !isRotate)
            {
                StartCoroutine(RotateCoroutine(1.0f));
                transform.RotateAround(hitPoint, Vector3.forward, -90);
                currentState = EnemyState.UP;
            }

            if (!isTouch && currentState == EnemyState.DOWN && !isRotate)
            {
                StartCoroutine(RotateCoroutine(1.0f));
                transform.RotateAround(hitPoint, Vector3.forward, -90);
                currentState = EnemyState.Left;
            }

            if (!isTouch && currentState == EnemyState.Right && !isRotate)
            {
                StartCoroutine(RotateCoroutine(1.0f));
                transform.RotateAround(hitPoint, Vector3.forward, -90);
                currentState = EnemyState.DOWN;
            }
        }

    }

    //上向きか下向きか
    void YMoveDirection()
    {
        if (transform.up.y < 0)
        {
            isUp = false;
        }
        else
        {
            isUp = true;
        }
    }

    //横移動
    void XMove()
    {
        if (currentState != EnemyState.Right && currentState != EnemyState.Left)
        {
            return;
        }

        Vector3 pos = transform.position;
        if (currentState == EnemyState.Right)
        {
            pos.x += speed * Time.deltaTime;
        }
        else
        {
            pos.x -= speed * Time.deltaTime;
        }
        pos.z = 0;
        transform.position = pos;
    }
    //回転コルーチン
    IEnumerator RotateCoroutine(float waitSecond)
    {
        isRotate = true;
        yield return new WaitForSeconds(waitSecond);
        isRotate = false;
    }


    //縦移動
    void YMove()
    {
        if (currentState != EnemyState.UP && currentState != EnemyState.DOWN)
        {
            return;
        }

        Vector3 pos = transform.position;
        if (currentState == EnemyState.UP)
        {
            pos.y += speed * Time.deltaTime;
        }
        else
        {
            pos.y -= speed * Time.deltaTime;
        }
        pos.z = 0;
        transform.position = pos;
    }




    void OnCollisionEnter(Collision col)
    {
        //特定のオブジェクトに当たる時反転
        if (col.gameObject.CompareTag("Enemy"))
        {
            if (currentState == EnemyState.Right)
            {
                Vector3 angle = transform.eulerAngles;
                angle.y = 0f;
                transform.eulerAngles = angle;
                currentState = EnemyState.Left;
                isReturn = false;
            }
            else if (currentState == EnemyState.Left)
            {
                Vector3 angle = transform.eulerAngles;
                angle.y = 180f;
                transform.eulerAngles = angle;
                currentState = EnemyState.Right;
                isReturn = true;
            }
            if (!isReturn)
            {
                if (currentState == EnemyState.UP)
                {
                    Vector3 angle = transform.eulerAngles;
                    angle.x = 180f;
                    transform.eulerAngles = angle;
                    currentState = EnemyState.DOWN;
                    isReturn = true;
                }
                else if (currentState == EnemyState.DOWN)
                {
                    Vector3 angle = transform.eulerAngles;
                    angle.x = 0f;
                    transform.eulerAngles = angle;
                    currentState = EnemyState.UP;
                    isReturn = true;
                }
            }
            else
            {
                if (currentState == EnemyState.UP)
                {
                    Vector3 angle = transform.eulerAngles;
                    angle.x = 0f;
                    transform.eulerAngles = angle;
                    currentState = EnemyState.DOWN;
                    isReturn = false;
                }
                else if (currentState == EnemyState.DOWN)
                {
                    Vector3 angle = transform.eulerAngles;
                    angle.x = 180f;
                    transform.eulerAngles = angle;
                    currentState = EnemyState.UP;
                    isReturn = false;
                }
            }

        }
    }

    void OnTriggerEnter(Collider col)
    {
        //ステージに当たる時回転
        if (col.gameObject.CompareTag("Wall"))
        {
            if (currentState == EnemyState.Left || currentState == EnemyState.Right)
            {
                if (isUp)
                {
                    StartCoroutine(RotateCoroutine(1.0f));
                    Vector3 angle = transform.eulerAngles;
                    angle.z = -90.0f;
                    transform.eulerAngles = angle;
                    currentState = EnemyState.UP;
                }
                else
                {
                    StartCoroutine(RotateCoroutine(1.0f));
                    Vector3 angle = transform.eulerAngles;
                    angle.z = 90.0f;
                    transform.eulerAngles = angle;
                    currentState = EnemyState.DOWN;
                }
            }
            else
            {

                if (currentState == EnemyState.DOWN)
                {
                    StartCoroutine(RotateCoroutine(1.0f));
                    Vector3 angle = transform.eulerAngles;
                    angle.z = 0f;
                    transform.eulerAngles = angle;
                    if (!isReturn)
                    {
                        currentState = EnemyState.Left;
                    }
                    else
                    {
                        currentState = EnemyState.Right;
                    }

                }
                else
                {
                    StartCoroutine(RotateCoroutine(1.0f));
                    Vector3 angle = transform.eulerAngles;
                    angle.z = 180f;
                    transform.eulerAngles = angle;
                    if (!isReturn)
                    {
                        currentState = EnemyState.Right;
                    }
                    else
                    {
                        currentState = EnemyState.Left;
                    }
                   
                }
            }

        }

    }
}



