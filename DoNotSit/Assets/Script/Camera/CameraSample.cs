using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScrollState
{
    Right,
    Left,
    Up,
    Down
}
public class CameraSample : MonoBehaviour
{
    public float speed;
    public GameObject player;
    public GameObject block_Right;
    public GameObject block_Left;
    public GameObject block_Up;
    public GameObject block_Down;
    public ScrollState currentScrollState; //現在の状態
    public float delay;
    public float speedDefalut;
    float cnt;
    //レベル系
    public int level = 1;//レベル
    public float exp;//経験値
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(player.transform.position.x,
            transform.position.y,
            transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        cnt += Time.deltaTime;
        if (cnt > delay)
        {
            ScrollState scrollState=ScrollState.Right;
            switch(scrollState)
            {
                case ScrollState.Right:
                    Right();
                    break;
                case ScrollState.Left:
                    Left();
                    break;
                case ScrollState.Up:
                    Up();
                    break;
                case ScrollState.Down:
                    Down();
                    break;
            }
            LevelUp();
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
        if (level == 1 && exp >= 5.0f)//レベル2
        {
            level += 1;
            speed *= 1.2f;
        }
        if (level == 2 && exp >= 10.0f)//レベル3
        {
            level += 1;
            speed = speedDefalut;
            if (speed == speedDefalut)
            {
                speed *= 1.6f;
            }
        }
        if (level == 3 && exp >= 15.0f)//レベル4
        {
            level += 1;
            speed = speedDefalut;
            if (speed == speedDefalut)
            {
                speed *= 2.0f;
            }
        }
        if (level == 4 && exp >= 20.0f)//レベル5
        {
            level += 1;
            speed = speedDefalut;
            if (speed == speedDefalut)
            {
                speed *= 2.4f;
            }
        }

    }
}
