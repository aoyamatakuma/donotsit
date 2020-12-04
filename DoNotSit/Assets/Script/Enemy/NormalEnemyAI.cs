using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemyAI : MonoBehaviour
{

    protected EnemyState enemyState;
    protected bool isTouch;
    protected bool isCol;

    public void StartState(float rotateY,float rotateZ)
    {
        if(rotateZ == 0)
        {
            if(rotateY == 0)
            {
                enemyState = EnemyState.Right;
            }
            else
            {
                enemyState = EnemyState.Left;
            }
        }
        else
        {
            if (rotateY == 0)
            {
                enemyState = EnemyState.Left;
            }
            else
            {
                enemyState = EnemyState.Right;
            }
        }
    }
 
    public void Move(float speed)
    {
        if(enemyState != EnemyState.Left && enemyState != EnemyState.Right)
        {
            return;
        }
        Vector3 pos = transform.position;
        if (enemyState == EnemyState.Right)
        {
            pos.x += speed * Time.deltaTime;
        }
        else
        {
            pos.x -= speed * Time.deltaTime;
        }
   
        transform.position = pos;
    }

   public void TouchGround(Transform rayPos)
    {
        RaycastHit hit;
       
        if (Physics.Raycast(
                    rayPos.position,
                    -transform.up,
                    out hit,
                    5f))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                isTouch = true;
            }       
        }
        else
        {
            isTouch = false;
        }

        Debug.DrawRay(rayPos.position, -transform.up * 2f, new Color(255, 0, 0));
    }

    public void Return_Ground()
    {
        if (!isTouch)
        {
            Return();
        }
    }

   public void Return()
    {
        transform.Rotate(0, 180, 0);
        if(enemyState == EnemyState.Right)
        {
            enemyState = EnemyState.Left;
        }
        else
        {
            enemyState = EnemyState.Right;
        }
    }


    public void ReturnBoolTrigger(Collider col)
    {
        if (col.gameObject.CompareTag("Wall") && !isCol)
        { 
            Return();
            isCol = true;
        }

        if (col.gameObject.CompareTag("Enemy"))
        {
            Return();
        }
    }

}
