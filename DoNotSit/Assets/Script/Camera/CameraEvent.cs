using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEvent : MonoBehaviour
{

    public ChaseEnemy enemy;
    public PlayerControl player;
    public List<GameObject> uiObjects;
    FollowCamera camera;
    public float offsetY = 0.3f;
    public float waitTime;
    public float moveTime;
    public float rotateSpeed = 0.02f;
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

        if (isEvent)
        {
            if (isMove)
            {
                CameraMove(enemy.transform);
            }
            else
            {
                CameraMove(player.transform);
            }
        }
       
    }

    //カメラの回転
    void CameraMove(Transform target)
    {

        // ターゲット方向のベクトルを取得
        Vector3 relativePos = target.position - transform.position;
        // 方向を、回転情報に変換
        Quaternion rotation = Quaternion.LookRotation(relativePos);
      
        //x軸とz軸は回転させないため初期化
        rotation.x = 0;
        rotation.z = 0;
        if (isMove)
        {
            rotation.y += offsetY;
        }
    
        // 現在の回転情報と、ターゲット方向の回転情報を補完する
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed);
     
    }

    IEnumerator CameraMove()
    {

        camera.enabled = false;
        enemy.enabled = false;
        player.enabled = false;
        for(int i = 0; i < uiObjects.Count; i++)
        {
            uiObjects[i].SetActive(false);
        }
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
        for (int i = 0; i < uiObjects.Count; i++)
        {
            uiObjects[i].SetActive(true);
        }
        enemy.enabled = true;
    }
}
