using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTime : MonoBehaviour
{
    public bool slowMotion;
    float slowTime;
    public float slowEnd;
    private CameraShakeScript camera;
    private FollowCamera folcamera;
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShakeScript>();
        folcamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowCamera>();
    }
    void Update()
    {
        if (Time.timeScale == 0.3f)
        {
            slowTime++;
            slowMotion = true;
            camera.slowFlag = true;
            folcamera.zoom = true;
            if (slowTime >= slowEnd)
            {
                Time.timeScale = 1f;
                slowTime = 0;
                slowMotion = false;
                camera.slowFlag = false;
                folcamera.zoom = false;
                folcamera.shack = true;
            }
        }
    }
}
