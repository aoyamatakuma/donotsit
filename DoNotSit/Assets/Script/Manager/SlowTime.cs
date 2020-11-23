using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTime : MonoBehaviour
{
    private bool slowMotion;
    float slowTime;
    public float slowEnd;
    private CameraShakeScript camera;
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShakeScript>();
    }
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
                camera.durations = 0.5f;
                camera.magnitudes = 0.5f;
            }
        }
    }
}
