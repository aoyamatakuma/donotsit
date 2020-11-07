using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class belloAttack : MonoBehaviour
{
    //Rigidbodyを入れる変数
    public Transform target;//追いかける対象-オブジェクトをインスペクタから登録できるように
    public float speed = 0.1f;//移動スピード
    private Vector3 vec;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, new Vector3(vec.x, transform.position.y), speed * Time.deltaTime);
       // transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed);
      //  this.gameObject.transform.LookAt(target.transform.position);
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }
}
