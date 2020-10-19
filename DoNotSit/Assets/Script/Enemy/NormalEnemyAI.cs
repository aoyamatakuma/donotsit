using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemyAI : MonoBehaviour
{

    protected EnemyState enemyState;
    protected bool isReturn;

 
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
        pos.z = 0;
        transform.position = pos;
    }


    public void ReturnBool(Collision col)
    {
        if (col.gameObject.CompareTag("Wall") || col.gameObject.CompareTag("Enemy"))
        {
            isReturn = !isReturn;
        }
    }

}
