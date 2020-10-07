using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : MonoBehaviour
{
    public float speed;
    public EnemyState enemyState;
    public bool isReturn;
    // Start is called before the first frame update
    void Start()
    {
        enemyState = EnemyState.Move;
        isReturn = false;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
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

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Wall"))
        {
            isReturn = !isReturn;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
