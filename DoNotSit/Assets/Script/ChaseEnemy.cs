using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseEnemy : MonoBehaviour
{
    CameraSample camera;
    public int enemyHitLimit;
    private int hitCnt;
    // Start is called before the first frame update
    void Start()
    {
        camera = transform.parent.GetComponent<CameraSample>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hitCnt >= enemyHitLimit)
        {
           //下がる処理ここに

            hitCnt = 0;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "BlowAway")
        {
            hitCnt++;
        }
    }
}
