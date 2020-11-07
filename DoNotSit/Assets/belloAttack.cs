using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class belloAttack : MonoBehaviour
{
    //Rigidbodyを入れる変数
    public Transform target;//追いかける対象-オブジェクトをインスペクタから登録できるように
    public float speed = 0.1f;//移動スピード
    Vector3 playerPos;
    public float lifeCnt;
    float cnt;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        playerPos = new Vector3(target.position.x, target.position.y, target.position.z);
        gameObject.transform.LookAt(playerPos);
    }

    void Update()
    {
        Death();
       
        transform.position = Vector3.MoveTowards(transform.position, playerPos, speed);
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }

    void Death()
    {
        cnt += Time.deltaTime;
        if(cnt > lifeCnt)
        {
            Destroy(gameObject);
        }
    }
}
