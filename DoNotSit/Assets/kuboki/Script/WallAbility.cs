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
    public Vector3 Height(bool i2)
    {
        Vector3 pos;
        if (i2)
        {
             pos = gameObject.transform.position + new Vector3(0, gameObject.transform.localScale.y / 2, 0);
        }
        else
        {
            pos = gameObject.transform.position - new Vector3(0, gameObject.transform.localScale.y / 2, 0);
        }      
        return pos;
    }
    //反射の判定用
    public Vector3 Width(bool i2)
    {
        Vector3 pos;
        if (i2)
        {
            pos = gameObject.transform.position + new Vector3(gameObject.transform.localScale.x / 2,0, 0);
        }
        else
        {
            pos = gameObject.transform.position - new Vector3(gameObject.transform.localScale.x / 2, 0,0);
        }
        return pos;
    }
}
