using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallWalkEnemy : MonoBehaviour
{
    public EnemyState currentState;
    public float speed;
    public float rayLength;
    private bool isClimb;
    private bool isRotate;
    private bool isTouch;
    private bool isUp;
    Vector3 hitPoint;
    [SerializeField]
    Transform CenterOfBalance;  // 重心
    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.Left;
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
            Debug.Log("浮いてる");
            isTouch = false;
           
        }

        Debug.DrawRay(CenterOfBalance.position, -transform.up, new Color(255, 0, 0), rayLength);
    }

    void Rotate()
    {

        if (!isTouch && currentState == EnemyState.UP && !isRotate)
        {
            StartCoroutine(RotateCoroutine(1.0f));
            transform.RotateAround(hitPoint,Vector3.forward, 90);
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

        if (!isTouch && currentState == EnemyState.Right&& !isRotate)
        {
            StartCoroutine(RotateCoroutine(1.0f));
            transform.RotateAround(hitPoint, Vector3.forward, 90);
            currentState = EnemyState.UP;
        }
    }

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

    IEnumerator RotateCoroutine(float waitSecond)
    {
        isRotate = true;
        yield return new WaitForSeconds(waitSecond);
        isRotate = false;
    }



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
        if (col.gameObject.CompareTag("Enemy"))
        {
            if (currentState == EnemyState.Right)
            {
                Vector3 angle = transform.eulerAngles;
                angle.y = 0f;
                transform.eulerAngles = angle;
                currentState = EnemyState.Left;
            }
            else if (currentState == EnemyState.Left)
            {
                Vector3 angle = transform.eulerAngles;
                angle.y = 180f;
                transform.eulerAngles = angle;
                currentState = EnemyState.Right;
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
            if(currentState == EnemyState.Left || currentState == EnemyState.Right)
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
                if(currentState == EnemyState.DOWN)
                {
                    StartCoroutine(RotateCoroutine(1.0f));
                    Vector3 angle = transform.eulerAngles;
                    angle.z = 0f;
                    transform.eulerAngles = angle;
                    currentState = EnemyState.Left;
                }
                else
                {
                    StartCoroutine(RotateCoroutine(1.0f));
                    Vector3 angle = transform.eulerAngles;
                    angle.z = 180f;
                    transform.eulerAngles = angle;
                    currentState = EnemyState.Right;
                }
            }
           
        }
    }

    void OnTriggerExit(Collider col)
    {

    }
}
