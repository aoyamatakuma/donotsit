using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Direction_RightLeft : MonoBehaviour
{
    public bool isRight;
    bool isBurst;
    public float maxBurstSpeed;
    public float minBurstSpeed;
    private float burstSpeed;
    private CameraShakeScript camera;
    public List<GameObject> effects;
    Vector3 dir;
    GameObject playerObj;
    PlayerControl player;
    public float score;
    void Start()
    {
        isBurst = false;
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShakeScript>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        burstSpeed = Random.Range(minBurstSpeed, maxBurstSpeed);
        RandomDirection();
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
        transform.position += (transform.right + dir) * burstSpeed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (player.currentPlayerState == PlayerState.Attack)
            {
                isBurst = true;
                camera.Shake(camera.durations, camera.magnitudes);
            }
        }

        if (col.gameObject.tag == "Bomb" || col.gameObject.tag == "BlowAway")
        {
            isBurst = true;
            camera.Shake(camera.durations, camera.magnitudes);
        }

        if ((col.gameObject.tag == "Wall" || col.gameObject.tag == "Enemy" || col.gameObject.tag == "ChaseEnemy") && isBurst)
        {
            player.Score(score);
            Death();
        }

        if(col.gameObject.tag == "ChaseEnemy")
        {
            Death();
        }
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "BackObj" && isBurst)
        {
            player.Score(score);
            Death();
        }
    }

    void Death()
    {
      
        int num = Random.Range(0, effects.Count);
        Instantiate(effects[num], transform.position, transform.rotation);
        effects[num].GetComponent<AudioSource>().Play();
        Destroy(gameObject);
    }

    void RandomDirection()
    {
        float num = Random.Range(0f, 1f);
        dir = new Vector3(0f, num, 0f);
        Debug.Log(dir);
    }
}
