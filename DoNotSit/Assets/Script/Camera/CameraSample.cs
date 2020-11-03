using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSample : MonoBehaviour
{
    public float speed;
    public GameObject player;
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
        if(cnt > delay)
        {
            Vector3 move = transform.position;
            move.x += speed * Time.deltaTime;
            transform.position = move;
           LevelUp();
        }
      
    }
    //レベルアップ
    public void LevelUp()
    {
        //経験値UP
        exp +=1.0f * Time.deltaTime;
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
