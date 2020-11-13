using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Direction_Back: MonoBehaviour
{
    bool isBurst;
    public float maxBurstSpeed;
    public float minBurstSpeed;
    float burstSpeed;
    private CameraShakeScript camera;
    public List<GameObject> effects;
    PlayerControl player;
    public float score;

    void Start()
    {
        burstSpeed = Random.Range(minBurstSpeed, maxBurstSpeed);
        isBurst = false;
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShakeScript>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if (!isBurst)
        {
            return;
        }
        Vector3 pos = transform.position;
       
        pos.z += burstSpeed * Time.deltaTime;
      
      
        transform.position = pos;
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player" )
        {        
            if(player.currentPlayerState == PlayerState.Attack)
            {
                isBurst = true;
                camera.Shake(camera.durations, camera.magnitudes);
            }
        }

        if(col.gameObject.tag == "Bomb" || col.gameObject.tag == "BlowAway")
        {
            isBurst = true;
            camera.Shake(camera.durations, camera.magnitudes);
        }

        if(col.gameObject.tag == "BackObj" && isBurst)
        {
           
            Death();
        }
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "BackObj" && isBurst)
        {
            Death();
        }
    }

    void Death()
    {
        player.Score(score);
        int num = Random.Range(0, effects.Count);
        Instantiate(effects[num], transform.position, transform.rotation);
        effects[num].GetComponent<AudioSource>().Play();
        Destroy(gameObject);
    }
}
