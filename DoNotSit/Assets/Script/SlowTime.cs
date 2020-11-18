using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTime : MonoBehaviour
{
    private bool slowMotion;
    float slowTime;
    public float slowEnd;
    void Update()
    {
        if (Time.timeScale == 0.3f)
        {
            slowTime++;
            slowMotion = true;
            if (slowTime >= slowEnd)
            {
                Time.timeScale = 1f;
                slowTime = 0;
                slowMotion = false;
            }
        }
    }
}
