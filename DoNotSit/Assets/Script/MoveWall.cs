using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWall : MonoBehaviour
{
    public float speed;
    public bool isReturn;
    void Start()
    {
        isReturn = false;
    }

    // Update is called once per frame
    void Update()
    {
        Move(speed);
    }


    public void Move(float speed)
    {
        Vector3 pos = transform.position;
        if (!isReturn)
        {
            pos.x += speed * Time.deltaTime;
        }
        else
        {
            pos.x -= speed * Time.deltaTime;
        }
        pos.z = 0;
        transform.position = pos;
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
            isReturn = !isReturn;
        }
    }
}
