﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject effect;
    public GameObject chaseEffect;
    public float bombRange;
    private CameraShakeScript camera;
    bool isChaseHit;
    private SlowTime slow;
    // Start is called before the first frame update
    void Start()
    {
       
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShakeScript>();
        slow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SlowTime>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player" || col.gameObject.tag == "Bomb" )
        {
            effect.GetComponent<SphereCollider>().radius = bombRange;
            camera.Shake(camera.durations, camera.magnitudes);
            Instantiate(effect, transform.position, transform.rotation);
            effect.GetComponent<AudioSource>().Play();
            slow.slowMotion = false;
            Destroy(gameObject);
        }
        if (col.gameObject.tag == "ChaseEnemy")
        {
           // effect.GetComponent<Effect>().isChaseHit = true;
            //camera.Shake(camera.durations, camera.magnitudes);
            Instantiate(chaseEffect, transform.position, transform.rotation);
            effect.GetComponent<AudioSource>().Play();
            Destroy(gameObject);
        }
    }
}
