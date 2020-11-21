using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public float lifeTime;
    private float cnt;
    public bool isChaseHit;
    // Start is called before the first frame update
    void Start()
    {
        cnt = 0;
    }

    // Update is called once per frame
    void Update()
    {
        cnt += Time.deltaTime;
        if(cnt > lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
