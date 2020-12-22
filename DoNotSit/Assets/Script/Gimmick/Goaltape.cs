﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goaltape : MonoBehaviour
{
    public List<GameObject> tapeObj;
    private GameObject nearObj;
    private int nearNum;
    public float speed;
    bool isGoal;
    // Start is called before the first frame update
    void Start()
    {
        isGoal = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGoal)
        {
            MoveObj();
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            SelectObj(col.ClosestPoint(transform.position));
            isGoal = true;
        }
    }

    void SelectObj(Vector3 point)
    {
        float nearDis = 0;
        float tmpDis = 0;
        for(int i = 0; i < tapeObj.Count; i++)
        {
            tmpDis = Vector3.Distance(tapeObj[i].transform.position, point);

            if(nearDis ==0 || nearDis > tmpDis)
            {
                nearDis = tmpDis;
                nearNum = i;
            }
        }
    }

    void MoveObj()
    {
        Debug.Log(nearNum);
        for(int i = 0; i < tapeObj.Count; i++)
        {
            tapeObj[i].GetComponent<Cloth>().enabled = true;
            Vector3 pos = tapeObj[i].transform.position;
            float x = Random.Range(-1, 1);
            float z = Random.Range(-1, 1);
            if (i ==0)
            {
               pos.y += speed*Time.deltaTime;
            }
            else
            {
                pos.y -= speed * Time.deltaTime;
            }
            //pos.x += x * speed * Time.deltaTime;
           // pos.z += z * speed * Time.deltaTime;
            tapeObj[i].transform.position = pos;
        }
    }
}
