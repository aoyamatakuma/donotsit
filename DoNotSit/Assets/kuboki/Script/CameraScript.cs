using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //ステージスクリプトコンポーネント
    [SerializeField, Header("ステージスクリプト")]
    private StageScript stageCS;

    void Start()
    {
        //カメラのスタート地点を設定
        startCameraPosition = Vector3.forward * -78;
        nowCameraPosition = new Vector3(stageCS.GetMapSize().x / 2, stageCS.GetMapSize().y / 2, -78);
        mainCamera.transform.position = nowCameraPosition;        
    }

    void Update()
    {
        testButton();
    }

    //カメラの視点移動処理
    #region CameraMove

    //右
    void RightMoveCamera()
    {
        moveCameraPosition = new Vector3(stageCS.GetMapSize().x - 20, 0, 0);
        nowCameraPosition += moveCameraPosition;
        StartCoroutine(RightMove());
       // mainCamera.transform.position = nowCameraPosition;
    }
    IEnumerator RightMove()
    {
        for(float f = 1f; f>=0;f-=0.1f)
        {
            mainCamera.transform.position += Vector3.right*14;
            yield return null;
        }
    }
    //左
    void LeftMoveCamera()
    {
        moveCameraPosition = new Vector3(-(stageCS.GetMapSize().x - 20), 0, 0);
        nowCameraPosition += moveCameraPosition;
        StartCoroutine(LeftMove());
      //  mainCamera.transform.position = nowCameraPosition;
    }
    IEnumerator LeftMove()
    {
        for (float f = 1f; f >= 0; f -= 0.1f)
        {
            mainCamera.transform.position += Vector3.left * 14;
            yield return null;
        }
    }
    //上
    void UpMoveCamera()
    {
        moveCameraPosition = new Vector3(0, stageCS.GetMapSize().y-20, 0);
        nowCameraPosition += moveCameraPosition;
        StartCoroutine(UpMove());
        //mainCamera.transform.position = nowCameraPosition;
    }
    IEnumerator UpMove()
    {
        for (float f = 1f; f >= 0; f -= 0.1f)
        {
            mainCamera.transform.position += Vector3.up * 7;
            yield return null;
        }
    }
    //下
    void DownMoveCamera()
    {
        moveCameraPosition = new Vector3(0, -(stageCS.GetMapSize().y - 20), 0);
        nowCameraPosition += moveCameraPosition;
        StartCoroutine(DownMove());
        //mainCamera.transform.position = nowCameraPosition;
    }
    IEnumerator DownMove()
    {
        for (float f = 1f; f >= 0; f -= 0.1f)
        {
            mainCamera.transform.position += Vector3.down * 7;
            yield return null;
        }
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
