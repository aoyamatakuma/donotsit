using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWall : MonoBehaviour
{
    public float speed;
    public bool isReturn;
    public float timer;//タイマー
    public float returntimer;
    void Start()
    {
        isReturn = false;
    }

    // Update is called once per frame
    void Update()
    {
        Move(gameObject,speed);
        timer += 1.0f * Time.deltaTime;
        Timer();
    }


    public void Move(GameObject obj,float speed)
    {
        Vector3 pos = obj.transform.position;
        if (isReturn == false)
        {
            pos.x += speed * Time.deltaTime;
        }
        else if(isReturn==true)
        {
            pos.x -= speed * Time.deltaTime;
        }
        obj.transform.position = pos;
       
    }
    void Timer()
    {
        if (timer >= returntimer && isReturn == false)
        {
            timer = 0;
            isReturn = true;
        }
        if (timer >= returntimer && isReturn == true)
        {
            timer = 0;
            isReturn = false;
        }
    }
}
