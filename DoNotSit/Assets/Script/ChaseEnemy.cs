using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public enum ScrollEnemy
{
    Right,
    Left,
    Up,
    Down
}
public class ChaseEnemy : MonoBehaviour
{
    public int enemyHitLimit;
    public int hitCnt;
    public float speed;
    public ScrollState currentScrollState; //現在の状態
    public float delay;
    public float speedDefalut;
    float cnt;
    //レベル系
    public int level = 1;//レベル
    public float exp;//経験値
    public Text levelText;//レベルテキスト
    // Start is called before the first frame update
    void Start()
    {
        // camera = transform.parent.GetComponent<CameraSample>();
        // camera = GameObject.FindGameObjectWithTag("ChaseEnemy").GetComponent<CameraSample>();
        transform.position = new Vector3(transform.position.x,
             transform.position.y,
             transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        cnt += Time.deltaTime;
        if (cnt > delay)
        {
            ScrollEnemy scrollState = ScrollEnemy.Right;
            switch (scrollState)
            {
                case ScrollEnemy.Right:
                    Right();
                    break;
                case ScrollEnemy.Left:
                    Left();
                    break;
                case ScrollEnemy.Up:
                    Up();
                    break;
                case ScrollEnemy.Down:
                    Down();
                    break;
            }
            LevelUp();
            if (hitCnt >= enemyHitLimit)
            {
                //下がる処理ここに
                LevelDown();
                hitCnt = 0;
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "BlowAway")
        {
            hitCnt++;
        }
    }
    //右
    void Right()
    {
        Vector3 move = transform.position;
        move.x += speed * Time.deltaTime;//右
        transform.position = move;
    }
    //左
    void Left()
    {
        Vector3 move = transform.position;
        move.x -= speed * Time.deltaTime;//左
        transform.position = move;
    }
    //上
    void Up()
    {
        Vector3 move = transform.position;
        move.y += speed * Time.deltaTime;//上
        transform.position = move;
    }
    //下
    void Down()
    {
        Vector3 move = transform.position;
        move.y -= speed * Time.deltaTime;//下
        transform.position = move;
    }
    //レベルアップ
    public void LevelUp()
    {
        //経験値UP
        exp += 1.0f * Time.deltaTime;
        // レベル系
        levelText.text = "SPEEDLEVRL:" + level.ToString();
        if (level == 0)//レベル1
        {
            level = 1;
        }
        if (exp >= 5.0f)//レベル2
        {
            level = 2;
            if (level == 2)
            {
                speed = speedDefalut;
                if (speed == speedDefalut)
                {
                    speed *= 1.2f;
                }
            }
        }

        if (exp >= 10.0f)//レベル3
        {
            level = 3;
            if (level == 3)
            {
                speed = speedDefalut;
                if (speed == speedDefalut)
                {
                    speed *= 1.6f;
                }
            }
        }

        if (exp >= 15.0f)//レベル4
        {
            level = 4;
            if (level == 4)
            {
                speed = speedDefalut;
                if (speed == speedDefalut)
                {
                    speed *= 2.0f;
                }
            }
        }

        if (exp >= 20.0f)//レベル5
        {
            level = 5;
            if (level == 5)
            {
                speed = speedDefalut;
                if (speed == speedDefalut)
                {
                    speed *= 2.4f;
                }
            }
        }
    }
    //レベルダウン
    public void LevelDown()
    {
        exp -= 7.0f;
        level -= 1;
        speed = speedDefalut;
    }
}
