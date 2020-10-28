using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Direction_RightLeft : MonoBehaviour
{
    bool isBurst;
    public float burstSpeed;
    private CameraShakeScript camera;
    public List<GameObject> effects;

    GameObject playerObj;
    void Start()
    {
        isBurst = false;
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShakeScript>();
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

        transform.position += transform.right * burstSpeed * Time.deltaTime;
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
}
