using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEvent : MonoBehaviour
{

    public ChaseEnemy enemy;
    public PlayerControl player;
    public GameObject scObj;
    public GameObject hpObj;
    public GameObject coObj;
    public GameObject spObj;
    FollowCamera camera;
    public float waitTime;
    public float moveTime;
    bool isEvent;
    bool isMove;
    Transform previousPos;
    public GameObject startImage;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(player.transform.position.x,
           transform.position.y,
           transform.position.z);
        previousPos = transform;
        isEvent = false;
        isMove = false;
        camera = GetComponent<FollowCamera>();
        StartCoroutine(CameraMove());
    }

    // Update is called once per frame
    void Update()
    {
        if (isEvent)
        {
            Vector3 pos = enemy.transform.position;
            pos.x += enemy.speed * Time.deltaTime;
            enemy.transform.position = pos;
        }

        if (isMove)
        {
            CameraMove(enemy.transform);
        }
        else
        {
            CameraMove(player.transform);
        }
    }

    //カメラの回転
    void CameraMove(Transform target)
    {
        // 補完スピードを決める
        float speed = 0.02f;
        // ターゲット方向のベクトルを取得
        Vector3 relativePos = target.position - transform.position;
        // 方向を、回転情報に変換
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        //x軸とz軸は回転させないため初期化
        rotation.x = 0;
        rotation.z = 0;
        // 現在の回転情報と、ターゲット方向の回転情報を補完する
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed);
    }

    IEnumerator CameraMove()
    {

        camera.enabled = false;
        enemy.enabled = false;
        player.enabled = false;
        hpObj.SetActive(false);
        scObj.SetActive(false);
        coObj.SetActive(false);
        spObj.SetActive(false);
        isEvent = true;
        yield return new WaitForSeconds(moveTime);
        isMove = true;
        yield return new WaitForSeconds(waitTime);
        isMove = false;
       
        yield return new WaitForSeconds(waitTime);
        startImage.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        isEvent = false;
        transform.rotation = Quaternion.identity;
        startImage.SetActive(false);
        camera.enabled = true;
        player.enabled = true;
        hpObj.SetActive(true);
        scObj.SetActive(true);
        coObj.SetActive(true);
        spObj.SetActive(true);
        enemy.enabled = true;
    }
}
