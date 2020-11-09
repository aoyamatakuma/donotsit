using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public ScrollState currentScrollState; //現在の状態
    public float delay;
    float cnt;

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
            ScrollState scrollState = ScrollState.Right;
            switch (scrollState)
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
 
    
}
