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
    void Start()
    {
        isBurst = false;
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShakeScript>();
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

        if (isRight)
        {
            transform.position += (transform.right + dir) * burstSpeed * Time.deltaTime;
        }
        else
        {
            transform.position += (-transform.right + dir) * burstSpeed * Time.deltaTime;
        }
       
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            PlayerControl player = col.gameObject.GetComponent<PlayerControl>();
            if (player.currentPlayerState == PlayerState.Attack)
            {
                //playerObj = col.gameObject;
                isBurst = true;
                camera.Shake(camera.durations, camera.magnitudes);
            }
        }

        if (col.gameObject.tag == "Bomb")
        {
            isBurst = true;
            camera.Shake(camera.durations, camera.magnitudes);
        }

        if ((col.gameObject.tag == "Wall" || col.gameObject.tag == "Enemy") && isBurst)
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
