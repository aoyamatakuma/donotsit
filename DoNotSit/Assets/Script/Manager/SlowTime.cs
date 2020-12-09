using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTime : MonoBehaviour
{
    public bool slowMotion;
    float slowTime;
    public float slowEnd;
    private CameraShakeScript camera;
    private PlayerControl player;
    private bool scale;
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShakeScript>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }
    void Update()
    {
        if (slowMotion == true)
        {
            if (player.select == false)
            {
                Time.timeScale = 0.3f;
            }
            if (player.select == true)
            {
                Time.timeScale = 0;
            }
            slowTime++;
            slowMotion = true;
            camera.slowFlag = true;
            scale = true;
        }
        if (slowMotion == false&&scale==true)
        {
            if (player.select == false)
            {
                Time.timeScale = 1f;
            }
            if (player.select == true)
            {
                Time.timeScale = 0;
            }
            camera.slowFlag = false;
            scale = false;
        }
    }
}
