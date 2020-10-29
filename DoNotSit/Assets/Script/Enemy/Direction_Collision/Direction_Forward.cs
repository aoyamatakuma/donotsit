using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Direction_Forward : MonoBehaviour
{
    bool isBurst;
    public float maxBurstSpeed;
    public float minBurstSpeed;
    private float burstSpeed;
    public float lifeTime;
    private GameObject camera;
    public List<GameObject> effects;
    float cnt;
    Renderer render;
    PlayerControl player;
    public int score;

    void Start()
    {
        cnt = 0;
        isBurst = false;
        burstSpeed = Random.Range(minBurstSpeed, maxBurstSpeed);
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        render = GetComponent<Renderer>();
    }

    void Update()
    {
        Move();
        if(cnt > lifeTime)
        {
            Death();
        }
    }

    void Move()
    {
        if (!isBurst)
        {
            return;
        }
       
        cnt += Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(camera.transform.position.x,
            camera.transform.position.y,
            camera.transform.position.z), burstSpeed);
    }

    void OnTriggerEnter(Collider col)
    {
        CameraShakeScript cam = camera.GetComponent<CameraShakeScript>();
        if (col.gameObject.tag == "Player")
        {
            if (player.currentPlayerState == PlayerState.Attack)
            {
                player.Score(score);
                isBurst = true;
                cam.Shake(cam.durations, cam.magnitudes);
            }
        }

        if (col.gameObject.tag == "Bomb" || col.gameObject.tag == "BlowAway")
        {
            player.Score(score);
            isBurst = true;
            cam.Shake(cam.durations, cam.magnitudes);
        }

    }
   

    void Death()
    {
        //int num = Random.Range(0, effects.Count);
        //Instantiate(effects[num], transform.position, transform.rotation);
        //effects[num].GetComponent<AudioSource>().Play();
        Destroy(gameObject);
    }
}
