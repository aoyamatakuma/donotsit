using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWall : MonoBehaviour
{
    public float speedX;
    public float speedY;
    public bool isReturn;
    public float timer;//タイマー
    public float returntimer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move(gameObject,speedX,speedY);
        timer += 1.0f * Time.deltaTime;
        Timer();
    }

    public void Move(GameObject obj,float speedX,float speedY)
    {
        Vector3 pos = obj.transform.position;
        if (isReturn == false)
        {
            pos.x += speedX * Time.deltaTime;
            pos.y += speedY * Time.deltaTime;
        }
        else if(isReturn==true)
        {
            pos.x -= speedX * Time.deltaTime;
            pos.y -= speedY * Time.deltaTime;
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
