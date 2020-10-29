using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemyAI : MonoBehaviour
{

    protected EnemyState enemyState;
    protected bool isReturn;
    protected bool isTouch;
 
    public void Move(float speed)
    {
        if(enemyState != EnemyState.Move)
        {
            return;
        }
        Vector3 pos = transform.position;
        if (!isReturn)
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
                    2f))
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

    void Return()
    {
        transform.Rotate(0, 180, 0);
        isReturn = !isReturn;
    }


    public void ReturnBoolCollision(Collision col)
    {
        if (col.gameObject.CompareTag("Wall") || col.gameObject.CompareTag("Enemy"))
        {
            Return();
        }
    }

    public void ReturnBoolTrigger(Collider col)
    {
        if (col.gameObject.CompareTag("Wall") || col.gameObject.CompareTag("Enemy") )
        {
            Return();
        }
    }

}
