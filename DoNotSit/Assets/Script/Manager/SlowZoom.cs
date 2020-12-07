﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowZoom : MonoBehaviour
{
    private FollowCamera camera;
    private CameraShakeScript shake;
    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowCamera>();
        shake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShakeScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            camera.zoom = true;
            shake.slowFlag = true;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")//ボム外した場合
        {
            camera.zoom = false;
            shake.slowFlag = false;
        }
    }

}
