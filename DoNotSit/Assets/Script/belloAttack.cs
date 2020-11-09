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
    public int damage;
    //予測
    public float rayline;//レイ長さ
    GameObject hitObject;
    public GameObject carsol;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        carsol = GameObject.Find("BelloP");
        playerPos = new Vector3(target.position.x, target.position.y, target.position.z);
        gameObject.transform.LookAt(playerPos);
    }

    void Update()
    {
        Death();
        RayObject();
        transform.position = Vector3.MoveTowards(transform.position, playerPos, speed);
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerControl player = col.gameObject.GetComponent<PlayerControl>();
            player.Damage(damage);
            Destroy(this.gameObject);
            carsol.SetActive(false);
        }
    }
    void RayObject()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            carsol.transform.position = hit.point;
        }
    }
    void Death()
    {
        cnt += Time.deltaTime;
        if (cnt > lifeCnt)
        {
            Destroy(gameObject);
            carsol.SetActive(false);
        }
    }
}
