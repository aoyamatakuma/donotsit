using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAbility : MonoBehaviour
{
    public int abilityNumber;
    public GameObject[] colObjs = new GameObject[4];


    private void Start()
    {
        ColObjectCheck(transform.up, 0);
        ColObjectCheck(-transform.up, 1);
        ColObjectCheck(transform.right, 2);
        ColObjectCheck(-transform.right, 3);
    }
    // Update is called once per frame
    void Update()
    {
        ColObjectCheck(transform.up, 0);
        ColObjectCheck(-transform.up, 1);
        ColObjectCheck(transform.right, 2);
        ColObjectCheck(-transform.right, 3);
    }
    void ColObjectCheck(Vector3 objVec,int num)
    {
        Ray ray = new Ray(transform.position, objVec);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit,8.0f))
        {
            if (hit.collider.tag == "Wall")
            {
                colObjs[num] = hit.collider.gameObject;               
            }
        }
        else
        {
            colObjs[num] = null;
        }
    }

    //反射の判定用
    public float Height(bool i2)
    {
        BoxCollider a = GetComponent<BoxCollider>();
        Vector3 pos;
        float y;

        if (i2)
        {
            pos = gameObject.transform.position + new Vector3(0, a.bounds.size.y / 2, 0);
            y = pos.y;
        }
        else
        {
            pos = gameObject.transform.position - new Vector3(0, a.bounds.size.y / 2, 0);
            y = pos.y;
        }
        y *= 10;
        y = Mathf.Floor(y) / 10;
        return y;
    }
    public Vector3 HeightPos(bool i2)
    {
        BoxCollider a = GetComponent<BoxCollider>();
        Vector3 pos;

        if (i2)
        {
            pos = gameObject.transform.position + new Vector3(0, a.bounds.size.y / 2, 0);
        }
        else
        {
            pos = gameObject.transform.position - new Vector3(0, a.bounds.size.y / 2, 0);
        }
        return pos;
    }
    //反射の判定用
    public float Width(bool i2)
    {
        BoxCollider a = GetComponent<BoxCollider>();
        Vector3 pos;
        float x;

        if (i2)
        {
            pos = gameObject.transform.position + new Vector3(a.bounds.size.x / 2, 0, 0);
            x = pos.x;
        }
        else
        {
            pos = gameObject.transform.position - new Vector3(a.bounds.size.x / 2, 0, 0);
            x = pos.x;
        }
        x *= 10;
        x = Mathf.Floor(x)/10;

        return x;
    }
    public Vector3 WidthPos(bool i2)
    {
        BoxCollider a = GetComponent<BoxCollider>();
        Vector3 pos;

        if (i2)
        {
            pos = gameObject.transform.position + new Vector3(a.bounds.size.x / 2, 0, 0);
        }
        else
        {
            pos = gameObject.transform.position - new Vector3(a.bounds.size.x / 2, 0, 0);
        }

        return pos;
    }
}
