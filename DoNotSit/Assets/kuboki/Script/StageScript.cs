using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageScript : MonoBehaviour
{
    //ステージのマップ数の設定
    [SerializeField]
    private int mapWidth;//横幅
    [SerializeField]
    private int mapHeight;//縦幅
    //ステージのマップをリスト化
    private int[] Map;
    //マップサイズ
    private Vector2 mapSize = new Vector2(160, 90);
    //マップの境目の判定を生成
    [SerializeField]
    private GameObject widthObj;
    [SerializeField]
    private GameObject heightObj;
    [SerializeField]
    private GameObject wallObj;

    

    // Start is called before the first frame update
    void Start()
    {
        CreateMap();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void CreateMap()
    {
        Map = new int[mapWidth*mapHeight];
        //マップの番号を設定
        for (int i = 0; i < Map.Length; i++)
        {
            Map[i] = i;
        }
        //横の当たり判定生成
        for (int w = 1; w < mapWidth; w++)
        {
            GameObject widthBox = Instantiate(widthObj, 
                new Vector3((mapSize.x - 20) * w + 10, ((mapSize.y - 20) * mapHeight + 20) / 2, 0),Quaternion.identity);
            widthBox.transform.localScale = new Vector3(20, (mapSize.y - 20) * mapHeight + 20, 0);
            widthBox.transform.parent = transform;
        }
        //縦の当たり判定の生成
        for (int h = 1; h < mapHeight; h++)
        {
            GameObject heightBox = Instantiate(heightObj,
                new Vector3(((mapSize.x - 20) * mapWidth + 20) / 2, (mapSize.y - 20) * h + 10, 0), Quaternion.identity);
            heightBox.transform.localScale = new Vector3((mapSize.x - 20) * mapWidth + 20, 20, 0);
            heightBox.transform.parent = transform;
        }


        //外枠の生成
        //左
        GameObject wall = Instantiate(wallObj, 
            new Vector3(0, ((mapSize.y - 20) * mapHeight + 20) / 2,0), Quaternion.identity);
        wall.transform.localScale = new Vector3(1, (mapSize.y - 20) * mapHeight + 20, 5);
        wall.transform.parent = transform;
        //右
        GameObject wall2 = Instantiate(wallObj,
    new Vector3((mapSize.x-20)*mapWidth+20, ((mapSize.y - 20) * mapHeight + 20) / 2, 0), Quaternion.identity);
        wall2.transform.localScale = new Vector3(1, (mapSize.y - 20) * mapHeight + 20, 5);
        wall2.transform.parent = transform;
        //下
        GameObject wall3 = Instantiate(wallObj,
            new Vector3(((mapSize.x - 20) * mapWidth + 20) / 2, 0, 0), Quaternion.identity);
        wall3.transform.localScale = new Vector3((mapSize.x - 20) * mapWidth + 20,1, 5);
        wall3.transform.parent = transform;
        //上
        GameObject wall4 = Instantiate(wallObj,
    new Vector3(((mapSize.x - 20) * mapWidth + 20) / 2, (mapSize.y-20)*mapHeight+20, 0), Quaternion.identity);
        wall4.transform.localScale = new Vector3((mapSize.x - 20) * mapWidth + 20, 1, 5);
        wall4.transform.parent = transform;

    }

    #region Getter

    //1マップのの画面サイズ取得
    public Vector2 GetMapSize()
    {
        return mapSize;
    }
    //横のマップ数取得
    public int GetMapWidth()
    {
        return mapWidth;
    }
    //縦のマップ数を取得
    public int GetMapHeight()
    {
        return mapHeight;
    }

    //マップ全体の枠の描画
    public void GetDrawMap()
    {
        Vector3 originMapVec = new Vector3(mapSize.x / 2, mapSize.y / 2, 0);
        //マップの横の判定を生成
        Gizmos.color = Color.green;
        for (int w = 1; w < mapWidth; w++)
        {
            Gizmos.DrawCube(
                new Vector3((mapSize.x - 20) * w + 10, ((mapSize.y - 20) * mapHeight + 20) / 2, 0),
                new Vector3(20, (mapSize.y - 20) * mapHeight + 20, 0));
        }
        //マップの縦の判定を生成
        Gizmos.color = Color.yellow;
        for (int h = 1; h < mapHeight; h++)
        {
            Gizmos.DrawCube(new Vector3(((mapSize.x - 20) * mapWidth + 20) / 2, (mapSize.y - 20) * h + 10, 0),
                new Vector3((mapSize.x - 20) * mapWidth + 20, 20, 0));
        }
        //マップ全体の枠を表示
        Gizmos.color = Color.white;
        for (int i = 0; i < mapWidth * mapHeight; i++)
        {
            Gizmos.DrawWireCube(originMapVec +
                new Vector3(i % mapWidth * (mapSize.x - 20), i / mapWidth % mapHeight * (mapSize.y - 20), 0),
                mapSize);
        }
        //マップ移動時の指標の線
        Gizmos.color = Color.red;
        for (int w = 1; w < mapWidth; w++)
        {
            Gizmos.DrawCube(
                new Vector3((mapSize.x - 20) * w + 10, ((mapSize.y - 20) * mapHeight + 20) / 2, 0),
                new Vector3(1, (mapSize.y - 20) * mapHeight + 20, 0));
        }
        for (int h = 1; h < mapHeight; h++)
        {
            Gizmos.DrawCube(new Vector3(((mapSize.x - 20) * mapWidth + 20) / 2, (mapSize.y - 20) * h + 10, 0),
                new Vector3((mapSize.x - 20) * mapWidth + 20, 1, 0));
        }


    }

    #endregion



}
