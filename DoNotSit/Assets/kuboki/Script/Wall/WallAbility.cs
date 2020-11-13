using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAbility : MonoBehaviour
{
    public int abilityNumber;
    
    
    // Update is called once per frame
    void Update()
    {
        
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
}
