using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraScript : MonoBehaviour
{
    //メインカメラの格納
    public GameObject mainCamera;
    //カメラの移動先のポジション
    private Vector3 moveCameraPosition;
    //カメラの現在のポジション
    private Vector3 nowCameraPosition;
    //カメラの初めのポジション
    private Vector3 startCameraPosition;
    //マップの1部分の番号取得用
    private int mapNumber;
    //キャラの当たり判定幅
    private float colBox;

    private float cameraPosZ;

    //ステージスクリプトコンポーネント
    [SerializeField, Header("ステージスクリプト")]
    private StageScript stageCS;

    void Start()
    {
        cameraPosZ = mainCamera.transform.position.z;
        stageCS = gameObject.GetComponent<StageScript>();
        colBox = stageCS.colBoxSize;
        //カメラのスタート地点を設定
        startCameraPosition = Vector3.forward * -78;
        nowCameraPosition = new Vector3(stageCS.GetMapSize().x / 2, stageCS.GetMapSize().y / 2, cameraPosZ);
        mainCamera.transform.position = nowCameraPosition;        
    }

    void Update()
    {
       
    }

    //カメラの視点移動処理()あとでまとめる
    #region CameraMove

    //右
    public void RightMoveCamera()
    {
        moveCameraPosition = new Vector3(stageCS.GetMapSize().x - colBox, 0, 0);
        nowCameraPosition += moveCameraPosition;
        StartCoroutine(RightMove());
       // mainCamera.transform.position = nowCameraPosition;
    }
    IEnumerator RightMove()
    {
        Time.timeScale = 0;
        for(float f = 1f; f>=0;f-=0.1f)
        {
            mainCamera.transform.position += Vector3.right*((stageCS.mapSize.x-colBox)/10);
            yield return null;
        }
        Time.timeScale = 1;
    }
    //左
    public void LeftMoveCamera()
    {
        moveCameraPosition = new Vector3(-(stageCS.GetMapSize().x - 20), 0, 0);
        nowCameraPosition += moveCameraPosition;
        StartCoroutine(LeftMove());
      //  mainCamera.transform.position = nowCameraPosition;
    }
    IEnumerator LeftMove()
    {
        Time.timeScale = 0;
        for (float f = 1f; f >= 0; f -= 0.1f)
        {
            mainCamera.transform.position += Vector3.left * ((stageCS.mapSize.x-colBox)/10);
            yield return null;
        }
        Time.timeScale = 1;
    }
    //上
    public void UpMoveCamera()
    {
        moveCameraPosition = new Vector3(0, stageCS.GetMapSize().y-20, 0);
        nowCameraPosition += moveCameraPosition;
        StartCoroutine(UpMove());
    }
    IEnumerator UpMove()
    {
        Time.timeScale = 0;
        for (float f = 1f; f >= 0; f -= 0.1f)
        {
            mainCamera.transform.position += Vector3.up * ((stageCS.mapSize.y-colBox)/10);
            yield return null;
        }
        Time.timeScale = 1;
    }
    //下
    public void DownMoveCamera()
    {
        moveCameraPosition = new Vector3(0, -(stageCS.GetMapSize().y - 20), 0);
        nowCameraPosition += moveCameraPosition;
        StartCoroutine(DownMove());
        //mainCamera.transform.position = nowCameraPosition;
    }
    IEnumerator DownMove()
    {
        Time.timeScale = 0;
        for (float f = 1f; f >= 0; f -= 0.1f)
        {
            mainCamera.transform.position += Vector3.down * ((stageCS.mapSize.y - colBox) / 10);
            yield return null;
        }
        Time.timeScale = 1;
    }
    #endregion


    //テスト用
    void testButton()
    {
        if (Input.GetKeyDown(KeyCode.A)&&mapNumber<4)
        {
            mapNumber++;
        }
        else if(Input.GetKeyDown(KeyCode.S)&&mapNumber>0)
        {
            mapNumber--;
        }

        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            RightMoveCamera();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LeftMoveCamera();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            UpMoveCamera();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            DownMoveCamera();
        }
    }

}
